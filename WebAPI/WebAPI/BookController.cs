using Microsoft.AspNetCore.Mvc;

namespace WebAPI;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{
    [HttpGet]
    [Route("[action]")]
    public IActionResult List()
    {
        return Ok(new { message = "data book" });
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Create()
    {
        return Ok(new { message = "post method" });
    }

    [HttpPut]
    [Route("[action]/{id}")]
    public IActionResult Edit(int id)
    {
        return Ok(new { message = "id is " + id });
    }

    [HttpDelete]
    [Route("[action]/{id}")]
    public IActionResult Remove(int id)
    {
        return Ok(new { message = "id = " + id });
    }
}
