namespace DungeonAndDragonsSharp.Models
{
    /// <summary>
    /// Represents a spell that can be cast
    /// </summary>
    public class Spell
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Description { get; set; }
        public int DamageDice { get; set; }
        public int DamageDiceType { get; set; }
        public int Range { get; set; }

        public Spell(string name, int level, string description, int damageDice, int damageDiceType, int range)
        {
            Name = name;
            Level = level;
            Description = description;
            DamageDice = damageDice;
            DamageDiceType = damageDiceType;
            Range = range;
        }
    }
}
