using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    [HttpGet]
    [Route("[action]")]
    public IActionResult list()
    {
        return Ok(new { message = "List" });
    }
}
