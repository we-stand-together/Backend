using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeStandTogether.Backend.Models.Memory;
using WeStandTogether.Dapper;

namespace WeStandTogether.Backend.Controllers;

[Authorize]
[ApiController]
public class DairyController : ControllerBase
{
    public DairyController(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }
    
    private readonly DapperContext _dapperContext;

    [HttpGet("dairy/memories")]
    public IActionResult GetMemory([FromBody] MemoryRequest request)
    {
        var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();

        var command = dbConnection.CreateCommand();
        command.CommandText = $"SELECT * FROM Memories " +
                              $"WHERE date = DATE('{request.Date})'" +
                              $"AND owner_phone_number = '{User.Identity.Name}'";

        Console.WriteLine(command.CommandText);
        var memories = command.GetResults<Memory>().ToList();
        return Ok(memories);
    }

    [HttpGet("dairy/calender")]
    public IActionResult GetCalender()
    {
        return Ok();
    }
}