using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Errors;

namespace Talabat.APIs.Controllers;
[Route("errors/{statusCode}")]
[ApiController]
[ApiExplorerSettings(IgnoreApi =true)]
public class ErrorsController : ControllerBase
{
    public ActionResult Error(int statusCode)
    {
        return NotFound(new ApiRequest(statusCode,"Endpoint Is Not Found"));
    }
}
