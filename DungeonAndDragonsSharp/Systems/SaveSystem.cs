using System.Text.Json;
using DungeonAndDragonsSharp.Models;

namespace DungeonAndDragonsSharp.Systems
{
    /// <summary>
    /// Handles saving and loading game state
    /// </summary>
    public class SaveSystem
    {
        private const string SaveDirectory = "SaveGames";
        private const string SaveExtension = ".sav";

        public SaveSystem()
        {
            // Create save directory if it doesn't exist
            if (!Directory.Exists(SaveDirectory))
            {
                Directory.CreateDirectory(SaveDirectory);
            }
        }

        /// <summary>
        /// Save the game state to a file
        /// </summary>
        public bool SaveGame(GameState gameState, string saveName)
        {
            try
            {
                string filePath = Path.Combine(SaveDirectory, saveName + SaveExtension);
                string json = JsonSerializer.Serialize(gameState, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving game: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Load a saved game
        /// </summary>
        public GameState? LoadGame(string saveName)
        {
            try
            {
                string filePath = Path.Combine(SaveDirectory, saveName + SaveExtension);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Save file not found.");
                    return null;
                }

                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<GameState>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading game: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// List all available saves
        /// </summary>
        public List<string> ListSaves()
        {
            var saves = new List<string>();
            if (Directory.Exists(SaveDirectory))
            {
                var files = Directory.GetFiles(SaveDirectory, "*" + SaveExtension);
                foreach (var file in files)
                {
                    saves.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            return saves;
        }

        /// <summary>
        /// Delete a save file
        /// </summary>
        public bool DeleteSave(string saveName)
        {
            try
            {
                string filePath = Path.Combine(SaveDirectory, saveName + SaveExtension);
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting save: {ex.Message}");
                return false;
            }
        }
    }
}
