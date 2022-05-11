using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BlazorPunterHomeApp.Data;
using Blazored.Modal;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using PunterHomeDomain.Interfaces;
using RazorShared;
using PunterHomeApiConnector.Interfaces;
using PunterHomeApiConnector.Clients;

namespace BlazorPunterHomeApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddBlazorise(opt =>
                {
                    opt.ChangeTextOnKeyPress = true;
                })
                .AddBootstrapProviders()
                .AddFontAwesomeIcons();
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredModal();
            services.AddSingleton<WeatherForecastService>();
            services.AddSingleton<ProductService>();
            services.AddSingleton<RecipeService>();
            services.AddSingleton<BlazorRecipeService>();
            services.AddSingleton<BlazorShoppingListService>();
            services.AddSingleton<IBarcodeScannerService, BarcodeScannerService>();
            services.AddSingleton<IBlazorTagService, BlazorTagService>();
            services.AddSingleton<IShoppingListApiConnector, ShoppingListApiConnector>();
            services.AddSingleton<IRecipeStepApiConnector, RecipeStepApiConnector>();
            services.AddSingleton<AppState>();
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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
