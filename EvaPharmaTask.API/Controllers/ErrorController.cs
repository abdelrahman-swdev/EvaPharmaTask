using EvaPharmaTask.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace EvaPharmaTask.API.Controllers
{
    [Route("errors/{code}")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorController : BaseApiController
    {
        public IActionResult Error(int code)
        {
            return StatusCode(code, new ApiResponse<bool>(code, false, default));
        }
    }
}
