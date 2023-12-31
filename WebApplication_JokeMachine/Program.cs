
using WebApplication_JokeMachine.DataHandlers;
using WebApplication_JokeMachine.Interfaces;
using WebApplication_JokeMachine.Middleware;
using WebApplication_JokeMachine.Services;

namespace WebApplication_JokeMachine
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
            builder.Services.AddSingleton<IDataHandler>(new LocalDataHandler());

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

            app.MapControllers();

            app.Run();
        }
    }
}