using DungeonAndDragonsSharp.Models;

namespace DungeonAndDragonsSharp.Systems
{
    /// <summary>
    /// Represents the current state of the game
    /// </summary>
    public class GameState
    {
        public List<Character> PlayerCharacters { get; set; }
        public bool InCombat { get; set; }
        public int CurrentRound { get; set; }
        public string CurrentLocation { get; set; }

        public GameState()
        {
            PlayerCharacters = new List<Character>();
            InCombat = false;
            CurrentRound = 0;
            CurrentLocation = "The Tavern";
        }
    }
}
