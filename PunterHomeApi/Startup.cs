using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PunterHomeAdapters;
using PunterHomeAdapters.DataAdapters;
using PunterHomeApp.DataAdapters;
using PunterHomeApp.Services;
using PunterHomeDomain;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Services;

namespace PunterHomeApi
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

            services.AddDbContext<HomeAppDbContext>(op =>
                op.UseNpgsql("Host=localhost;Database=HomeAppDb;Username=postgres;Password=2964Lppos"));


            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IIngredientService, IngredientService>();

            services.AddScoped<IProductDataAdapter, ProductDataAdapter>();
            services.AddScoped<IRecipeDataAdapter, RecipeDataAdapter>();
            services.AddScoped<IIngredientDataAdapter, IngredientDataAdapter>();
            services.AddScoped<IShoppingListDataAdapter, ShoppingListDataAdapter>();
            services.AddScoped<IShoppingListService, ShoppingListService>();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            services.AddSwaggerGen(s =>
            {
                s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Punter Home Api",
                    Version = "v1"
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger v1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
