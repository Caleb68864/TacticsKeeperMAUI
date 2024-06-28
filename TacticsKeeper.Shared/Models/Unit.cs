using System;
using System.Collections.Generic;

namespace TacticsKeeper.Shared.Models
{
    public class Unit
    {
        public string Name { get; set; } = string.Empty;
        public int Movement { get; set; } = 0;
        public int Strength { get; set; } = 1;
        public int Toughness { get; set; } = 1;
        public int Wounds { get; set; } = 1;
        public int Leadership { get; set; } = 6;
        public int Save { get; set; } = 6;
        public int InvulnerableSave { get; set; } = 0;

        public List<Weapon> Weapons { get; set; } = new List<Weapon>();
        public List<Ability> Abilities { get; set; } = new List<Ability>();

        public Unit() { }

        public Unit(string name, int movement, int strength, int toughness, int wounds, int leadership, int save)
        {
            Name = name;
            Movement = movement;
            Strength = strength;
            Toughness = toughness;
            Wounds = wounds;
            Leadership = leadership;
            Save = save;
        }

        public void AddWeapon(Weapon weapon)
        {
            Weapons.Add(weapon);
        }

        public void AddAbility(Ability ability)
        {
            Abilities.Add(ability);
        }

        public void RemoveWeapon(string weaponName)
        {
            Weapons.RemoveAll(w => w.Name == weaponName);
        }

        public void RemoveAbility(string abilityName)
        {
            Abilities.RemoveAll(a => a.Name == abilityName);
        }

        // Method to calculate total hit probability for all weapons
        public List<(string WeaponName, double MinHitProbability, double MaxHitProbability)> CalculateWeaponHitProbabilities()
        {
            List<(string WeaponName, double MinHitProbability, double MaxHitProbability)> hitProbabilities = new List<(string WeaponName, double MinHitProbability, double MaxHitProbability)>();

            foreach (var weapon in Weapons)
            {
                var (min, max) = weapon.CalculateHitProbabilityRange();
                hitProbabilities.Add((weapon.Name, min, max));
            }

            return hitProbabilities;
        }
    }
}
