namespace DungeonAndDragonsSharp.Models
{
    /// <summary>
    /// Represents a player character or NPC
    /// </summary>
    public class Character
    {
        public string Name { get; set; }
        public Race Race { get; set; }
        public CharacterClass Class { get; set; }
        public int Level { get; set; }
        public AbilityScores Abilities { get; set; }
        public int MaxHitPoints { get; set; }
        public int CurrentHitPoints { get; set; }
        public int ArmorClass { get; set; }
        public int Initiative { get; set; }
        public List<Spell> Spells { get; set; }
        public Position Position { get; set; }
        public bool IsAlive => CurrentHitPoints > 0;

        public Character()
        {
            Name = "Unknown";
            Level = 1;
            Abilities = new AbilityScores();
            Spells = new List<Spell>();
            Position = new Position(0, 0);
        }

        /// <summary>
        /// Roll initiative for combat (d20 + Dexterity modifier)
        /// </summary>
        public int RollInitiative(Random random)
        {
            Initiative = random.Next(1, 21) + Abilities.GetDexterityModifier();
            return Initiative;
        }

        /// <summary>
        /// Calculate hit points based on class and constitution
        /// </summary>
        public void CalculateHitPoints()
        {
            int hitDie = Class switch
            {
                CharacterClass.Fighter => 10,
                CharacterClass.Wizard => 6,
                CharacterClass.Rogue => 8,
                CharacterClass.Cleric => 8,
                _ => 8
            };

            MaxHitPoints = hitDie + Abilities.GetConstitutionModifier();
            if (MaxHitPoints < 1) MaxHitPoints = 1;
            CurrentHitPoints = MaxHitPoints;
        }

        /// <summary>
        /// Calculate armor class based on dexterity
        /// </summary>
        public void CalculateArmorClass()
        {
            ArmorClass = 10 + Abilities.GetDexterityModifier();
            
            // Add class-based armor bonuses
            if (Class == CharacterClass.Fighter)
                ArmorClass += 6; // Chain mail
            else if (Class == CharacterClass.Cleric)
                ArmorClass += 4; // Scale mail
        }

        /// <summary>
        /// Take damage
        /// </summary>
        public void TakeDamage(int damage)
        {
            CurrentHitPoints -= damage;
            if (CurrentHitPoints < 0) CurrentHitPoints = 0;
        }

        /// <summary>
        /// Heal hit points
        /// </summary>
        public void Heal(int amount)
        {
            CurrentHitPoints += amount;
            if (CurrentHitPoints > MaxHitPoints) CurrentHitPoints = MaxHitPoints;
        }
    }
}
