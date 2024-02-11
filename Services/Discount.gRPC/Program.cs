using Discount.gRPC.Data;
using Discount.gRPC.Repositories;
using Discount.gRPC.Services;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Serilog;

namespace Discount.gRPC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("npgsql");
            // Add services to the container.
            builder.Services.AddDbContext<DiscountContext>(option =>
            {
                option.UseNpgsql(connectionString);

            });

            builder.Services.Configure<DatabaseOptions>(options =>
            {
                options.ConnectionString = connectionString!;
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

            MigrateDatabase(app.Services, connectionString!);
            ApplyPendingMigrations();

            app.Run();

            static void AddSeriLog(WebApplicationBuilder builder)
            {
                builder.Host.UseSerilog((fileContext, loggingConfig) =>
                {
                    loggingConfig.WriteTo.File("logs\\log.log", rollingInterval: RollingInterval.Day);
                });
            }

            void ApplyPendingMigrations()
            {
                using var scope = app!.Services.CreateScope();
                var _db = scope.ServiceProvider.GetRequiredService<DiscountContext>();

                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();

                }
            }



        }

        private static void MigrateDatabase(IServiceProvider serviceProvider, string connectionString, int retries = 0)
        {

            try
            {
                using (var scope = serviceProvider.CreateScope())
                {

                    using var connection = new NpgsqlConnection(connectionString);
                    connection.Open();

                    using var command = new NpgsqlCommand
                    {
                        Connection = connection,
                    };
                    ExecuteCommand(command, "DROP TABLE IF EXISTS Coupon");

                    ExecuteCommand(command, @"CREATE TABLE Coupon(Id SERIAL PRIMARY KEY, 
                                                                ProductName VARCHAR(24) NOT NULL,
                                                                Description TEXT,
                                                                Amount INT)");

                    ExecuteCommand(command, "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('IPhone X', 'IPhone Discount', 150);");

                    ExecuteCommand(command, "INSERT INTO Coupon(ProductName, Description, Amount) VALUES('Samsung 10', 'Samsung Discount', 100);");

                }
            }
            catch (Exception)
            {
                if(retries > 0)
                {
                    Thread.Sleep(3000);
                    MigrateDatabase(serviceProvider, connectionString, --retries);
                }
            }
        }

        private static void ExecuteCommand(NpgsqlCommand command, string query)
        {
            command.CommandText = query;
            command.ExecuteNonQuery();
        }
    }
}