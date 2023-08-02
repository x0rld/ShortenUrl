using System.Data.Common;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ShortenUrl.Tests;

public class DatabaseRepositoryTests : IDisposable
{
    private readonly DbTransaction _transaction;
    private readonly IDatabaseRepository _sut;

    public DatabaseRepositoryTests()
    {
        var connection = new SqliteConnection("Data Source=shortURLTest.sqlite");
        connection.Open();
        SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());
        _transaction = connection.BeginTransaction();
        _sut = new DatabaseRepository(connection);
        connection.QueryAsync<string>(
            """
            INSERT INTO storedUrl (id, website) VALUES ('1234', 'https://twitch.tv/merry');
    """);
    }
    
    [Fact]
    public async Task QueryAsync_Should_Return_The_Line()
    {
        var expected= new StoredUrl("1234", "https://twitch.tv/merry");
        var actual = await _sut.QueryAsync<StoredUrl>("1234");
        Check.That(actual).IsEqualTo(expected);
    }
      
    [Fact]
    public void InsertAsync_Should_Not_Throw_An_Exception_When_Id_Doesnt_Exist()
    {
        Check.ThatCode(async () => await _sut.InsertAsync(new StoredUrl("4563", "https://twitch.tv/merry")))
            .DoesNotThrow();
    }
    
    private void Dispose(bool disposing)
    {
        _transaction.Rollback();
        if (disposing)
        {
            _transaction.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}