using DungeonAndDragonsSharp.Models;
using DungeonAndDragonsSharp.Systems;
using DungeonAndDragonsSharp.UI;

namespace DungeonAndDragonsSharp
{
    /// <summary>
    /// Main game engine that coordinates all systems
    /// </summary>
    public class GameEngine
    {
        private ConsoleUI _ui;
        private DiceSystem _dice;
        private CharacterCreationSystem _characterCreation;
        private CombatSystem _combat;
        private SaveSystem _saveSystem;
        private GameState _gameState;
        private bool _running;

        public GameEngine()
        {
            _ui = new ConsoleUI();
            _dice = new DiceSystem();
            _characterCreation = new CharacterCreationSystem(_dice);
            _combat = new CombatSystem(_dice);
            _saveSystem = new SaveSystem();
            _gameState = new GameState();
            _running = true;
        }

        public void Run()
        {
            _ui.ShowWelcome();
            
            while (_running)
            {
                _ui.ShowMainMenu();
                string choice = _ui.ReadLine();

                switch (choice)
                {
                    case "1":
                        StartNewGame();
                        break;
                    case "2":
                        LoadGame();
                        break;
                    case "3":
                        _running = false;
                        _ui.ShowMessage("Thanks for playing!");
                        break;
                    default:
                        _ui.ShowMessage("Invalid option.");
                        break;
                }
            }
        }

        private void StartNewGame()
        {
            _gameState = new GameState();
            _ui.ShowMessage("\n=== Character Creation ===");
            _ui.ShowMessage("Creating 4 player characters...\n");

            // Create 4 player characters
            for (int i = 1; i <= 4; i++)
            {
                _ui.ShowMessage($"\n--- Character {i} ---");
                Console.Write("Enter character name: ");
                string name = _ui.ReadLine();

                // Choose race
                _ui.ShowMessage("\nChoose race:");
                _ui.ShowMessage("1. Human");
                _ui.ShowMessage("2. Elf");
                _ui.ShowMessage("3. Dwarf");
                _ui.ShowMessage("4. Halfling");
                Console.Write("Choice: ");
                Race race = int.TryParse(_ui.ReadLine(), out int raceChoice) && raceChoice >= 1 && raceChoice <= 4
                    ? (Race)(raceChoice - 1)
                    : Race.Human;

                // Choose class
                _ui.ShowMessage("\nChoose class:");
                _ui.ShowMessage("1. Fighter");
                _ui.ShowMessage("2. Wizard");
                _ui.ShowMessage("3. Rogue");
                _ui.ShowMessage("4. Cleric");
                Console.Write("Choice: ");
                CharacterClass charClass = int.TryParse(_ui.ReadLine(), out int classChoice) && classChoice >= 1 && classChoice <= 4
                    ? (CharacterClass)(classChoice - 1)
                    : CharacterClass.Fighter;

                var character = _characterCreation.CreateCharacter(name, race, charClass);
                _gameState.PlayerCharacters.Add(character);

                _ui.ShowCharacterSheet(character);
                _ui.Pause();
            }

            _ui.ShowMessage("\n=== Party Created! ===");
            GameLoop();
        }

        private void LoadGame()
        {
            var saves = _saveSystem.ListSaves();
            if (saves.Count == 0)
            {
                _ui.ShowMessage("No saved games found.");
                _ui.Pause();
                return;
            }

            _ui.ShowMessage("\n=== Load Game ===");
            for (int i = 0; i < saves.Count; i++)
            {
                _ui.ShowMessage($"{i + 1}. {saves[i]}");
            }
            Console.Write("\nChoose save (or 0 to cancel): ");
            
            if (int.TryParse(_ui.ReadLine(), out int choice) && choice > 0 && choice <= saves.Count)
            {
                var loadedState = _saveSystem.LoadGame(saves[choice - 1]);
                if (loadedState != null)
                {
                    _gameState = loadedState;
                    _ui.ShowMessage("Game loaded successfully!");
                    _ui.Pause();
                    GameLoop();
                }
            }
        }

        private void GameLoop()
        {
            bool inGame = true;
            
            while (inGame)
            {
                _ui.ShowGameMenu();
                string choice = _ui.ReadLine();

                switch (choice)
                {
                    case "1":
                        ViewParty();
                        break;
                    case "2":
                        Adventure();
                        break;
                    case "3":
                        Rest();
                        break;
                    case "4":
                        SaveGame();
                        break;
                    case "5":
                        inGame = false;
                        break;
                    default:
                        _ui.ShowMessage("Invalid option.");
                        break;
                }
            }
        }

