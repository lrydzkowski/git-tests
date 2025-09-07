using Microsoft.AspNetCore.Mvc;

namespace GitTests.Controllers;

[ApiController]
[Route("api/app")]
public class AppController : ControllerBase
{
    [HttpGet]
    public IActionResult GetAppInfo()
    {
        return Ok(
            new
            {
                App = "GitTests",
                Version = "v31",
                Test = "6"
            }
        );
    }
}
