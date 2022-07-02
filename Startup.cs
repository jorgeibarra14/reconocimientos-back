using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Reconocimientos.Interfaces;
using Reconocimientos.Models;
using Reconocimientos.Services;

using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.FileProviders;

namespace Reconocimientos
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
            var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            services.AddSingleton(emailConfig);

            services.AddScoped<IColaboradoresService, ColaboradoresService>();
            services.AddScoped<ICompetenciaService, CompetenciaService>();
            services.AddScoped<ICategoriasService, CategoriasService>();
            services.AddScoped<IReconocimientoService, ReconocimientoService>();
            services.AddScoped<IPuntosService, PuntosService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<IUsuarioRolService, UsuarioRolService>();
            services.AddScoped<INotificacionesService, NotificacionesService>();
            services.AddScoped<IOdsService, OdsService>();
            services.AddScoped<IPedidosService, PedidosService>();
            services.AddScoped<IProductosPedidoService, ProductosPedidoService>();
            services.AddScoped<IEstatusPedidoService, EstatusPedidoService>();
            services.AddScoped<IProductosService, ProductosService>();
            services.AddScoped<IPuestosService, PuestosService>();
            services.AddScoped<IAutorizadoresService, AutorizadoresService>();
            services.AddScoped<ILoginService, LoginService>();

            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     options.CheckConsentNeeded = context => true;
            //     options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
            //     options.OnAppendCookie = cookieContext =>
            //         CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            //     options.OnDeleteCookie = cookieContext =>
            //         CheckSameSite(cookieContext.Context, cookieContext.CookieOptions);
            // });
            //
            // services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //     .AddCookie(options =>
            //     {
            //         options.Cookie.IsEssential = true;
            //         options.AccessDeniedPath = "/access-denied";
            //         options.LoginPath = "/api/login/redirecting";
            //         options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            //         options.Cookie.SameSite = SameSiteMode.Unspecified;
            //         //options.ReturnUrlParameter = "mirh/fromhome";
            //     });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseExceptionHandler(new ExceptionHandlerOptions
            {
                ExceptionHandler = (c) =>
                {
                    var exception = c.Features.Get<IExceptionHandlerFeature>();
                    var statusCode = exception.Error.GetType().Name switch
                    {
                        "ArgumentException" => HttpStatusCode.BadRequest,
                        _ => HttpStatusCode.ServiceUnavailable
                    };

                    c.Response.StatusCode = (int)statusCode;
                    var content = Encoding.UTF8.GetBytes($"Error[{ exception.Error.Message}]");
                    c.Response.Body.WriteAsync(content, 0, content.Length);

                    return Task.CompletedTask;

                }
            });

            if (env.IsDevelopment()) app.UseDeveloperExceptionPage();
            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().SetIsOriginAllowed(_ => true).AllowCredentials());
            app.UseHttpsRedirection();
            app.UseRouting();

            // var cookiePolicyOptions = new CookiePolicyOptions
            // {
            //     MinimumSameSitePolicy = SameSiteMode.None,
            // };
            //
            // app.UseCookiePolicy(cookiePolicyOptions);
            app.UseAuthentication();
            //app.UseAuthorization();
            //app.UseHttpsRedirection();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "Docs")),
                RequestPath = "/files"
            });
        }

        private void CheckSameSite(HttpContext httpContext, CookieOptions options)
        {
            if (options.SameSite == SameSiteMode.None)
            {
                var userAgent = httpContext.Request.Headers["User-Agent"].ToString();
                if (DisallowsSameSiteNone(userAgent)) options.SameSite = SameSiteMode.Unspecified;
            }
        }

        private static bool DisallowsSameSiteNone(string userAgent)
        {
            // Cover all iOS based browsers here. This includes:
            //   - Safari on iOS 12 for iPhone, iPod Touch, iPad
            //   - WkWebview on iOS 12 for iPhone, iPod Touch, iPad
            //   - Chrome on iOS 12 for iPhone, iPod Touch, iPad
            // All of which are broken by SameSite=None, because they use the
            // iOS networking stack.
            // Notes from Thinktecture:
            // Regarding https://caniuse.com/#search=samesite iOS versions lower
            // than 12 are not supporting SameSite at all. Starting with version 13
            // unknown values are NOT treated as strict anymore. Therefore we only
            // need to check version 12.
            if (userAgent.Contains("CPU iPhone OS 12")
                || userAgent.Contains("iPad; CPU OS 12"))
                return true;

            // Cover Mac OS X based browsers that use the Mac OS networking stack.
            // This includes:
            //   - Safari on Mac OS X.
            // This does not include:
            //   - Chrome on Mac OS X
            // because they do not use the Mac OS networking stack.
            // Notes from Thinktecture:
            // Regarding https://caniuse.com/#search=samesite MacOS X versions lower
            // than 10.14 are not supporting SameSite at all. Starting with version
            // 10.15 unknown values are NOT treated as strict anymore. Therefore we
            // only need to check version 10.14.
            if (userAgent.Contains("Safari")
                && userAgent.Contains("Macintosh; Intel Mac OS X 10_14")
                && userAgent.Contains("Version/"))
                return true;

            // Cover Chrome 50-69, because some versions are broken by SameSite=None
            // and none in this range require it.
            // Note: this covers some pre-Chromium Edge versions,
            // but pre-Chromium Edge does not require SameSite=None.
            // Notes from Thinktecture:
            // We can not validate this assumption, but we trust Microsofts
            // evaluation. And overall not sending a SameSite value equals to the same
            // behavior as SameSite=None for these old versions anyways.
            if (userAgent.Contains("Chrome/5") || userAgent.Contains("Chrome/6")) return true;

            return false;
        }
    }
}