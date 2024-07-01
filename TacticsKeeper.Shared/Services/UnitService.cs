using Microsoft.Data.Sqlite;
using TacticsKeeper.Shared.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TacticsKeeper.Shared.Services;

public class UnitService : DatabaseService<Unit>
{
    public UnitService() : base("tacticskeeper.db", "Units")
    {
    }

    protected override string GetCreateTableSql()
    {
        return @"
            CREATE TABLE IF NOT EXISTS Units (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Movement INTEGER,
                Strength INTEGER,
                Toughness INTEGER,
                Wounds INTEGER,
                Leadership INTEGER,
                Save INTEGER,
                InvulnerableSave INTEGER
            )";
    }

    protected override void MapReaderToItem(SqliteDataReader reader, Unit item)
    {
        item.Id = reader.GetInt32(0);
        item.Name = reader.GetString(1);
        item.Movement = reader.GetInt32(2);
        item.Strength = reader.GetInt32(3);
        item.Toughness = reader.GetInt32(4);
        item.Wounds = reader.GetInt32(5);
        item.Leadership = reader.GetInt32(6);
        item.Save = reader.GetInt32(7);
        item.InvulnerableSave = reader.GetInt32(8);
    }

    protected override int GetItemId(Unit item)
    {
        return item.Id;
    }

    protected override string GetInsertSql()
    {
        return @"
            INSERT INTO Units (Name, Movement, Strength, Toughness, Wounds, Leadership, Save, InvulnerableSave)
            VALUES ($name, $movement, $strength, $toughness, $wounds, $leadership, $save, $invulnerableSave)";
    }

    protected override string GetUpdateSql()
    {
        return @"
            UPDATE Units
            SET Name = $name, Movement = $movement, Strength = $strength, Toughness = $toughness, Wounds = $wounds, Leadership = $leadership, Save = $save, InvulnerableSave = $invulnerableSave
            WHERE Id = $id";
    }

    protected override void AddCommandParameters(SqliteCommand command, Unit item)
    {
        command.Parameters.AddWithValue("$id", item.Id);
        command.Parameters.AddWithValue("$name", item.Name);
        command.Parameters.AddWithValue("$movement", item.Movement);
        command.Parameters.AddWithValue("$strength", item.Strength);
        command.Parameters.AddWithValue("$toughness", item.Toughness);
        command.Parameters.AddWithValue("$wounds", item.Wounds);
        command.Parameters.AddWithValue("$leadership", item.Leadership);
        command.Parameters.AddWithValue("$save", item.Save);
        command.Parameters.AddWithValue("$invulnerableSave", item.InvulnerableSave);
    }

    public async Task<List<Weapon>> GetWeaponsForUnitAsync(int unitId)
    {
        var weapons = new List<Weapon>();

        using var connection = new SqliteConnection($"Data Source={_dbPath}");
        await connection.OpenAsync();

        using var command = connection.CreateCommand();
        command.CommandText = $"SELECT * FROM Weapons WHERE UnitId = $unitId";
        command.Parameters.AddWithValue("$unitId", unitId);

        using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            var weapon = new Weapon
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Range = reader.GetInt32(2),
                MinAttacks = reader.GetInt32(3),
                MaxAttacks = reader.GetInt32(4),
                BS = reader.GetInt32(5),
                S = reader.GetInt32(6),
                AP = reader.GetInt32(7),
                Damage = reader.GetInt32(8),
                AttackMultiplier = reader.GetInt32(9),
                AttackModifier = reader.GetInt32(10),
                UnitId = reader.GetInt32(11)
            };
            weapons.Add(weapon);
        }

        return weapons;
    }
}
