using Microsoft.AspNetCore.Mvc;

namespace WeStandTogether.Backend.Controllers
{
    [ApiController]
    public class FormController : ControllerBase
    {

        [HttpGet("form")]
        public IActionResult GetForm()
        {
            return Ok(System.IO.File.ReadAllText("Form.txt"));
        }
    }
}
