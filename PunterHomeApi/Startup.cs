using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PunterHomeAdapters;
using PunterHomeAdapters.DataAdapters;
using PunterHomeAdapters.Models;
using PunterHomeApi.Shared;
using PunterHomeApp.DataAdapters;
using PunterHomeApp.Services;
using PunterHomeDomain;
using PunterHomeDomain.Commands.RecipeStepCommand;
using PunterHomeDomain.Commands.RecipeStepCommand.Requests;
using PunterHomeDomain.Interfaces;
using PunterHomeDomain.Models;
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

            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<RecipeStepAggregate, DbRecipeStep>().ForMember(m => m.Recipe, opt => opt.Ignore())
                                                                  .DisableCtorValidation();
                cfg.CreateMap<RecipeStepIngredient, DbRecipeStepIngredient>().ForMember(m => m.Product, opt => opt.Ignore())
                                                                                .ForMember(m => m.RecipeStep, opt => opt.Ignore());
                cfg.CreateMap<DbRecipeStepIngredient, RecipeStepIngredient>().ForMember(m => m.ProductName, opt => opt.MapFrom(u => u.Product != null ? u.Product.Name : string.Empty));
                cfg.CreateMap<DbRecipeStep, RecipeStepAggregate>().ConstructUsing(c => new RecipeStepAggregate(c.Id,c.RecipeId,c.Text,c.Order, c.Ingredients.Select(i => new RecipeStepIngredient
                {
                    RecipeStepId = i.RecipeStepId,
                    ProductId = i.ProductId,
                    UnitQuantity = (int)i.UnitQuantity,
                    UnitQuantityType = i.UnitQuantityType,
                    ProductName = i.Product.Name

                }).ToList())).DisableCtorValidation();

                cfg.CreateMap<RecipeStep, RecipeStepApiModel>().ReverseMap();
                cfg.CreateMap<RecipeStepIngredient, RecipeStepIngredientApiModel>().ForMember(m => m.ProductName, opt => opt.Ignore());
                cfg.CreateMap<RecipeStepIngredientApiModel, RecipeStepIngredient>();

                cfg.CreateMap<AddIngredientToRecipeStepRequest, AddIngredientToRecipeStep>().ReverseMap();
                cfg.CreateMap<RemoveIngredientFromRecipeStepRequest, RemoveIngredientFromRecipeStep>().ReverseMap();
            });

            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IIngredientService, IngredientService>();

            services.AddScoped<IProductDataAdapter, ProductDataAdapter>();
            services.AddScoped<IRecipeDataAdapter, RecipeDataAdapter>();
            services.AddScoped<IIngredientDataAdapter, IngredientDataAdapter>();
            services.AddScoped<IShoppingListDataAdapter, ShoppingListDataAdapter>();
            services.AddScoped<IShoppingListService, ShoppingListService>();
            services.AddScoped<ITagDataAdapter, TagDataAdapter>();
            services.AddScoped<IProductTagService, ProductTagService>();
            services.AddScoped<IRecipeStepRepository, EfRecipeStepRepository>();
            services.AddScoped<IRecipeStepCommanHandlers, RecipeStepCommanHandlers>();
            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });


            services.AddSwaggerDocument();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            // Register the Swagger generator and the Swagger UI middlewares
            app.UseOpenApi();
            app.UseSwaggerUi3();

            app.UseRouting();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
