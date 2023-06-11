using System.Data;
using Dapper;

namespace ShortenUrl;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly IDbConnection _dbConnection;

    public DatabaseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        
    }

    public async Task<T> QueryAsync<T>(string id)
    {
        _dbConnection.Open();
        var parameters = new DynamicParameters();
        parameters.Add("@Id",id);
        var result = await _dbConnection.QueryFirstOrDefaultAsync<T>("select * from storedUrl where id=@Id",parameters);
        return result;
    }
}


// ReSharper disable once ClassNeverInstantiated.Global
public record StoredUrl(string Id, string Website);

public interface IDatabaseRepository
{
    Task<T> QueryAsync<T>(string id);
}