using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WeStandTogether.Backend.Controllers;

[Authorize]
[ApiController]
public class FormController : ControllerBase
{
    [HttpGet("form")]
    public IActionResult GetForm()
    {
        Console.WriteLine("Get Form");
        return Ok(System.IO.File.ReadAllText("Form.json"));
    }
}