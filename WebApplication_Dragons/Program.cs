
//using WebApplication_Dragons.Middleware;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebApplication_Dragons.Extensions;

namespace WebApplication_Dragons
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.ConfigureSwaggerGenWithAuthentication();
            builder.ConfigureAuthentication();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("V1/swagger.json", "Product WebAPI");
                });
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseAuthentication();

            // Add configuration from jwt.json
            builder.Configuration.AddJsonFile("jwt.json", optional: false, reloadOnChange: true).AddEnvironmentVariables();

            app.MapControllers();

            app.Run();
        }
    }
}