using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TacticsKeeper.Shared.Models;

public class Modifier
{
    public string Keyword { get; set; }
    public int Value { get; set; }
    public ModifierType Type { get; set; }

    public Modifier(string keyword, int value, ModifierType type)
    {
        Keyword = keyword;
        Value = value;
        Type = type;
    }

    public enum ModifierType
    {
        Hit,
        Wound,
        Attack  // New type for attack modifiers
    }
}
