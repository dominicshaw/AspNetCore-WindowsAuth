using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WindowsAuth.Controllers;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.Extensions.Logging;

namespace WindowsAuth
{
    public class Startup
    {
        private static readonly string[] _origins =
        {
            "http://lonhapp04",
            "http://lonhapp04:3278",
            "http://lonhapp04.ttint.com:3278",
            "http://localhost",
            "http://localhost:3278",
            "http://localhost:4200"
        };

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", builder => builder.WithOrigins(_origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials()));

            services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
                .AddNegotiate();

            services.AddHttpContextAccessor();

            services.AddScoped<StaffPermissionsService>();
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseCors(builder => builder.WithOrigins(_origins).AllowAnyHeader().AllowAnyMethod().AllowCredentials());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
