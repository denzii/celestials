using Application;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Domain.Model;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("DevelopmentPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
                options.AddPolicy("ProductionPolicy", builder =>
                {
                    builder.WithOrigins("https://celestialsweb.netlify.app")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
            var config = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var dbConn = builder.Environment.IsDevelopment()
                ? config.GetValue<string>("DATABASE_CONNECTION")
                : getDbConnectionFromKeyVault();

            builder.Services.AddScoped(x => new Database { Connection = dbConn! });

            builder.Services.AddAppLayer()
                .AddInfraLayer(
                    builder.Environment.IsEnvironment("Test"), 
                    dbConn!,    
                    config.GetSection("Test:Database:Connection").Value!
                );

            var app = builder.Build();
            using (var serviceScope = app.Services.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetRequiredService<AppDbContext>();
                dbContext.Database.EnsureCreated();

                var shouldMigrate = config.GetValue<bool>(config["DATABASE_MIGRATE"]!);
                if (shouldMigrate)
                {

                    if (dbContext.Database.GetPendingMigrations().Any())
                    {
                        dbContext.Database.Migrate();
                    }
                }
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseCors("DevelopmentPolicy");
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseCors("ProductionPolicy");
                app.UseHttpsRedirection();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }

        private static string? getDbConnectionFromKeyVault()
        {
            string keyVaultName = "celestials-key-vault";
            string keyVaultUrl = $"https://{keyVaultName}.vault.azure.net/";

            var credential = new DefaultAzureCredential();
            var client = new SecretClient(new Uri(keyVaultUrl), credential);

            string secretName = "celestials-prod-db-connection";
            KeyVaultSecret secret = client.GetSecret(secretName);
            string connectionString = secret.Value;

            return connectionString.Substring(connectionString.IndexOf('=') + 1).Trim();
        }
    }
}


