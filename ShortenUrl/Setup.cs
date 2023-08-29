using System.Data;
using Dapper;

namespace ShortenUrl;

public class Setup
{
    private readonly IDbConnection _dbConnection;

    public Setup(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
        _dbConnection.Open();
    }
    public async Task InitDatabase()
    {
        var initSqlScript = await File.ReadAllTextAsync("init.sql");
        var isExist = await _dbConnection.QueryFirstOrDefaultAsync<bool>("SELECT 1 FROM sqlite_master WHERE type='table' AND name='storedUrl'");
        if (isExist)
        {
            return;   
        }
        await _dbConnection.QueryAsync(initSqlScript);
    }

}