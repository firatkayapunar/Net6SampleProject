using Microsoft.AspNetCore.Diagnostics;
using Net6SampleProject.Core.DTOs;
using Net6SampleProject.Service.Exceptions;
using System.Text.Json;

namespace Net6SampleProject.API.Middlewares
{
    public static class UseCustomExceptionHandler
    {
        // Bu bir extension method.ASP.NET Core'un IApplicationBuilder arayüzüne yeni bir özel metot ekliyoruz. Yani bu sayede şunu yazabiliyoruz:
        // app.UseCustomException();

        public static void UseCustomException(this IApplicationBuilder app)
        {
            // Bu yapı, ASP.NET Core’un yerleşik global hata yakalama middleware’ini devreye alır. İçerisine tanımladığımız config.Run(...) bloğu yalnızca bir hata (exception) oluştuğunda çalışır. Normal akışta middleware zincirinde pasif olarak bekler, hata durumunda ise devreye girerek özel bir işlem yürütür.
            // Eğer bu middleware tanımlanmazsa, uygulama içerisinde oluşan hatalar kontrolsüz bir şekilde dışarı yansır. ASP.NET Core varsayılan olarak bu hataları yakalayamaz ve istemciye genellikle HTML formatında bir hata sayfası döner.
            // Bu durum, özellikle bir JSON API geliştiriyorsak büyük sorunlara yol açar. Çünkü frontend uygulamamız (örneğin React, Vue, Angular ya da mobil uygulama) gelen yanıtı JSON formatında beklerken, beklenmedik bir HTML içerik ile karşılaşır. Bu da JSON parse işleminin başarısız olmasına ve uygulamanın hata vermesine neden olur.
            app.UseExceptionHandler(config =>
            {
                // Eğer uygulama içinde bir yerde throw new Exception(...) gibi bir hata fırlatılırsa, ASP.NET Core zinciri durdurur, UseExceptionHandler devreye girer ve burada tanımladığımız Run(...) bloğu çalışır.
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";

                    // ASP.NET Core, her HTTP isteği için bir HttpContext nesnesi oluşturur.
                    // Oluşan exception bilgisi, ASP.NET Core tarafından otomatik olarak HttpContext.Features koleksiyonuna eklenir.
                    // IExceptionHandlerFeature, meydana gelen exception'ı (Error) tutan özel bir feature'dır.
                    // Bu satır, ASP.NET Core’un yakaladığı hataya (exception) erişmek için ilgili feature'ı alır.
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    //Exception’ın türüne göre uygun HTTP durum kodunu belirliyoruz. (exceptionFeature.Error is ClientSideException)
                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => StatusCodes.Status400BadRequest,
                        NotFoundException => StatusCodes.Status404NotFound,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    context.Response.StatusCode = statusCode;

                    var response = CustomResponseDto<NoContentDto>.Fail(exceptionFeature.Error.Message, statusCode);

                    // Aşağıdaki kod, response nesnesini JSON formatına çevirip HTTP response body’ye yazar.
                    // PropertyNamingPolicy: CamelCase ayarı sayesinde property isimleri frontend tarafında beklenen formata dönüşür (örneğin: IsSuccess → isSuccess).
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response, new JsonSerializerOptions()
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    }));
                });
            });
        }
    }
}
