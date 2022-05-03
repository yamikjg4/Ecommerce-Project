using E_Commerce.Helper;
using E_Commerce.Models;
using E_Commerce.repositry;
using E_Commerce.services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

namespace E_Commerce
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddDbContext<Eshopcontext>(option => option.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            /*services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();*/
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<Eshopcontext>().AddDefaultTokenProviders();
            services.Configure<SMTPConfigModel>(Configuration.GetSection("SMTPConfig"));
            services.AddRazorPages().AddRazorRuntimeCompilation();
            services.AddScoped<IAccountrepostry, Accountrepostry>();
            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();
            services.AddScoped<Isendmail, sendmail>();
            services.AddScoped<IProductRepositry, ProductRepositry>();

            services.Configure<IdentityOptions>(option =>
            {
                option.Password.RequiredLength = 9;
                /*  option.Password.RequiredLength = 12;*/
                option.Password.RequiredUniqueChars = 1;
                option.Password.RequireUppercase = true;
                option.Password.RequireLowercase = true;
                option.Password.RequireDigit = true;
                option.Password.RequireNonAlphanumeric = true;
                option.SignIn.RequireConfirmedEmail = true;
                option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                option.Lockout.MaxFailedAccessAttempts = 5;
            });
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Login/";
                config.AccessDeniedPath = "/Account/AccessDenied";
            }
            );

        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                  name: "MyArea",
                  areaName: "Admin",
                  pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");
            });
        }
    }
}
