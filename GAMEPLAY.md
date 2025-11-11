# D&D Game Engine Demo Script

This document demonstrates the features of the D&D game engine.

## Running the Game

```bash
cd DungeonAndDragonsSharp
dotnet run
```

## Demo Walkthrough

### 1. Main Menu
When you start, you'll see:
```
╔════════════════════════════════════════════════════════════╗
║                                                            ║
║         DUNGEONS & DRAGONS - Console Edition               ║
║                                                            ║
╚════════════════════════════════════════════════════════════╝

=== Main Menu ===
1. New Game
2. Load Game
3. Exit
```

### 2. Creating Characters
Select "1" to create a new game. You'll create 4 characters:

For each character:
- Enter a name (e.g., "Aragorn", "Gandalf", "Legolas", "Gimli")
- Choose a race:
  - 1. Human (+1 to all abilities)
  - 2. Elf (+2 Dexterity)
  - 3. Dwarf (+2 Constitution)
  - 4. Halfling (+2 Dexterity)
- Choose a class:
  - 1. Fighter (high HP, heavy armor)
  - 2. Wizard (magic user, low HP)
  - 3. Rogue (sneaky, medium HP)
  - 4. Cleric (healer, medium HP)

After creation, you'll see a character sheet like:
```
╔════════════════════════════════════════╗
║ Aragorn                                ║
╠════════════════════════════════════════╣
║ Race: Human                           ║
║ Class: Fighter                        ║
║ Level: 1                              ║
╠════════════════════════════════════════╣
║ HP: 12/12                             ║
║ AC: 17                                ║
╠════════════════════════════════════════╣
║ STR: 16 (+3)                          ║
║ DEX: 14 (+2)                          ║
║ CON: 15 (+2)                          ║
║ INT: 12 (+1)                          ║
║ WIS: 13 (+1)                          ║
║ CHA: 10 (+0)                          ║
╚════════════════════════════════════════╝
```

### 3. Game Menu
After character creation, you'll see:
```
=== Game Menu ===
1. View Party
2. Adventure
3. Rest
4. Save Game
5. Return to Main Menu
```

### 4. Adventuring
Select "2" to go on an adventure. There's a 60% chance of encountering enemies!

### 5. Combat
If you encounter enemies, combat begins:
1. Initiative is rolled for all combatants
2. Combat order is displayed
3. On each character's turn, you can:
   - **Attack**: Roll d20 + modifier vs enemy AC
   - **Cast Spell**: Use class-specific spells
   - **Move**: Change position on the battlefield
   - **Pass**: Skip your turn

Example combat output:
```
=== COMBAT BEGINS ===

Initiative rolled!
=== Combat Status ===
Initiative Order:
  1. Gandalf (Init: 18) - HP: 8/8 - Pos: (1, 0)
  2. Goblin 1 (Init: 15) - HP: 6/6 - Pos: (8, 2)
  3. Aragorn (Init: 14) - HP: 12/12 - Pos: (1, 2)
  4. Goblin 2 (Init: 12) - HP: 5/5 - Pos: (8, 4)

=== Gandalf's Turn ===
1. Attack
2. Cast Spell
3. Move
4. Pass

Select target:
1. Goblin 1 (HP: 6/6)
2. Goblin 2 (HP: 5/5)

Gandalf casts Magic Missile at Goblin 1!
  HIT! 8 damage dealt!
```

### 6. Saving Your Game
Select "4" from the Game Menu to save:
```
=== Save Game ===
Enter save name: MyAdventure
Game saved successfully!
```

### 7. Loading a Game
From the main menu, select "2":
```
=== Load Game ===
1. MyAdventure
Choose save (or 0 to cancel): 1
Game loaded successfully!
```

## Example Character Builds

### Tank Fighter (Dwarf)
- Race: Dwarf (+2 CON)
- Class: Fighter
- Focus: High HP and AC for absorbing damage

### Damage Wizard (Elf)
- Race: Elf (+2 DEX)
- Class: Wizard
- Focus: Powerful offensive spells

### Healer Cleric (Human)
- Race: Human (+1 to all)
- Class: Cleric
- Focus: Cure Wounds spell for healing party

### Sneaky Rogue (Halfling)
- Race: Halfling (+2 DEX)
- Class: Rogue
- Focus: High initiative and damage

## Combat Tips

1. **Use Initiative Wisely**: High DEX characters go first
2. **Position Matters**: Stay within spell range
3. **Heal Strategically**: Use Cure Wounds before characters fall
4. **Focus Fire**: Eliminate one enemy at a time
5. **Rest When Safe**: Restore full HP between encounters

## D&D Mechanics Implemented

- **Ability Score Modifiers**: (Score - 10) / 2
- **Attack Rolls**: d20 + modifier vs AC
- **Damage Rolls**: Weapon/spell dice + modifier
- **Critical Hits**: Natural 20 = 2x damage
- **Initiative**: d20 + DEX modifier
- **Movement**: 30 feet (6 squares) per turn
- **Hit Points**: Class hit die + CON modifier

Enjoy your adventure!
