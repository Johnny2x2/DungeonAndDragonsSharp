using DungeonAndDragonsSharp.Models;

namespace DungeonAndDragonsSharp.Systems
{
    /// <summary>
    /// Manages combat encounters including turn order, attacks, and spells
    /// </summary>
    public class CombatSystem
    {
        private DiceSystem _dice;
        private List<Character> _combatants;
        private int _currentTurnIndex;

        public CombatSystem(DiceSystem dice)
        {
            _dice = dice;
            _combatants = new List<Character>();
            _currentTurnIndex = 0;
        }

        /// <summary>
        /// Start combat and roll initiative for all combatants
        /// </summary>
        public void StartCombat(List<Character> characters)
        {
            _combatants = new List<Character>(characters);
            
            // Roll initiative for each character
            foreach (var character in _combatants)
            {
                character.RollInitiative(new Random());
            }

            // Sort by initiative (highest first)
            _combatants = _combatants.OrderByDescending(c => c.Initiative).ToList();
            _currentTurnIndex = 0;
        }

        /// <summary>
        /// Get the character whose turn it is
        /// </summary>
        public Character? GetCurrentTurn()
        {
            if (_combatants.Count == 0) return null;
            return _combatants[_currentTurnIndex];
        }

        /// <summary>
        /// Advance to next turn
        /// </summary>
        public void NextTurn()
        {
            do
            {
                _currentTurnIndex = (_currentTurnIndex + 1) % _combatants.Count;
            } while (!_combatants[_currentTurnIndex].IsAlive && _combatants.Any(c => c.IsAlive));
        }

        /// <summary>
        /// Perform a melee attack
        /// </summary>
        public AttackResult Attack(Character attacker, Character target)
        {
            // Roll to hit (d20 + ability modifier)
            int attackRoll = _dice.D20();
            int modifier = attacker.Abilities.GetStrengthModifier();
            int totalAttack = attackRoll + modifier;

            var result = new AttackResult
            {
                Attacker = attacker.Name,
                Target = target.Name,
                AttackRoll = attackRoll,
                TotalAttack = totalAttack,
                TargetAC = target.ArmorClass
            };

            // Check if hit
            if (totalAttack >= target.ArmorClass || attackRoll == 20)
            {
                result.Hit = true;
                
                // Roll damage
                int weaponDamage = attacker.Class switch
                {
                    CharacterClass.Fighter => _dice.D8(), // Longsword
                    CharacterClass.Rogue => _dice.D6(),   // Shortsword
                    CharacterClass.Cleric => _dice.D6(),  // Mace
                    CharacterClass.Wizard => _dice.D4(),  // Dagger
                    _ => _dice.D4()
                };

                int damage = weaponDamage + modifier;
                if (damage < 1) damage = 1;

                // Critical hit
                if (attackRoll == 20)
                {
                    result.CriticalHit = true;
                    damage *= 2;
                }

                result.Damage = damage;
                target.TakeDamage(damage);
            }
            else
            {
                result.Hit = false;
            }

            return result;
        }

        /// <summary>
        /// Cast a spell
        /// </summary>
        public AttackResult CastSpell(Character caster, Spell spell, Character target)
        {
            var result = new AttackResult
            {
                Attacker = caster.Name,
                Target = target.Name,
                SpellName = spell.Name
            };

            // Check range
            if (caster.Position.DistanceTo(target.Position) > spell.Range)
            {
                result.Hit = false;
                result.OutOfRange = true;
                return result;
            }

            // Roll damage
            int damage = _dice.RollMultiple(spell.DamageDice, spell.DamageDiceType);
            result.Damage = damage;
            result.Hit = true;
            
            target.TakeDamage(damage);

            return result;
        }

        /// <summary>
        /// Move a character to a new position
        /// </summary>
        public bool Move(Character character, Position newPosition)
        {
            int distance = character.Position.DistanceTo(newPosition);
            int maxMovement = 6; // Standard movement in D&D is 30 feet = 6 squares

            if (distance <= maxMovement)
            {
                character.Position = newPosition;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Check if combat is over
        /// </summary>
        public bool IsCombatOver()
        {
            return _combatants.Count(c => c.IsAlive) <= 1;
        }

        public List<Character> GetTurnOrder()
        {
            return new List<Character>(_combatants);
        }
    }

    /// <summary>
    /// Result of an attack or spell
    /// </summary>
    public class AttackResult
    {
        public string Attacker { get; set; } = "";
        public string Target { get; set; } = "";
        public int AttackRoll { get; set; }
        public int TotalAttack { get; set; }
        public int TargetAC { get; set; }
        public bool Hit { get; set; }
        public bool CriticalHit { get; set; }
        public int Damage { get; set; }
        public string? SpellName { get; set; }
        public bool OutOfRange { get; set; }
    }
}
