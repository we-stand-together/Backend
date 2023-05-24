using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeStandTogether.Backend.Models.Memory;
using WeStandTogether.Dapper;

namespace WeStandTogether.Backend.Controllers;

[Authorize]
[ApiController]
public class DiaryController : ControllerBase
{
    public DiaryController(DapperContext dapperContext)
    {
        _dapperContext = dapperContext;
    }

    private readonly DapperContext _dapperContext;

    [HttpGet("diary/memories")]
    public IActionResult GetMemory([FromBody] MemoryRequest request)
    {
        var memoryDate = DateOnly.Parse(request.Date);

        Console.WriteLine("Diary/Memories");
        var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();

        var command = dbConnection.CreateCommand();
        var startOfDay = memoryDate.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd HH:mm:ss");
        var endOfDay = memoryDate.ToDateTime(TimeOnly.MaxValue).ToString("yyyy-MM-dd HH:mm:ss");

        command.CommandText = $"SELECT * FROM Memories " +
                              $"WHERE date >= '{startOfDay}'" +
                              $"AND date <= '{endOfDay}'" +
                              $"AND owner_phone_number = '{User.Identity.Name}';";

        Console.WriteLine(command.CommandText);

        var resultsReader = command.ExecuteReader();
        var memories = new List<Memory>();
        for (var i = 0; i < resultsReader.FieldCount; i++)
        {
            if (resultsReader.Read())
            {
                memories.Add(new Memory
                {
                    MemoryId = (string)resultsReader[0],
                    
                });
            }
        }

        return Ok();
    }

    [HttpGet("diary/calender")]
    public IActionResult GetCalender()
    {
        return Ok();
    }
}