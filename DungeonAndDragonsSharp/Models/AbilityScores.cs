namespace DungeonAndDragonsSharp.Models
{
    /// <summary>
    /// Represents the six core ability scores in D&D
    /// </summary>
    public class AbilityScores
    {
        public int Strength { get; set; }
        public int Dexterity { get; set; }
        public int Constitution { get; set; }
        public int Intelligence { get; set; }
        public int Wisdom { get; set; }
        public int Charisma { get; set; }

        public AbilityScores()
        {
            Strength = 10;
            Dexterity = 10;
            Constitution = 10;
            Intelligence = 10;
            Wisdom = 10;
            Charisma = 10;
        }

        /// <summary>
        /// Calculate ability modifier based on D&D 5e rules
        /// </summary>
        public static int GetModifier(int score)
        {
            return (score - 10) / 2;
        }

        public int GetStrengthModifier() => GetModifier(Strength);
        public int GetDexterityModifier() => GetModifier(Dexterity);
        public int GetConstitutionModifier() => GetModifier(Constitution);
        public int GetIntelligenceModifier() => GetModifier(Intelligence);
        public int GetWisdomModifier() => GetModifier(Wisdom);
        public int GetCharismaModifier() => GetModifier(Charisma);
    }
}
