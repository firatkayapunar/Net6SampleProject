using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Net6SampleProject.Core.DTOs;
using System.Net;

namespace Net6SampleProject.API.Filters
{
    public class ValidateFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 1. İstek(Request) API'ye gelir.
            // 2. Model Binding ve Validation çalışır → ModelState nesnesi oluşur.
            // 3. OnActionExecuting metodu çalışır. (ActionFilter devreye girer)
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState.Values.SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                // Result özelliği, ASP.NET'e “cevap hazır, Controller’a gitme” demektir. Eğer context.Result ayarlanırsa, ASP.NET Core action methodu çalıştırmaz.
                context.Result = new BadRequestObjectResult(CustomResponseDto<NoContentDto>.Fail(errors, (int)HttpStatusCode.BadRequest));
            }
        }
    }
}
