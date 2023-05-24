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

    [HttpGet("dairy/memory")]
    public IActionResult GetMemory([FromBody] MemoryRequest request)
    {
        var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();

        var command = dbConnection.CreateCommand();
        command.CommandText = $"SELECT * FROM Memories WHERE owner_phone_number = '{request.OwnerPhoneNumber}'";

        var memories = command.GetResults<Memory>().ToList();
        return Ok(memories);
    }

    [HttpGet("dairy/calender")]
    public IActionResult GetCalender()
    {
        return Ok();
    }
}