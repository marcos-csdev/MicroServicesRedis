
using Serilog;
using ShoppingCartAPI.Repositories;
using StackExchange.Redis;

namespace ShoppingCartAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            AddRedis(builder);

            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            AddSeriLog(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();

            void AddSeriLog(WebApplicationBuilder builder)
            {
                builder.Host.UseSerilog((fileContext, loggingConfig) =>
                {
                    loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                });
            }
        }

        private static void AddRedis(WebApplicationBuilder builder)
        {
            // redis connection config 
            builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                var redisUrl = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
                return ConnectionMultiplexer.Connect(redisUrl!);
            });
            //builder.Services.AddDistributedMemoryCache();
            //builder.Services.AddStackExchangeRedisCache(options =>
            //{
            //    options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
            //    options.InstanceName = "LocalRedis";
            //});
        }
    }
}
