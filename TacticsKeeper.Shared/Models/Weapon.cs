using System;
using System.Collections.Generic;
using System.Linq;

namespace TacticsKeeper.Shared.Models
{
    public class Weapon
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Range = 0;
        public int MinAttacks = 1;  // Minimum number of attacks
        public int MaxAttacks = 1;  // Maximum number of attacks
        public int BS = 6;
        public int S = 1;
        public int AP = 0;
        public int Damage = 1;
        public int AttackMultiplier = 1; // Multiplier for the number of attacks
        public int AttackModifier = 1; // Modifier for the number of attacks

        // Foreign key
        public int UnitId { get; set; }

        private List<Modifier> Modifiers = new List<Modifier>();


        public Weapon() { }

        public Weapon(string name, int range, int minAttacks, int maxAttacks, int ballisticSkill, int strength, int armorPenetration, int damage, int attackMultiplier = 1, int attackModifier = 0)
        {
            Name = name;
            Range = range;
            MinAttacks = minAttacks;
            MaxAttacks = maxAttacks;
            BS = ballisticSkill;
            S = strength;
            AP = armorPenetration;
            Damage = damage;
            AttackMultiplier = attackMultiplier;
            AttackModifier = attackModifier;
            Modifiers = new List<Modifier>();
        }

        public void AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
        }

        public void RemoveModifier(string keyword)
        {
            Modifiers.RemoveAll(m => m.Keyword == keyword);
        }

        public double CalculateHitProbability()
        {
            int hitModifierTotal = Modifiers.Where(m => m.Type == Modifier.ModifierType.Hit).Sum(m => m.Value);
            int adjustedBS = BS + hitModifierTotal;
            adjustedBS = Math.Clamp(adjustedBS, 2, 6);
            return (7 - adjustedBS) / 6.0;
        }

        // New method to calculate the range of hit probabilities based on the minimum and maximum number of attacks
        public (double Min, double Max) CalculateHitProbabilityRange()
        {
            double hitProbabilityPerAttack = CalculateHitProbability();

            // Calculate the effective number of attacks considering multipliers and modifiers
            int effectiveMinAttacks = MinAttacks * AttackMultiplier + AttackModifier;
            int effectiveMaxAttacks = MaxAttacks * AttackMultiplier + AttackModifier;

            // Calculate hit probabilities for minimum and maximum possible attacks
            double minProbability = hitProbabilityPerAttack * effectiveMinAttacks;
            double maxProbability = hitProbabilityPerAttack * effectiveMaxAttacks;

            return (minProbability, maxProbability);
        }

        public List<(double Probability, string Condition)> CalculateWoundProbabilities(int targetToughness, int targetSave, int targetInvulSave = 0)
        {
            List<(double Probability, string Condition)> woundProbabilities = new List<(double Probability, string Condition)>();
            double probability;

            // Determine the required roll to wound based on strength vs toughness
            int requiredRoll;
            if (S >= 2 * targetToughness)
            {
                requiredRoll = 2;
            }
            else if (S > targetToughness)
            {
                requiredRoll = 3;
            }
            else if (S == targetToughness)
            {
                requiredRoll = 4;
            }
            else if (S < targetToughness && S > targetToughness / 2)
            {
                requiredRoll = 5;
            }
            else
            {
                requiredRoll = 6;
            }

            probability = (7 - requiredRoll) / 6.0;
            woundProbabilities.Add((probability, $"Strength vs Toughness roll required: {requiredRoll}+"));

            // Adjust for save and invulnerable save
            int effectiveSave = targetSave - AP;
            if (targetInvulSave > 0 && targetInvulSave < effectiveSave)
            {
                effectiveSave = targetInvulSave;
            }

            // Calculate the probability of failing the save
            double saveFailProbability = (7 - effectiveSave) / 6.0;
            if (effectiveSave > 6)
            {
                saveFailProbability = 1.0; // Impossible to save if effective save is greater than 6
            }
            else if (effectiveSave < 2)
            {
                saveFailProbability = 0.0; // Impossible to fail if effective save is less than 2
            }

            // Final probability to wound after saves
            double finalWoundProbability = probability * saveFailProbability;
            woundProbabilities.Add((finalWoundProbability, $"Final wound probability after saves with effective save {effectiveSave}"));

            return woundProbabilities;
        }

        public List<(double Probability, string Condition)> CalculateWoundProbabilities(Unit unit)
        {
            return CalculateWoundProbabilities(unit.Toughness, unit.Save, unit.InvulnerableSave);
        }
    }
}
