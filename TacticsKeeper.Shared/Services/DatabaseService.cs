using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TacticsKeeper.Shared.Services;


public abstract class DatabaseService<T> where T : new()
{
    protected readonly string _dbPath;
    protected readonly string _tableName;

    public DatabaseService(string dbName, string tableName)
    {
        _dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), dbName);
        _tableName = tableName;
        InitializeDatabase();
    }

    private void InitializeDatabase()
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        connection.Open();

        string createTableSql = GetCreateTableSql();
        using var command = connection.CreateCommand();
        command.CommandText = createTableSql;
        command.ExecuteNonQuery();
    }

    protected abstract string GetCreateTableSql();

    protected abstract void MapReaderToItem(SqliteDataReader reader, T item);

    protected abstract int GetItemId(T item);

    protected abstract string GetInsertSql();

    protected abstract string GetUpdateSql();

    protected abstract void AddCommandParameters(SqliteCommand command, T item);

    public async Task<List<T>> GetItemsAsync()
    {
        var items = new List<T>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM {_tableName}";

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var item = new T();
            MapReaderToItem(reader, item);
            items.Add(item);
        }

        return items;
    }

    public async Task<int> SaveItemAsync(T item)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        if (GetItemId(item) != 0)
        {
            command.CommandText = GetUpdateSql();
            AddCommandParameters(command, item);
        }
        else
        {
            command.CommandText = GetInsertSql();
            AddCommandParameters(command, item);
        }

        return await command.ExecuteNonQueryAsync();
    }

    public async Task<int> DeleteItemAsync(T item)
    {
        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = $"DELETE FROM {_tableName} WHERE Id = $id";
        command.Parameters.AddWithValue("$id", GetItemId(item));

        return await command.ExecuteNonQueryAsync();
    }
}
