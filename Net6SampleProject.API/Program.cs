using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Net6SampleProject.API.Filters;
using Net6SampleProject.API.Middlewares;
using Net6SampleProject.Core.Repositories;
using Net6SampleProject.Core.Services;
using Net6SampleProject.Core.UnitOfWorks;
using Net6SampleProject.Repository;
using Net6SampleProject.Repository.Repositories;
using Net6SampleProject.Repository.UnitOfWorks;
using Net6SampleProject.Service.Mappings;
using Net6SampleProject.Service.Services;
using Net6SampleProject.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(opt =>
{
    /*
    Bu satýr, tüm controller action’lara global düzeyde özel bir model doðrulama filtresi (ValidateFilterAttribute) ekler.
    Bu filtre, gelen model verisi geçerli deðilse (ModelState.IsValid == false), özelleþtirilmiþ bir iþlem yapmamýzý saðlar.

    - API projelerinde: Varsayýlan olarak [ApiController] attribute'u varsa, ASP.NET Core model geçersizse otomatik olarak  400 Bad Request ve 'application/problem+json' formatýnda hata döner. Ancak bu davranýþ aþaðýda devre dýþý býrakýldýðý için  artýk kontrol tamamen bu filtreye býrakýlmýþ olur. Bu sayede kendi özel hata mesajlarýmýzý JSON formatýnda dönebiliriz.

    - MVC (View tabanlý) projelerde: ASP.NET Core model doðruluðunu otomatik kontrol etmez. Geliþtirici manuel olarak `if (!ModelState.IsValid)` kontrolü yapar ve `return View(model)` gibi dönüþler saðlar. 

    Bu filtre sayesinde bu kontrolü controller içinde deðil, filtre seviyesinde merkezi olarak yapabiliriz.
    */
    opt.Filters.Add(new ValidateFilterAttribute());
});

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    /*
     [ApiController] attribute'u kullanýldýðýnda ASP.NET Core, gelen model verisi geçerli deðilse (ModelState.IsValid == false), otomatik olarak 400 Bad Request döner ve içeriði 'application/problem+json' türünde bir hata yapýsý olur.

     Bu yapý; type, title, status ve errors gibi alanlar içerir ve hata detaylarýný client'a anlamlý þekilde sunar. Ancak aþaðýdaki ayar ile bu varsayýlan davranýþ devre dýþý býrakýlýr:
      - Amaç: ASP.NET Core'un kendi otomatik model doðrulama ve hata üretme mekanizmasýný kapatmak, ve onun yerine yukarýda eklediðimiz ValidateFilterAttribute filtresiyle bu süreci tamamen özelleþtirmektir.
    */
    opt.SuppressModelStateInvalidFilter = true;
});

// ProductDtoValidator sýnýfýnýn bulunduðu assembly içindeki tüm validator sýnýflarýný otomatik olarak tarar ve kaydeder. Yani tek tek her IValidator<T> için services.AddScoped<IValidator<T>, TValidator>() yazmak gerekmez.
builder.Services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

builder.Services.AddScoped(typeof(NotFoundFilter<,>));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(IService<,>), typeof(Service<,>));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddScoped<IProductService, ProductServiceWithCaching>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddMemoryCache();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseSqlServer(builder.Configuration.GetConnectionString("SqlConnection"), option =>
    {
        // AppDbContext hangi projedeyse, migration dosyalarý da orada bulunacak.
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCustomException();

app.UseAuthorization();

app.MapControllers();

app.Run();
