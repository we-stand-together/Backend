using System.Data;
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
    public IActionResult GetMemory([FromBody] GetMemoryRequest request)
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
        IDataReader resultsReader = command.ExecuteReader();

        var memories = ExtractMemoryDisplayDataFromResultsReader(resultsReader);

        return Ok(memories);
    }

    [HttpPost("diary/memories")]
    public IActionResult SaveMemory([FromBody] SaveMemoryRequest request)
    {
        Console.WriteLine("Post Diary/Memories");

        using var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();
        var command = dbConnection.CreateCommand();

        var memoryDate = DateOnly.Parse(request.Date);
        var date = memoryDate.ToDateTime(TimeOnly.MinValue).ToString("yyyy-MM-dd HH:mm:ss");

        command.CommandText =
            $"insert into Memories (memory_id, owner_phone_number, message, date)" +
            $"values ('{Guid.NewGuid().ToString()}', '{User.Identity.Name}'," +
            $" '{request.Message}', '{date}');";

        Console.WriteLine(command.CommandText);
        command.ExecuteNonQuery();

        return Ok();
    }

    [HttpGet("diary/calender")]
    public IActionResult GetDatesWithMemories()
    {
        Console.WriteLine("Get All Memories");
        
        using var dbConnection = _dapperContext.CreateConnection();
        dbConnection.Open();
        var command = dbConnection.CreateCommand();

        command.CommandText = $"select DATE(date) from Memories group by DATE(date);";
        Console.WriteLine(command.CommandText);

        var reader = command.ExecuteReader();
        var datesWithMemories = ExtractDatesFromResultsReader(reader).ToList();
        
        return Ok(datesWithMemories);
    }

    private IEnumerable<MemoryDisplayData> ExtractMemoryDisplayDataFromResultsReader(IDataReader resultsReader)
    {
        for (var i = 0; i < resultsReader.FieldCount; i++)
        {
            if (resultsReader.Read())
            {
                yield return new MemoryDisplayData
                (
                    (string)resultsReader[2],
                    (DateTime)resultsReader[3]
                ); 
            }
        }
    }


    private IEnumerable<DateOnly> ExtractDatesFromResultsReader(IDataReader resultsReader)
    {
        // TODO: This endpoint is buggy - only return first row instead of all of the dates
        for (var i = 0; i < resultsReader.FieldCount; i++)
        {
            if (resultsReader.Read())
            {
                yield return DateOnly.FromDateTime((DateTime)(resultsReader[0]));
            }
        }
    }
}