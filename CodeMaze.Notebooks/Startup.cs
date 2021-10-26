

using CodeMaze.Notebooks.Extensions;
using CodeMaze.Services;

using MazeCore.MongoDb.Extensions;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using System;

namespace CodeMaze.Notebooks
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
            //services.AddScoped(typeof(IRepository<>), typeof(DbContextRepository<>));
            //services.AddScoped<IDbClient, DbClient>();
            services.AddControllersWithViews();
            //services.AddScoped(typeof(BaseService));
            services.AddScoped(typeof(IUserService), typeof(UserService));
            services.AddScoped(typeof(INotebookService), typeof(NotebookService));

            services.AddAutoMapper(x => x.AddProfile(new MappingProfileHelper()));

            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.Cookie.Name = "CookieNotebook";
                options.IdleTimeout = System.TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            //services.AddAuthentication()
            //    .AddCookie("CookieNotebook", options =>
            //    {
            //        options.LoginPath = "/auth/login.html";
            //        options.LogoutPath = "/auth/logout.html";
            //        options.ExpireTimeSpan = TimeSpan.FromDays(1);
            //        options.SlidingExpiration = true;
            //        options.Cookie.HttpOnly = true;
            //        options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //    });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/auth/login.html";
                    options.LogoutPath = "/auth/logout.html";
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.SlidingExpiration = true;
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                });

            services.AddMongoDbContext(setting =>
                setting.AddConnectionString("mongodb://localhost:27017")
                       .AddDatabaseName("Notebooks"));

            services.ConfigureApplicationCookie(o =>
            {
                o.ExpireTimeSpan = System.TimeSpan.FromMinutes(10);
                o.SlidingExpiration = true;
            });

            //services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpContextAccessor();
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
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            });
            app.UseSession();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllers();
            });
        }
    }
}
