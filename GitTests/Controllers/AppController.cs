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
                Version = "v7.hotfix.1",
                Test = "1"
            }
        );
    }
}
