using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Users;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Orders;
using OnlineShopWebApp.Data.Repository.Products;
using OnlineShopWebApp.Data.Repository.Roles;
using OnlineShopWebApp.Services;
using Serilog;
namespace OnlineShopWebApp
{

    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, configuration) => configuration
                   .ReadFrom.Configuration(context.Configuration)
                   .Enrich.WithProperty("ApplicationName", "SteamKooper"));
            builder.Services.AddControllersWithViews();

            builder.Services.AddRazorPages();
            // builder.Services.AddSingleton<IProductsRepository, ProductsInMemoryRepository>();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IProductsRepository, ProductsEfRepository>();
            builder.Services.AddScoped<ICartsRepository, CartsEfRepository>();
            builder.Services.AddScoped<IOrdersRepository, OrdersEfRepository>();
            builder.Services.AddScoped<IUsersManager, UsersEfRepository>();
            builder.Services.AddScoped<IRolesRepository, RolesEfRepository>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddSession();
            builder.Services.AddScoped<Cart>();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                app.UseHsts();
            }
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseSerilogRequestLogging();
            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "MyArea",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();

        }
    }
}
