using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ShortenUrl.Tests;

public class DatabaseRepositoryTests
{
    private SqliteConnection _connection;
    private DbTransaction transaction;

    public DatabaseRepositoryTests()
    {
        _connection = new SqliteConnection("Data Source=shortURLTest.sqlite");
        _connection.Open();
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        transaction = _connection.BeginTransactionAsync().GetAwaiter().GetResult();
        _connection.QueryAsync<string>(
            """
            INSERT INTO storedUrl (id, website) VALUES ('1234', 'https://twitch.tv/merry');
    """);
    }

    ~DatabaseRepositoryTests()
    {
        transaction?.Dispose();
    }
    [Fact]
    public async Task QueryAsync_Should_Return_The_Line()
    {
        var expected= new StoredUrl("1234", "https://twitch.tv/merry");
        var dynamicParameters = new DynamicParameters();
        dynamicParameters.Add("@Id","1234");
        var actual = await _connection.QueryFirstOrDefaultAsync<StoredUrl>("select * from storedUrl where id = @Id",dynamicParameters);
        Check.That(actual).IsEqualTo(expected);
    }
}