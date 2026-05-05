using Microsoft.EntityFrameworkCore;
using OnlineShopWebApp.Data;
using OnlineShopWebApp.Data.Models;
using OnlineShopWebApp.Data.Repository.Users;
using OnlineShopWebApp.Data.Repository.Carts;
using OnlineShopWebApp.Data.Repository.Orders;
using OnlineShopWebApp.Data.Repository.Products;
using OnlineShopWebApp.Data.Repository.Roles;
using OnlineShopWebApp.Data.Repository.Favorites;
using Microsoft.AspNetCore.Authentication.Cookies;
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
            builder.Services.AddScoped<IFavoritesRepository, FavoritesEfRepository>();
            builder.Services.AddMemoryCache();
            builder.Services.AddHttpClient<IRawgService, RawgService>(client =>
            {
                client.BaseAddress = new Uri("https://api.rawg.io/api/");
            });

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Authorization";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                });

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
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
            app.UseAuthentication();
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
