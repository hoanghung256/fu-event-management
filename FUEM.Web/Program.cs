using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Application.UseCases.Event;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Persistence;
using FUEM.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FUEM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register controllers and views  
            builder.Services.AddControllersWithViews();

            // Get connection string  
            var connectionString = builder.Configuration.GetConnectionString("Default") 
                                            ?? throw new InvalidOperationException("Default connection string not found");

            // Register DbContext  
            builder.Services.AddDbContext<FUEMDbContext>(options => options.UseSqlServer(connectionString));

            registerRepositories(builder.Services);

            registerUseCases(builder.Services);

            var app = builder.Build();

            // Configure the HTTP request pipeline.  
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        private static void registerUseCases(IServiceCollection services)
        {
            services.AddTransient<IGetEventForGuest, GetEventForGuest>();
        }

        private static void registerRepositories(IServiceCollection services)
        {
            services.AddScoped<IEventRepository, EventRepository>();
        }
    }
}
