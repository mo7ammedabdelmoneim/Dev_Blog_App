using Interface.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using source;
using Source.Models;
using Source.Services.Implementations;
using Source.Services.Interfaces;

namespace Interface
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationContext>(options =>
               options.UseSqlServer("Data Source=MOHAMED-ABDELMO\\SQLEXPRESS;Initial Catalog=MiniBlogAAppDB;Integrated Security=True;Encrypt=False;Trust Server Certificate=True")
               //options.UseSqlServer(builder.Configuration.GetConnectionString("Main"))
           );
            // register the Identity Service
            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Models.AppContext>();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {   // with Reset password Rules
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 4;
                options.Password.RequireDigit = false;
            }).AddEntityFrameworkStores<ApplicationContext>();

            //Custom Services
            builder.Services.AddScoped<IPostService,PostService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseMiddleware<ProfillingMiddleware>();

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