        private void ViewParty()
        {
            _ui.ShowParty(_gameState.PlayerCharacters);
            _ui.ShowMessage("\nEnter character number to view details (or 0 to go back): ");
            
            if (int.TryParse(_ui.ReadLine(), out int choice) && choice > 0 && choice <= _gameState.PlayerCharacters.Count)
            {
                _ui.ShowCharacterSheet(_gameState.PlayerCharacters[choice - 1]);
            }
            _ui.Pause();
        }

        private void Adventure()
        {
            _ui.ShowMessage($"\n=== Adventure in {_gameState.CurrentLocation} ===");
            _ui.ShowMessage("You venture forth into the unknown...\n");

            // Random encounter
            if (_dice.D100() <= 60) // 60% chance of encounter
            {
                _ui.ShowMessage("You encounter hostile creatures!");
                _ui.Pause();
                StartCombat();
            }
            else
            {
                _ui.ShowMessage("You explore the area but find nothing of interest.");
                _ui.Pause();
            }
        }

        private void StartCombat()
        {
            _ui.ShowMessage("\n=== COMBAT BEGINS ===");
            
            // Create enemies
            var enemies = new List<Character>();
            int enemyCount = _dice.Roll(3) + 1; // 2-4 enemies

            for (int i = 1; i <= enemyCount; i++)
            {
                var enemy = _characterCreation.CreateCharacter(
                    $"Goblin {i}",
                    Race.Human,
                    CharacterClass.Rogue
                );
                enemy.MaxHitPoints = _dice.D8();
                enemy.CurrentHitPoints = enemy.MaxHitPoints;
                enemy.Position = new Position(8, i * 2);
                enemies.Add(enemy);
            }

            // Combine all combatants
            var allCombatants = new List<Character>(_gameState.PlayerCharacters);
            allCombatants.AddRange(enemies);

            // Set player positions
            for (int i = 0; i < _gameState.PlayerCharacters.Count; i++)
            {
                _gameState.PlayerCharacters[i].Position = new Position(1, i * 2);
            }

            _combat.StartCombat(allCombatants);
            _ui.ShowMessage("\nInitiative rolled!");
            _ui.ShowCombatStatus(_combat.GetTurnOrder());
            _ui.Pause();

            // Combat loop
            while (!_combat.IsCombatOver())
            {
                var currentCharacter = _combat.GetCurrentTurn();
                if (currentCharacter == null || !currentCharacter.IsAlive)
                {
                    _combat.NextTurn();
                    continue;
                }

                _ui.ShowCombatStatus(_combat.GetTurnOrder());

                // Check if current character is a player or enemy
                if (_gameState.PlayerCharacters.Contains(currentCharacter))
                {
                    // Player turn
                    PlayerCombatTurn(currentCharacter, enemies);
                }
                else
                {
                    // Enemy turn (AI)
                    EnemyCombatTurn(currentCharacter, _gameState.PlayerCharacters);
                }

                _combat.NextTurn();
            }

            // Combat over
            _ui.ShowMessage("\n=== COMBAT ENDED ===");
            
            bool playerVictory = _gameState.PlayerCharacters.Any(c => c.IsAlive);
            if (playerVictory)
            {
                _ui.ShowMessage("Victory! The enemies have been defeated!");
            }
            else
            {
                _ui.ShowMessage("Defeat! Your party has fallen...");
                _ui.ShowMessage("Game Over.");
                _running = false;
            }
            
            _ui.Pause();
        }

        private void PlayerCombatTurn(Character character, List<Character> enemies)
        {
            bool turnOver = false;
            
            while (!turnOver)
            {
                _ui.ShowCombatMenu(character);
                string choice = _ui.ReadLine();

                switch (choice)
                {
                    case "1": // Attack
                        var target = SelectTarget(enemies);
                        if (target != null)
                        {
                            var result = _combat.Attack(character, target);
                            _ui.ShowAttackResult(result);
                            _ui.Pause();
                            turnOver = true;
                        }
                        break;
                    case "2": // Cast Spell
                        if (character.Spells.Count > 0)
                        {
                            CastSpell(character, enemies);
                            turnOver = true;
                        }
                        else
                        {
                            _ui.ShowMessage("You have no spells!");
                        }
                        break;
                    case "3": // Move
                        Move(character);
                        turnOver = true;
                        break;
                    case "4": // Pass
                        _ui.ShowMessage($"{character.Name} passes their turn.");
                        turnOver = true;
                        break;
                    default:
                        _ui.ShowMessage("Invalid option.");
                        break;
                }
            }
        }

