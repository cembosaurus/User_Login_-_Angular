using API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;
using Users.Data;

namespace Users
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
            services.AddDbContext<DataContext>(d => d.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddCors();
            services.AddScoped<IAuthRepository, AuthRepository>();
            // ... add authentication as a service:
            // ... specifying authentication scheme ...
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                // ... options in scheme ...
                .AddJwtBearer(options =>
                {
                    // ... get my SECRET string from 'APPSETTINGS.JSON' and dconvert it into ByteArray ...
                    var configuration = Configuration.GetSection("Cembo_Settings:Token").Value;
                    var byteArray = Encoding.ASCII.GetBytes(configuration);

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        // ... options server wants to validate against ...
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(byteArray), // ... KEY from my configuration ...
                        ValidateIssuer = false,
                        ValidateAudience = false

                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {

                // ... middleware to intercept exceptions ...
                app.UseExceptionHandler(builder => {
                    builder.Run(async httpContext => {

                        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                        var err = httpContext.Features.Get<IExceptionHandlerFeature>();
                        if(err != null)
                        {
                            httpContext.Response.AddMyCustomError(err.Error.Message);
                            await httpContext.Response.WriteAsync(err.Error.Message);
                        }
                    });
                });

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // ... this is for https ...
                //app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCors(c => c.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            // ... my authentication inserted into HTTP request pipeline. Order matters ...
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
