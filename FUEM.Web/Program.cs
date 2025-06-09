using FUEM.Application;
using FUEM.Infrastructure.Persistence;
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
            builder.Services.AddDbContextPool<FUEMDbContext>(options => options.UseSqlServer(connectionString));

            builder.AddRepositories();

            builder.AddUseCases();

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
    }
}
