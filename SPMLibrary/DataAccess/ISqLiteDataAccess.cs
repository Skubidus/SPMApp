namespace SPMLibrary.DataAccess;

public interface ISqLiteDataAccess
{
    void SqlExecute<T>(string sqlStatement, T parameters, string connectionStringName = "Default");
    List<T> SqlQuery<T, U>(string sqlStatement, U parameters, string connectionStringName = "Default");
}