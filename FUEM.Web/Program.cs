using FUEM.Application;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Application.UseCases.UserUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using FUEM.Infrastructure.Persistence;
using FUEM.Infrastructure.Persistence.Repositories;
using FUEM.Web.Filters;
using FUEM.Web.Hubs;
using Microsoft.EntityFrameworkCore;

using FUEM.Application.UseCases.EventUseCases;
using FUEM.Infrastructure;

namespace FUEM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpContextAccessor(); // ✅ Đăng ký

            // Register controllers and views  
            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpContextAccessor();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(1);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<IGetAttendedEvents, GetAttendedEvents>();
            builder.Services.AddScoped<FUEM.Domain.Interfaces.Repositories.IFeedbackRepository, FUEM.Infrastructure.Persistence.Repositories.FeedbackRepository>();
            builder.Services.AddScoped<FUEM.Application.Interfaces.UserUseCases.IFeedback, FUEM.Application.UseCases.UserUseCases.FeedbackService>(); 
            // Get connection string  
            var connectionString = GetConnectionString(builder);
            //var connectionString = builder.Configuration.GetConnectionString("LocalConnection");

            // Register DbContext  
            builder.Services.AddDbContextPool<FUEMDbContext>(options => options.UseSqlServer(connectionString));

            builder.Services.Configure<MongoDbSettings>(builder.Configuration.GetSection("MongoDb"));

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

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins("https://fuem.azurewebsites.net") // 👈 your actual frontend domain
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials(); // 👈 needed for cookies or auth
                });
            });

            builder.Services.AddAuthorization();

            //builder.Services.AddScoped<InsertSignedFirebaseUrl>();

            builder.Services.AddControllers(options =>
            {
                options.Filters.Add<InsertSignedFirebaseUrl>();
            });

            builder.Services.AddSignalR();

            //builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true && options.AppendTrailingSlash);

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

            //app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseWebSockets();
            app.UseSession();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Authentication}/{action=Login}/{id?}");

            app.MapHub<ChatHub>("/chatHub");
            //app.MapHub<ChatHub>("/chatHub").RequireCors("AllowFrontend");


            app.Run();
        }



        private static string GetConnectionString(WebApplicationBuilder builder)
        {
            string server = builder.Configuration.GetConnectionString("Server");
            string database = builder.Configuration.GetConnectionString("Database");
            string username = builder.Configuration.GetConnectionString("UserId");
            string password = builder.Configuration.GetConnectionString("Password");

            return $"Server={server};Database={database};User ID={username};Password={password};TrustServerCertificate=True;";
        }
    }
}