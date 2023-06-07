using Dapper;

using Microsoft.Extensions.Configuration;

using System.Data;
using System.Data.SQLite;

namespace SPMLibrary.DataAccess;

public class SqLiteDataAccess : ISqLiteDataAccess
{
    private readonly IConfiguration _config;

    public SqLiteDataAccess(IConfiguration config)
    {
        _config = config;
    }

    public List<T> SqlQuery<T, U>(string sqlStatement,
                                  U parameters,
                                  string connectionStringName = "Default")
    {
        string? connectionString = _config.GetConnectionString(connectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Could not find the connection string {connectionStringName}.");
        }

        using IDbConnection connection = new SQLiteConnection(connectionString);

        List<T> rows = connection.Query<T>(sqlStatement, parameters).ToList();

        return rows;
    }

    public void SqlExecute<T>(string sqlStatement,
                              T parameters,
                              string connectionStringName = "Default")
    {
        string? connectionString = _config.GetConnectionString(connectionStringName);

        if (string.IsNullOrEmpty(connectionString))
        {
            throw new InvalidOperationException($"Could not find the connection string {connectionStringName}.");
        }

        using IDbConnection connection = new SQLiteConnection(connectionString);

        connection.Execute(sqlStatement, parameters);
    }
}