        private void EnemyCombatTurn(Character enemy, List<Character> players)
        {
            _ui.ShowMessage($"\n{enemy.Name}'s turn...");
            
            var aliveTargets = players.Where(p => p.IsAlive).ToList();
            if (aliveTargets.Count > 0)
            {
                // Simple AI: attack random player
                var target = aliveTargets[_dice.Roll(aliveTargets.Count) - 1];
                var result = _combat.Attack(enemy, target);
                _ui.ShowAttackResult(result);
            }
            
            _ui.Pause();
        }

        private Character? SelectTarget(List<Character> enemies)
        {
            var aliveEnemies = enemies.Where(e => e.IsAlive).ToList();
            if (aliveEnemies.Count == 0) return null;

            _ui.ShowMessage("\nSelect target:");
            for (int i = 0; i < aliveEnemies.Count; i++)
            {
                _ui.ShowMessage($"{i + 1}. {aliveEnemies[i].Name} (HP: {aliveEnemies[i].CurrentHitPoints}/{aliveEnemies[i].MaxHitPoints})");
            }
            Console.Write("Choice: ");

            if (int.TryParse(_ui.ReadLine(), out int choice) && choice > 0 && choice <= aliveEnemies.Count)
            {
                return aliveEnemies[choice - 1];
            }

            return null;
        }

        private void CastSpell(Character caster, List<Character> enemies)
        {
            _ui.ShowMessage("\nSelect spell:");
            for (int i = 0; i < caster.Spells.Count; i++)
            {
                var spell = caster.Spells[i];
                _ui.ShowMessage($"{i + 1}. {spell.Name} - {spell.Description}");
            }
            Console.Write("Choice: ");

            if (int.TryParse(_ui.ReadLine(), out int spellChoice) && spellChoice > 0 && spellChoice <= caster.Spells.Count)
            {
                var spell = caster.Spells[spellChoice - 1];
                
                // For healing spells
                if (spell.Name == "Cure Wounds")
                {
                    _ui.ShowParty(_gameState.PlayerCharacters);
                    Console.Write("Select target to heal: ");
                    if (int.TryParse(_ui.ReadLine(), out int targetChoice) && targetChoice > 0 && targetChoice <= _gameState.PlayerCharacters.Count)
                    {
                        var target = _gameState.PlayerCharacters[targetChoice - 1];
                        int healing = _dice.D8() + caster.Abilities.GetWisdomModifier();
                        target.Heal(healing);
                        _ui.ShowMessage($"{caster.Name} heals {target.Name} for {healing} HP!");
                        _ui.Pause();
                    }
                }
                else
                {
                    var target = SelectTarget(enemies);
                    if (target != null)
                    {
                        var result = _combat.CastSpell(caster, spell, target);
                        _ui.ShowAttackResult(result);
                        _ui.Pause();
                    }
                }
            }
        }

        private void Move(Character character)
        {
            _ui.ShowMessage($"\nCurrent position: {character.Position}");
            Console.Write("Enter new X position: ");
            if (int.TryParse(_ui.ReadLine(), out int x))
            {
                Console.Write("Enter new Y position: ");
                if (int.TryParse(_ui.ReadLine(), out int y))
                {
                    var newPos = new Position(x, y);
                    if (_combat.Move(character, newPos))
                    {
                        _ui.ShowMessage($"{character.Name} moves to {newPos}");
                    }
                    else
                    {
                        _ui.ShowMessage("Too far to move!");
                    }
                }
            }
            _ui.Pause();
        }

        private void Rest()
        {
            _ui.ShowMessage("\n=== Long Rest ===");
            _ui.ShowMessage("Your party takes a long rest...");
            
            foreach (var character in _gameState.PlayerCharacters)
            {
                if (character.IsAlive)
                {
                    character.CurrentHitPoints = character.MaxHitPoints;
                }
            }
            
            _ui.ShowMessage("All characters restored to full health!");
            _ui.Pause();
        }

        private void SaveGame()
        {
            _ui.ShowMessage("\n=== Save Game ===");
            Console.Write("Enter save name: ");
            string saveName = _ui.ReadLine();
            
            if (!string.IsNullOrWhiteSpace(saveName))
            {
                if (_saveSystem.SaveGame(_gameState, saveName))
                {
                    _ui.ShowMessage("Game saved successfully!");
                }
                else
                {
                    _ui.ShowMessage("Failed to save game.");
                }
            }
            else
            {
                _ui.ShowMessage("Invalid save name.");
            }
            
            _ui.Pause();
        }
    }
}
