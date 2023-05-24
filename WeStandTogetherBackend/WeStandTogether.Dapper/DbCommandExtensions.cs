using System.Collections.Generic;
using System.Data;

namespace WeStandTogether.Dapper;

public static class DbCommandExtensions
{
    public static IEnumerable<T> GetResults<T>(this IDbCommand dbCommand)
    {
        var resultsReader = dbCommand.ExecuteReader();
        for (var i = 0; i < resultsReader.FieldCount; i++)
        {
            yield return (T)resultsReader[i];
        }
    }
}