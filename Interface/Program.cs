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
                // Password settings (strong but user-friendly)
                options.Password.RequireDigit = true;                    // at least one number
                options.Password.RequireLowercase = true;                // at least one lowercase letter
                options.Password.RequireUppercase = true;                // at least one uppercase letter
                options.Password.RequireNonAlphanumeric = true;          // at least one special character
                options.Password.RequiredLength = 8;                     // minimum length
                options.Password.RequiredUniqueChars = 1;                // at least one unique character

                // User settings
                options.User.RequireUniqueEmail = true;                  // emails must be unique

                // Lockout settings (optional, security feature)
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;
            }).AddEntityFrameworkStores<ApplicationContext>();

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var google = builder.Configuration.GetSection("Authentication:Google");

                    options.ClientId = google["ClientId"]!;
                    options.ClientSecret = google["ClientSecret"]!;
                    options.CallbackPath = "/signin-google";
                });

            builder.Services.AddAutoMapper(typeof(Program));

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

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
