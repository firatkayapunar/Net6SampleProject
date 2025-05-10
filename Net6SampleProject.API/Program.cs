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
    Bu sat�r, t�m controller action�lara global d�zeyde �zel bir model do�rulama filtresi (ValidateFilterAttribute) ekler.
    Bu filtre, gelen model verisi ge�erli de�ilse (ModelState.IsValid == false), �zelle�tirilmi� bir i�lem yapmam�z� sa�lar.

    - API projelerinde: Varsay�lan olarak [ApiController] attribute'u varsa, ASP.NET Core model ge�ersizse otomatik olarak  400 Bad Request ve 'application/problem+json' format�nda hata d�ner. Ancak bu davran�� a�a��da devre d��� b�rak�ld��� i�in  art�k kontrol tamamen bu filtreye b�rak�lm�� olur. Bu sayede kendi �zel hata mesajlar�m�z� JSON format�nda d�nebiliriz.

    - MVC (View tabanl�) projelerde: ASP.NET Core model do�rulu�unu otomatik kontrol etmez. Geli�tirici manuel olarak `if (!ModelState.IsValid)` kontrol� yapar ve `return View(model)` gibi d�n��ler sa�lar. 

    Bu filtre sayesinde bu kontrol� controller i�inde de�il, filtre seviyesinde merkezi olarak yapabiliriz.
    */
    opt.Filters.Add(new ValidateFilterAttribute());
});

builder.Services.Configure<ApiBehaviorOptions>(opt =>
{
    /*
     [ApiController] attribute'u kullan�ld���nda ASP.NET Core, gelen model verisi ge�erli de�ilse (ModelState.IsValid == false), otomatik olarak 400 Bad Request d�ner ve i�eri�i 'application/problem+json' t�r�nde bir hata yap�s� olur.

     Bu yap�; type, title, status ve errors gibi alanlar i�erir ve hata detaylar�n� client'a anlaml� �ekilde sunar. Ancak a�a��daki ayar ile bu varsay�lan davran�� devre d��� b�rak�l�r:
      - Ama�: ASP.NET Core'un kendi otomatik model do�rulama ve hata �retme mekanizmas�n� kapatmak, ve onun yerine yukar�da ekledi�imiz ValidateFilterAttribute filtresiyle bu s�reci tamamen �zelle�tirmektir.
    */
    opt.SuppressModelStateInvalidFilter = true;
});

// ProductDtoValidator s�n�f�n�n bulundu�u assembly i�indeki t�m validator s�n�flar�n� otomatik olarak tarar ve kaydeder. Yani tek tek her IValidator<T> i�in services.AddScoped<IValidator<T>, TValidator>() yazmak gerekmez.
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
        // AppDbContext hangi projedeyse, migration dosyalar� da orada bulunacak.
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
