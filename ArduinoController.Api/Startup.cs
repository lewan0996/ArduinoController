using System.Text;
using System.Threading.Tasks;
using ArduinoController.Api.Auth;
using ArduinoController.Core.Contract.Auth;
using ArduinoController.Core.Contract.DataAccess;
using ArduinoController.Core.Contract.Services;
using ArduinoController.Core.Models;
using ArduinoController.Core.Models.Commands;
using ArduinoController.Core.Services;
using ArduinoController.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace ArduinoController.Api
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
            services.AddMvc();

            services.AddDbContext<AppDbContext>(opts =>
                opts.UseSqlServer(Configuration.GetConnectionString("Database"),
                    x => x.MigrationsAssembly("ArduinoController.DataAccess")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(opts =>
                    {
                        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    })
                .AddJwtBearer(opts =>
                {
                    var jwtConfig = Configuration.GetSection("Jwt");
                    opts.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = jwtConfig["Issuer"],
                        ValidAudience = jwtConfig["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig["Key"]))
                    };

                    opts.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            {
                                context.Response.Headers.Add("Token-Expired", "true");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            services.AddScoped<IUnitOfWork, EfUnitOfWork>();
            services.AddScoped<IAuthenticationService, JwtAuthenticationService>();
            services.AddScoped<IProcedureService>(
                p => new ProcedureService(p.GetService<IRepository<Command>>(),
                    p.GetService<IRepository<ArduinoDevice>>(),
                    p.GetService<IRepository<Procedure>>(),
                    Configuration.GetConnectionString("IoTHub"))
                );
            services.AddScoped<IDeviceService>
            (
                p => new DeviceService(p.GetService<IRepository<ArduinoDevice>>(),
                Configuration.GetConnectionString("IoTHub"))
            );
            services.AddScoped(typeof(IAuthorizationService<>), typeof(AuthorizationService<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
