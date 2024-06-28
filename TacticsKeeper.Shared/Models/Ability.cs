using System;
using System.Collections.Generic;

namespace TacticsKeeper.Shared.Models
{
    public class Ability
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public Ability() { }

        public Ability(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
