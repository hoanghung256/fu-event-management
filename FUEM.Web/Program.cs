using FUEM.Application;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Application.UseCases.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Persistence;
using FUEM.Infrastructure.Persistence.Repositories;
using FUEM.Web.Filters;
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

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1); 
                options.Cookie.HttpOnly = true; 
                options.Cookie.IsEssential = true; 
            });

            // Get connection string  
            var connectionString = builder.Configuration.GetConnectionString("Default") 
                                            ?? throw new InvalidOperationException("Default connection string not found");

            // Register DbContext  
            builder.Services.AddDbContextPool<FUEMDbContext>(options => options.UseSqlServer(connectionString));

            builder.AddRepositories();

            builder.AddUseCases();

            builder.Services
                .AddAuthentication("Cookies")
                .AddCookie("Cookies", options =>
                {
                    options.LoginPath = "/Authentication/Login";
                    options.AccessDeniedPath = "/Error/403";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;
                });

            builder.Services.AddAuthorization();

            builder.Services.AddScoped<InsertSignedFirebaseUrl>();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<InsertSignedFirebaseUrl>();
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.  
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/Exception");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.  
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.UseSession();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
