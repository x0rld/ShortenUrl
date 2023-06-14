using System.Data;
using Dapper;

namespace ShortenUrl;

public class DatabaseRepository : IDatabaseRepository
{
    private readonly IDbConnection _dbConnection;

    public DatabaseRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _dbConnection.Open();
    }

    public async Task<T> QueryAsync<T>(string id)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", id);
        var result =
            await _dbConnection.QueryFirstOrDefaultAsync<T>("select * from storedUrl where id=@Id", parameters);
        return result;
    }

    public async Task InsertAsync(StoredUrl info)
    {
        var parameters = new DynamicParameters();
        parameters.Add("@Id", info.Id);
        parameters.Add("@Url", info.Website);
            await _dbConnection.QueryAsync(
            "INSERT INTO storedUrl (id, website) VALUES (@Id, @Url);", parameters);
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
public record StoredUrl(string Id, string Website)
{
    // ReSharper disable once UnusedMember.Local
    private StoredUrl()  : this(default!, default!) { }
};

public interface IDatabaseRepository
{
    Task<T> QueryAsync<T>(string id);

    Task InsertAsync(StoredUrl info);
}