namespace DungeonAndDragonsSharp.Systems
{
    /// <summary>
    /// Handles all dice rolling in the game
    /// </summary>
    public class DiceSystem
    {
        private Random _random;

        public DiceSystem()
        {
            _random = new Random();
        }

        public DiceSystem(int seed)
        {
            _random = new Random(seed);
        }

        public int Roll(int sides)
        {
            return _random.Next(1, sides + 1);
        }

        public int RollMultiple(int count, int sides)
        {
            int total = 0;
            for (int i = 0; i < count; i++)
            {
                total += Roll(sides);
            }
            return total;
        }

        public int D4() => Roll(4);
        public int D6() => Roll(6);
        public int D8() => Roll(8);
        public int D10() => Roll(10);
        public int D12() => Roll(12);
        public int D20() => Roll(20);
        public int D100() => Roll(100);

        /// <summary>
        /// Roll with advantage (roll twice, take higher)
        /// </summary>
        public int RollWithAdvantage(int sides)
        {
            return Math.Max(Roll(sides), Roll(sides));
        }

        /// <summary>
        /// Roll with disadvantage (roll twice, take lower)
        /// </summary>
        public int RollWithDisadvantage(int sides)
        {
            return Math.Min(Roll(sides), Roll(sides));
        }

        /// <summary>
        /// Standard ability score generation (4d6, drop lowest)
        /// </summary>
        public int RollAbilityScore()
        {
            int[] rolls = new int[4];
            for (int i = 0; i < 4; i++)
            {
                rolls[i] = D6();
            }
            Array.Sort(rolls);
            // Sum the three highest
            return rolls[1] + rolls[2] + rolls[3];
        }
    }
}
