# Dungeons & Dragons - Console Edition

A C# console application implementing a Dungeons & Dragons 5th Edition inspired game engine for 4 players and a Dungeon Master.

## Features

### Character Creation
- **Races**: Human, Elf, Dwarf, Halfling
- **Classes**: Fighter, Wizard, Rogue, Cleric
- **Ability Scores**: Strength, Dexterity, Constitution, Intelligence, Wisdom, Charisma
- Automatic ability score generation using standard D&D 4d6 drop lowest method
- Racial bonuses applied automatically

### Combat System
- **Initiative System**: Turn-based combat with d20 + Dexterity modifier
- **Attack Rolls**: d20 + ability modifier vs Armor Class
- **Movement System**: Grid-based positioning (6 squares per turn)
- **Spell Casting**: Class-specific spells with range and damage
- **Critical Hits**: Natural 20s deal double damage
- **Hit Points**: Class-based hit dice with Constitution modifier

### Game Systems
- **Save/Load**: JSON-based save game system
- **Adventure Mode**: Random encounters with configurable difficulty
- **Rest System**: Long rest to restore hit points
- **Party Management**: View and manage 4 player characters

### Supported Spells
#### Wizard
- **Fire Bolt** (Cantrip): 1d10 fire damage, 24 range
- **Magic Missile** (Level 1): 3d4 force damage, 24 range

#### Cleric
- **Sacred Flame** (Cantrip): 1d8 radiant damage, 12 range
- **Cure Wounds** (Level 1): 1d8 + Wisdom modifier healing, 1 range

## D&D 5th Edition Rules Reference

### Core Mechanics
- **Ability Scores**: Range from 3-20, with 10-11 being average
- **Ability Modifiers**: (Score - 10) / 2, rounded down
- **d20 System**: Most actions involve rolling a d20 and adding modifiers

### Combat
1. **Initiative**: Each combatant rolls d20 + Dexterity modifier
2. **Turn Order**: Combatants act in initiative order (highest to lowest)
3. **Actions Per Turn**: 
   - Movement (up to 30 feet = 6 squares)
   - One action (Attack, Cast Spell, etc.)
4. **Attack Resolution**: 
   - Roll d20 + ability modifier
   - Compare to target's Armor Class (AC)
   - If equal or higher: Hit!
   - Roll damage dice + ability modifier

### Character Classes
- **Fighter**: High HP (d10), good armor, melee specialists
- **Wizard**: Low HP (d6), powerful spells, Intelligence-based
- **Rogue**: Medium HP (d8), Dexterity-based, sneaky
- **Cleric**: Medium HP (d8), healing spells, Wisdom-based

### Races
- **Human**: +1 to all ability scores
- **Elf**: +2 Dexterity
- **Dwarf**: +2 Constitution
- **Halfling**: +2 Dexterity

## How to Build and Run

### Prerequisites
- .NET 8.0 SDK or later

### Build
```bash
cd DungeonAndDragonsSharp
dotnet build
```

### Run
```bash
cd DungeonAndDragonsSharp
dotnet run
```

## Gameplay Guide

### Starting a New Game
1. Choose "New Game" from the main menu
2. Create 4 characters by:
   - Entering a name
   - Selecting a race
   - Selecting a class
3. View each character's generated stats

### Main Game Loop
- **View Party**: See all characters and their stats
- **Adventure**: Enter a random encounter (60% chance of combat)
- **Rest**: Restore all characters to full HP
- **Save Game**: Save your current progress

### Combat
During combat, each character can:
- **Attack**: Make a melee attack against an enemy
- **Cast Spell**: Use class-specific spells
- **Move**: Reposition on the battlefield
- **Pass**: Skip the turn

### Saving and Loading
- Save games are stored in the `SaveGames/` directory
- Multiple save files are supported
- Save files use the `.sav` extension

## Project Structure
```
DungeonAndDragonsSharp/
├── Models/
│   ├── AbilityScores.cs    # Ability score management
│   ├── Character.cs         # Player/NPC character
│   ├── Enums.cs            # Race and Class enumerations
│   ├── Position.cs         # Battlefield positioning
│   └── Spell.cs            # Spell definitions
├── Systems/
│   ├── CharacterCreationSystem.cs  # Character generation
│   ├── CombatSystem.cs             # Combat mechanics
│   ├── DiceSystem.cs               # Dice rolling
│   ├── GameState.cs                # Game state management
│   └── SaveSystem.cs               # Save/Load functionality
├── UI/
│   └── ConsoleUI.cs        # Console interface
├── GameEngine.cs           # Main game coordinator
└── Program.cs              # Entry point
```

## Future Enhancements
- More character classes and races
- Equipment system
- Inventory management
- More complex encounters and dungeons
- Character leveling and experience
- More spells and abilities
- Multiplayer support

## License
Educational project demonstrating D&D mechanics in C#.

## References
- [D&D 5th Edition Basic Rules](https://www.dndbeyond.com/sources/basic-rules)
- [System Reference Document (SRD)](https://dnd.wizards.com/resources/systems-reference-document)

