using System.Security.Principal;
using AutoMapper;
using BlazorApp1.Server.Controllers;
using HomeApp.Shared;
using HomeAppDomain.AggregateRoots;
using HomeAppDomain.Interfaces;
using HomeAppRepositories;
using HomeAppRepositories.Entities;
using HomeAppRepositories.Repositories;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

//builder.Services.AddScoped<IShoppingListRepository, InMemoryShoppingListRepo>();

builder.Services.AddDbContext<HomeAppContext>();

builder.Services.AddScoped<IShoppingListItemRepository, ShoppingListItemRepository>();


var mapperConfig = new MapperConfiguration(mf => {
    mf.CreateMap<ShoppingListItem, ShoppingListItemDto>();
    mf.CreateMap<ShoppingListItemDto, ShoppingListItem>();


    mf.CreateMap<JsonPatchDocument<ShoppingListItemDto>, JsonPatchDocument<ShoppingListItem>>();
    mf.CreateMap<Operation<ShoppingListItemDto>, Operation<ShoppingListItem>>();


    mf.CreateMap<ShoppingListItem, DbShoppingListItem>();
    mf.CreateMap<DbShoppingListItem, ShoppingListItem>();


    mf.CreateMap<ShoppingList, ShoppingListDto>();
    mf.CreateMap<ShoppingListDto, ShoppingList>();
});
var mapper = new Mapper(mapperConfig);

builder.Services.AddScoped(a => mapper);

var app = builder.Build();


var scope = app.Services.CreateScope();
scope.ServiceProvider.GetRequiredService<HomeAppContext>().Database.Migrate();
//builder.Services.AddControllers(options =>
//{
//    options.InputFormatters.Insert(0, MyJPIF.GetJsonPatchInputFormatter());
//});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();

    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();


public static class MyJPIF
{
    public static NewtonsoftJsonPatchInputFormatter GetJsonPatchInputFormatter()
    {
        var builder = new ServiceCollection()
            .AddLogging()
            .AddMvc()
            .AddNewtonsoftJson()
            .Services.BuildServiceProvider();

        return builder
            .GetRequiredService<IOptions<MvcOptions>>()
            .Value
            .InputFormatters
            .OfType<NewtonsoftJsonPatchInputFormatter>()
            .First();
    }
}