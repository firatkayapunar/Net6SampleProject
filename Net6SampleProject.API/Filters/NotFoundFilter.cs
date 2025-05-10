using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Core.Models;
using Net6SampleProject.Core.Services;
using System.Net;

namespace Net6SampleProject.API.Filters
{
    public class NotFoundFilter<TEntity, TDto> : IAsyncActionFilter
     where TEntity : BaseEntity
     where TDto : class
    {
        private readonly IService<TEntity, TDto> _service;

        public NotFoundFilter(IService<TEntity, TDto> service)
        {
            _service = service;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // ActionArguments, action metoduna gönderilen parametreleri bir  dictionary olarak tutar.

            // context.ActionArguments.TryGetValue("id", out var idObj) şunu yapar:
            // id adında bir parametre var mı diye bakar.
            // Varsa → değeri idObj isimli değişkene atar ve true döner.
            // Yoksa → idObj'ye bir şey atamaz, false döner.

            // out var idObj ne demek?
            // out: C#’ta normalde bir metot tek bir şey döndürür. Ama bazen bir metottan birden fazla bilgi almak isteriz. out sayesinde bir metot içinde üretilen değeri dışarıya aktarabiliriz.
            // var idObj: Bu değeri karşılayan değişken.

            if (!context.ActionArguments.TryGetValue("id", out var idObj))
            {
                var error = CustomResponseDto<NoContentDto>.Fail("ID parameter is missing.", 400);

                // Bu satır, isteğe verilecek yanıtı manuel olarak belirler.
                // ASP.NET Core bu sonucu gördüğünde, artık controller metoduna gitmez.
                // Yani, controller metodunun içindeki kodlar hiç çalıştırılmaz.
                // İstek burada sonlanır ve doğrudan client'a (kullanıcıya) bu yanıt gönderilir.
                context.Result = new BadRequestObjectResult(error);

                // context.Result ile sonucu belirledik.
                // Ama filtrenin çalışmaya devam etmesini engellemek için burada return; ile işlemi durduruyoruz.
                return;
            }

            // idObj is not int id ne demek? Bu da şunu yapar:
            // idObj değişkeni gerçekten bir int türünde mi?
            // Eğer evetse → id isminde bir int değişken oluştur.
            // Eğer değilse → true olur, yani hatalı sayılır.

            if (idObj is not int id)
            {
                var error = CustomResponseDto<NoContentDto>.Fail("ID must be a valid integer.", 400);

                context.Result = new BadRequestObjectResult(error);

                return;
            }

            var exists = await _service.AnyAsync(x => x.Id == id);

            if (!exists)
            {
                var error = CustomResponseDto<NoContentDto>.Fail($"{typeof(TEntity).Name} not found.", (int)HttpStatusCode.NotFound);

                context.Result = new NotFoundObjectResult(error);

                return;
            }

            // Bu next bir delegate'tir.
            // Action Filter'dan sonra çalıştırılacak olan asıl action metodunu temsil eder.
            // await next() demek, “Benim işim bitti, şimdi action metodunu çalıştır” anlamına gelir.
            await next();
        }
    }
}
