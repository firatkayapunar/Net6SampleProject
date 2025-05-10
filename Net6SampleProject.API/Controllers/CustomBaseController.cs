using Microsoft.AspNetCore.Mvc;
using Net6SampleProject.Core.DTOs;

namespace Net6SampleProject.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomBaseController : ControllerBase
    {
        [NonAction]
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response == null)
                return StatusCode(500, "Unexpected null response.");

            if (response.StatusCode == 204)
                return NoContent();

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
    }
}
