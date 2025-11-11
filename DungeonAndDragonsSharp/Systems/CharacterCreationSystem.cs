using DungeonAndDragonsSharp.Models;

namespace DungeonAndDragonsSharp.Systems
{
    /// <summary>
    /// Handles character creation
    /// </summary>
    public class CharacterCreationSystem
    {
        private DiceSystem _dice;

        public CharacterCreationSystem(DiceSystem dice)
        {
            _dice = dice;
        }

        /// <summary>
        /// Create a new character with rolled ability scores
        /// </summary>
        public Character CreateCharacter(string name, Race race, CharacterClass characterClass)
        {
            var character = new Character
            {
                Name = name,
                Race = race,
                Class = characterClass,
                Level = 1,
                Abilities = RollAbilityScores()
            };

            // Apply racial bonuses
            ApplyRacialBonuses(character);

            // Calculate derived stats
            character.CalculateHitPoints();
            character.CalculateArmorClass();

            // Add class-specific spells
            AddClassSpells(character);

            return character;
        }

        /// <summary>
        /// Roll ability scores using standard method (4d6 drop lowest)
        /// </summary>
        private AbilityScores RollAbilityScores()
        {
            return new AbilityScores
            {
                Strength = _dice.RollAbilityScore(),
                Dexterity = _dice.RollAbilityScore(),
                Constitution = _dice.RollAbilityScore(),
                Intelligence = _dice.RollAbilityScore(),
                Wisdom = _dice.RollAbilityScore(),
                Charisma = _dice.RollAbilityScore()
            };
        }

        /// <summary>
        /// Apply racial bonuses to ability scores
        /// </summary>
        private void ApplyRacialBonuses(Character character)
        {
            switch (character.Race)
            {
                case Race.Human:
                    // +1 to all abilities
                    character.Abilities.Strength++;
                    character.Abilities.Dexterity++;
                    character.Abilities.Constitution++;
                    character.Abilities.Intelligence++;
                    character.Abilities.Wisdom++;
                    character.Abilities.Charisma++;
                    break;
                case Race.Elf:
                    character.Abilities.Dexterity += 2;
                    break;
                case Race.Dwarf:
                    character.Abilities.Constitution += 2;
                    break;
                case Race.Halfling:
                    character.Abilities.Dexterity += 2;
                    break;
            }
        }

        /// <summary>
        /// Add starting spells based on class
        /// </summary>
        private void AddClassSpells(Character character)
        {
            switch (character.Class)
            {
                case CharacterClass.Wizard:
                    character.Spells.Add(new Spell("Magic Missile", 1, "Creates darts of magical force", 3, 4, 24));
                    character.Spells.Add(new Spell("Fire Bolt", 0, "Hurls a mote of fire", 1, 10, 24));
                    break;
                case CharacterClass.Cleric:
                    character.Spells.Add(new Spell("Sacred Flame", 0, "Radiant flame-like energy descends", 1, 8, 12));
                    character.Spells.Add(new Spell("Cure Wounds", 1, "Heals a creature you touch", 1, 8, 1));
                    break;
            }
        }
    }
}
