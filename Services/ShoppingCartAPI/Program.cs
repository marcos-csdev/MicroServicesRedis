using Microsoft.Extensions.Configuration;
using Serilog;
using ShoppingCartAPI.gRPCServices;
using ShoppingCartAPI.Repositories;
using StackExchange.Redis;
using static Discount.gRPC.Protos.DiscountProtoService;

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

            builder.Services.AddGrpcClient<DiscountProtoServiceClient>(option =>
            {
                option.Address = new Uri(builder.Configuration.GetValue<string>("GrpcSettings:DiscountUrl")!);
            });
            builder.Services.AddScoped<IDiscountGrpcService, DiscountGrpcService>();
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

            static void AddSeriLog(WebApplicationBuilder builder)
            {
                builder.Host.UseSerilog((fileContext, loggingConfig) =>
                {
                    loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                });
            }
        }

        private static IConfigurationRoot GetConfig(WebApplicationBuilder builder)
        {
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var configuration = builder.Configuration
                 .AddJsonFile("appsettings.json", optional: false)
                 .AddJsonFile($"appsettings.{env}.json", optional: true)
                 .AddEnvironmentVariables()
                 .Build();

            return configuration;
        }

        private static void AddRedis(WebApplicationBuilder builder)
        {
            // redis connection config 
            //var config = GetConfig(builder);

            var redisUrl = builder.Configuration.GetConnectionString("Redis");
            //"redis-cache"
            builder.Services.AddSingleton<IConnectionMultiplexer>(opt =>
            {
                var options = new ConfigurationOptions
                {
                    EndPoints = { { "localhost", 6379 } },
                    User = "default",  // use your Redis user. More info https://redis.io/docs/management/security/acl/
                    Password = "nopass", // use your Redis password
                    Ssl = false,
                    SslProtocols = System.Security.Authentication.SslProtocols.None,

                };
                var connection = ConnectionMultiplexer.Connect(options);

                return connection;
            });



        }
    }
}
