using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using PetShop.Core.ApplicationService;
using PetShop.Core.ApplicationService.Impl;
using PetShop.Core.DomainService;
using PetShop.Infrastructure.Data;
using PetShop.Infrastructure.Security;
using PetShop.Infrastructure.SQLLite.Data;

namespace PetShop.RestAPI
{
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment Env { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            Byte[] secretBytes = new byte[40];
            Random rand = new Random();
            rand.NextBytes(secretBytes);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductSQLRepository>();

            services.AddScoped<IProductTypeService, ProductTypeService>();
            services.AddScoped<IProductTypeRepository, ProductTypeSQLRepository>();

            services.AddScoped<IColorService, ColorService>();
            services.AddScoped<IColorRepository, ColorSQLRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserSQLRepository>();
            
            services.AddSingleton<IAuthenticationHelper>(new AuthenticationHelper(secretBytes));
            services.AddScoped<InitStaticData>();

            services.AddControllers().AddNewtonsoftJson(options =>
            { options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                options.SerializerSettings.MaxDepth = 3;
            });

            if (Env.IsDevelopment())
            {
                services.AddDbContext<ProductShopContext>(opt =>
                    {
                        opt.UseSqlite("Data Source=ProductShopApp.db");
                        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    }, 
                    ServiceLifetime.Transient);
            }

            else if (Env.IsProduction())
            {
                services.AddDbContext<ProductShopContext>(opt =>
                    {
                        opt.UseSqlServer(Configuration.GetConnectionString("defaultConnection"));
                        opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
                    },
                    ServiceLifetime.Transient);
            }

            services.AddCors(options => options.AddDefaultPolicy(
                builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); }
                ));

            services.AddSwaggerGen((options) => {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Product Shop",
                    Description = "A RestAPI for a product shop application",
                    Version = "v1"
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretBytes),
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductShop API");
            });

                app.UseDeveloperExceptionPage();
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var ctx = scope.ServiceProvider.GetService<ProductShopContext>();

                    if (Env.IsDevelopment())
                    {
                        ctx.Database.EnsureDeleted();
                        ctx.Database.EnsureCreated();

                        InitStaticData dataInitilizer = scope.ServiceProvider.GetRequiredService<InitStaticData>();
                        dataInitilizer.InitData();
                    }
                    else
                    {
                        {
                            ctx.Database.EnsureCreated();
                        }
                    }
                    
                }

            app.UseCors();

            //app.UseHttpsRedirection();

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
