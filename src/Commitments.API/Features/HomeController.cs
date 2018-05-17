using Microsoft.AspNetCore.Mvc;

namespace Commitments.API.Features
{
    [Route("")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class HomeController
    {
        [HttpGet("health")]
        public IActionResult Health() 
            => new OkObjectResult(new {
                Status = "Healthy"
            });
        
        [HttpGet]
        public IActionResult Index()
            => new RedirectResult("~/swagger");
    }
}