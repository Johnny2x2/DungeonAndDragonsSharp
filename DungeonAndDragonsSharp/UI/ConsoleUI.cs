using DungeonAndDragonsSharp.Models;
using DungeonAndDragonsSharp.Systems;

namespace DungeonAndDragonsSharp.UI
{
    /// <summary>
    /// Handles all console UI rendering
    /// </summary>
    public class ConsoleUI
    {
        public void ShowWelcome()
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("║         DUNGEONS & DRAGONS - Console Edition               ║");
            Console.WriteLine("║                                                            ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();
        }

        public void ShowMainMenu()
        {
            Console.WriteLine("\n=== Main Menu ===");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Exit");
            Console.Write("\nChoose an option: ");
        }

        public void ShowGameMenu()
        {
            Console.WriteLine("\n=== Game Menu ===");
            Console.WriteLine("1. View Party");
            Console.WriteLine("2. Adventure");
            Console.WriteLine("3. Rest");
            Console.WriteLine("4. Save Game");
            Console.WriteLine("5. Return to Main Menu");
            Console.Write("\nChoose an option: ");
        }

        public void ShowCombatMenu(Character currentCharacter)
        {
            Console.WriteLine($"\n=== {currentCharacter.Name}'s Turn ===");
            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Cast Spell");
            Console.WriteLine("3. Move");
            Console.WriteLine("4. Pass");
            Console.Write("\nChoose an action: ");
        }

        public void ShowCharacterSheet(Character character)
        {
            Console.WriteLine("\n╔════════════════════════════════════════╗");
            Console.WriteLine($"║ {character.Name,-38} ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine($"║ Race: {character.Race,-31} ║");
            Console.WriteLine($"║ Class: {character.Class,-30} ║");
            Console.WriteLine($"║ Level: {character.Level,-30} ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine($"║ HP: {character.CurrentHitPoints}/{character.MaxHitPoints,-30} ║");
            Console.WriteLine($"║ AC: {character.ArmorClass,-33} ║");
            Console.WriteLine("╠════════════════════════════════════════╣");
            Console.WriteLine($"║ STR: {character.Abilities.Strength,2} ({GetModifierString(character.Abilities.GetStrengthModifier())})                        ║");
            Console.WriteLine($"║ DEX: {character.Abilities.Dexterity,2} ({GetModifierString(character.Abilities.GetDexterityModifier())})                        ║");
            Console.WriteLine($"║ CON: {character.Abilities.Constitution,2} ({GetModifierString(character.Abilities.GetConstitutionModifier())})                        ║");
            Console.WriteLine($"║ INT: {character.Abilities.Intelligence,2} ({GetModifierString(character.Abilities.GetIntelligenceModifier())})                        ║");
            Console.WriteLine($"║ WIS: {character.Abilities.Wisdom,2} ({GetModifierString(character.Abilities.GetWisdomModifier())})                        ║");
            Console.WriteLine($"║ CHA: {character.Abilities.Charisma,2} ({GetModifierString(character.Abilities.GetCharismaModifier())})                        ║");
            Console.WriteLine("╚════════════════════════════════════════╝");

            if (character.Spells.Count > 0)
            {
                Console.WriteLine("\nSpells:");
                foreach (var spell in character.Spells)
                {
                    Console.WriteLine($"  - {spell.Name} (Level {spell.Level})");
                }
            }
        }

        private string GetModifierString(int modifier)
        {
            return modifier >= 0 ? $"+{modifier}" : modifier.ToString();
        }

        public void ShowParty(List<Character> party)
        {
            Console.WriteLine("\n=== Your Party ===");
            for (int i = 0; i < party.Count; i++)
            {
                var character = party[i];
                string status = character.IsAlive ? "Alive" : "Dead";
                Console.WriteLine($"{i + 1}. {character.Name} ({character.Class}) - HP: {character.CurrentHitPoints}/{character.MaxHitPoints} [{status}]");
            }
        }

        public void ShowCombatStatus(List<Character> combatants)
        {
            Console.WriteLine("\n=== Combat Status ===");
            Console.WriteLine("Initiative Order:");
            for (int i = 0; i < combatants.Count; i++)
            {
                var c = combatants[i];
                string status = c.IsAlive ? $"HP: {c.CurrentHitPoints}/{c.MaxHitPoints}" : "DEFEATED";
                Console.WriteLine($"  {i + 1}. {c.Name} (Init: {c.Initiative}) - {status} - Pos: {c.Position}");
            }
        }

        public void ShowAttackResult(AttackResult result)
        {
            Console.WriteLine();
            if (result.SpellName != null)
            {
                Console.WriteLine($"{result.Attacker} casts {result.SpellName} at {result.Target}!");
                if (result.OutOfRange)
                {
                    Console.WriteLine("  MISS - Out of range!");
                }
                else
                {
                    Console.WriteLine($"  HIT! {result.Damage} damage dealt!");
                }
            }
            else
            {
                Console.WriteLine($"{result.Attacker} attacks {result.Target}!");
                Console.WriteLine($"  Roll: {result.AttackRoll} + modifier = {result.TotalAttack} vs AC {result.TargetAC}");
                
                if (result.Hit)
                {
                    if (result.CriticalHit)
                        Console.WriteLine($"  CRITICAL HIT! {result.Damage} damage dealt!");
                    else
                        Console.WriteLine($"  HIT! {result.Damage} damage dealt!");
                }
                else
                {
                    Console.WriteLine("  MISS!");
                }
            }
        }

        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadLine()
        {
            return Console.ReadLine() ?? "";
        }

        public void Pause()
        {
            Console.WriteLine("\nPress any key to continue...");
            try
            {
                Console.ReadKey();
            }
            catch (InvalidOperationException)
            {
                // Handle redirected input (for automated testing)
                Console.ReadLine();
            }
        }
    }
}
