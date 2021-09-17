using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using VinylApp.Infrastructure.UnitOfWork;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using System.Threading.Tasks;
using VinylApp.Domain.Models.VinylAppModels.AlbumAggregate;
using VinylApp.Api.Services;
using VinylApp.Infrastructure.Persistence.DbContexts;
using VinylApp.Infrastructure.Persistence.Repository;
using VinylApp.Infrastructure.Services.SpotifyService;
using VinylApp.Infrastructure.Services.AuthServices;

namespace VinylApp.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("VinylAppDb");
            var serverVersion = new MySqlServerVersion(ServerVersion.AutoDetect(connectionString));
            services.AddDbContext<VinylAppContext>(
                dbContextOptions => dbContextOptions.UseMySql(
                    connectionString,
                    serverVersion,
                    b => b.MigrationsAssembly(typeof(VinylAppContext).Assembly.FullName))
                .UseLazyLoadingProxies()
            );

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = _ => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddCors(options =>
            { 
                options.AddPolicy(name: "AllowedOrigins", builder =>
                {
                    builder.WithOrigins("https://localhost:3000");
                });
            });

            services
                .AddAuthentication("OAuth")
                .AddJwtBearer("OAuth", opts =>
                {
                   var symmetricKey = Encoding
                       .UTF8
                       .GetBytes(Configuration.GetSection("ServerCredentials").ToString() ?? string.Empty);
                   var key = new SymmetricSecurityKey(symmetricKey);
                    opts.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        IssuerSigningKey = key
                    };

                    opts.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Token = context.Request.Cookies["_bearer"];
                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IAlbumRepo, AlbumRepo>();
            services.AddScoped<IUserServices, UserServices>();
            services.AddScoped<ISpotifyTokenClient, SpotifyTokenClient>();
            services.AddScoped<ISpotifyRequestClient, SpotifyRequestClient>();
            services.AddScoped<IAuthModel, JwtAuth>();
            services.AddScoped<IAuthService, JwtService>();
            services.AddControllers();
        }

        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();
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
