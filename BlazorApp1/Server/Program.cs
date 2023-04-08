using AutoMapper;
using BlazorApp1.Server.Controllers;
using HomeApp.Shared;
using HomeAppDomain.AggregateRoots;
using HomeAppDomain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews().AddNewtonsoftJson();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IShoppingListRepository, InMemoryShoppingListRepo>();
builder.Services.AddSingleton<IShoppingListItemRepository, InMemoryShoppingListItemRepo>();


var mapperConfig = new MapperConfiguration(mf => {
    mf.CreateMap<ShoppingListItem, ShoppingListItemDto>();
    mf.CreateMap<ShoppingListItemDto, ShoppingListItem>();


    mf.CreateMap<ShoppingList, ShoppingListDto>();
    mf.CreateMap<ShoppingListDto, ShoppingList>();
});
var mapper = new Mapper(mapperConfig);

builder.Services.AddScoped(a => mapper);

var app = builder.Build();

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