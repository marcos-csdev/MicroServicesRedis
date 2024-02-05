using Discount.gRPC.Data;
using Discount.gRPC.Repositories;
using Discount.gRPC.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Discount.gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<DiscountContext>(option =>
            {
                option.UseNpgsql(builder.Configuration.GetConnectionString("npgsql"));
            });
            builder.Services
                .AddScoped<IDiscountRepository, DiscountRepository>();

            builder.Services.AddAutoMapper(typeof(Program));
            //AddAutoMapper(builder);
            builder.Services.AddGrpc();

            AddSeriLog(builder);

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            app.MapGrpcService<DiscountService>();
            app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

            app.Run();

            static void AddSeriLog(WebApplicationBuilder builder)
            {
                builder.Host.UseSerilog((fileContext, loggingConfig) =>
                {
                    loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                });
            }
            //void AddAutoMapper(WebApplicationBuilder builder)
            //{
            //    var mapper = MappingConfig.RegisterMaps().CreateMapper();
            //    builder.Services.AddSingleton(mapper);
            //    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //}
        }
    }
}