using Microsoft.Extensions.Options;
using Net6SampleProject.API.Models.Configurations;
using Net6SampleProject.MVC.Controllers;
using Net6SampleProject.MVC.Models.Configurations;

var builder = WebApplication.CreateBuilder(args);

// CORS ayarlarý
builder.Services.Configure<CorsSettings>(
    builder.Configuration.GetSection("CorsSettings"));

// Add services to the container.
builder.Services.AddControllersWithViews();

// Bu satýrýn amacý, ProductsController veya CategoriesController sýnýfýnýn constructor’ýnda ihtiyaç duyduðu HttpClient nesnesini Dependency Injection (DI) ile saðlamak.
builder.Services.AddHttpClient<ProductsController>();
builder.Services.AddHttpClient<CategoriesController>();

builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection("ApiSettings"));

var app = builder.Build();

var corsSettings = app.Services.GetRequiredService<IOptions<CorsSettings>>().Value;

app.UseCors(policy =>
{
    policy.WithOrigins(corsSettings.AllowedOrigins.ToArray())
          .AllowAnyHeader()
          .AllowAnyMethod();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
