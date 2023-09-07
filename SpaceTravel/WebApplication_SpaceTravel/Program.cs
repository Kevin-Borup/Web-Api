
using Microsoft.Extensions.DependencyInjection.Extensions;
using WebApplication_SpaceTravel.DataHandlers;
using WebApplication_SpaceTravel.Interfaces;
using WebApplication_SpaceTravel.Middleware;
using WebApplication_SpaceTravel.Services;
using System.Web.Http.ExceptionHandling;


namespace WebApplication_SpaceTravel
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
            builder.Services.AddSwaggerGen();

            builder.Services.AddTransient<IEncryptionService, EncryptorService>();
            builder.Services.AddTransient<IDataHandler, MongoDataHandler>();
            builder.Services.Replace(new ServiceDescriptor(typeof(IExceptionHandler), new ApiExceptionHandler()));

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });


            var app = builder.Build();
            app.UseSession();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            

            app.UseHttpsRedirection();


            app.UseAuthorization();

            app.UseMiddleware<ApiKeyMiddleware>();
            //app.UseMiddleware<ExceptionMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}