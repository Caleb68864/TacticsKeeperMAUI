using Microsoft.Data.Sqlite;
using TacticsKeeper.Shared.Models;

namespace TacticsKeeper.Shared.Services;


public class WeaponService : DatabaseService<Weapon>
{
    public WeaponService() : base("tacticskeeper.db", "Weapons")
    {
    }

    protected override string GetCreateTableSql()
    {
        return @"
            CREATE TABLE IF NOT EXISTS Weapons (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Name TEXT NOT NULL,
                Range INTEGER,
                MinAttacks INTEGER,
                MaxAttacks INTEGER,
                BS INTEGER,
                S INTEGER,
                AP INTEGER,
                Damage INTEGER,
                AttackMultiplier INTEGER,
                AttackModifier INTEGER,
                UnitId INTEGER,
                FOREIGN KEY (UnitId) REFERENCES Units(Id)
            )";
    }

    protected override void MapReaderToItem(SqliteDataReader reader, Weapon item)
    {
        item.Id = reader.GetInt32(0);
        item.Name = reader.GetString(1);
        item.Range = reader.GetInt32(2);
        item.MinAttacks = reader.GetInt32(3);
        item.MaxAttacks = reader.GetInt32(4);
        item.BS = reader.GetInt32(5);
        item.S = reader.GetInt32(6);
        item.AP = reader.GetInt32(7);
        item.Damage = reader.GetInt32(8);
        item.AttackMultiplier = reader.GetInt32(9);
        item.AttackModifier = reader.GetInt32(10);
        item.UnitId = reader.GetInt32(11);
    }

    protected override int GetItemId(Weapon item)
    {
        return item.Id;
    }

    protected override string GetInsertSql()
    {
        return @"
            INSERT INTO Weapons (Name, Range, MinAttacks, MaxAttacks, BS, S, AP, Damage, AttackMultiplier, AttackModifier, UnitId)
            VALUES ($name, $range, $minAttacks, $maxAttacks, $bs, $s, $ap, $damage, $attackMultiplier, $attackModifier, $unitId)";
    }

    protected override string GetUpdateSql()
    {
        return @"
            UPDATE Weapons
            SET Name = $name, Range = $range, MinAttacks = $minAttacks, MaxAttacks = $maxAttacks, BS = $bs, S = $s, AP = $ap, Damage = $damage, AttackMultiplier = $attackMultiplier, AttackModifier = $attackModifier, UnitId = $unitId
            WHERE Id = $id";
    }

    protected override void AddCommandParameters(SqliteCommand command, Weapon item)
    {
        command.Parameters.AddWithValue("$id", item.Id);
        command.Parameters.AddWithValue("$name", item.Name);
        command.Parameters.AddWithValue("$range", item.Range);
        command.Parameters.AddWithValue("$minAttacks", item.MinAttacks);
        command.Parameters.AddWithValue("$maxAttacks", item.MaxAttacks);
        command.Parameters.AddWithValue("$bs", item.BS);
        command.Parameters.AddWithValue("$s", item.S);
        command.Parameters.AddWithValue("$ap", item.AP);
        command.Parameters.AddWithValue("$damage", item.Damage);
        command.Parameters.AddWithValue("$attackMultiplier", item.AttackMultiplier);
        command.Parameters.AddWithValue("$attackModifier", item.AttackModifier);
        command.Parameters.AddWithValue("$unitId", item.UnitId);
    }
}
