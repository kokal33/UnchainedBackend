using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UnchainedBackend.Data;
using UnchainedBackend.Helpers;
using UnchainedBackend.Repos;

namespace UnchainedBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Environment { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://billing.asteriskdoo.si",
                                                           "http://192.168.190.10", //TODO: Change ip to frontend
                                                           "http://localhost:4200")
                                      .AllowAnyHeader().AllowAnyMethod();
                                  });
            });
            services.AddDbContext<ApplicationContext>(options => options.UseNpgsql(Configuration["Data:DbContext:ConnectionString"]));

            services.AddControllersWithViews();
            services.AddScoped<IEthRepo, EthRepo>();
            services.AddScoped<IEthHelper, EthHelper>();
            services.AddScoped<IStorageRepo, StorageRepo>();
            services.AddScoped<IArtistsRepo, ArtistsRepo>();
            services.AddScoped<ITracksRepo, TracksRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationContext context)
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
            // Apply Migrations
            context.Database.Migrate();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
