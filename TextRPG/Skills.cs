using System.Text;
using System.Text.Json.Serialization;

namespace TextRPG
{
    /// <summary>
    /// Base Class of Skills
    /// </summary>
    abstract class Skill
    {
        // Field
        private string name;
        private string description;
        private float coefficient;
        private int manaCost;
        private int usedTurn;
        private bool isTargetable;

        // Property
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public float Coefficient { get { return coefficient; } set { coefficient = value; } }
        public int ManaCost { get { return manaCost; } set { manaCost = value; } }
        public int UsedTurn { get { return usedTurn; } set { usedTurn = value; } }
        public bool IsTargetable { get { return isTargetable; } set { isTargetable = value; } }

        // Constructor
        public Skill(string name, string description, float coefficient, int manaCost, bool isTargetable)
        {
            this.name = name;
            this.description = description;
            this.coefficient = coefficient;
            this.manaCost = manaCost;
            this.isTargetable = isTargetable;
        }

        public virtual new string ToString()
        {
            return $"Name : {name}, Desc. : {description}, Coef. : {coefficient:F2}, Mana Cost: {manaCost}";
        }
    }

    /// <summary>
    /// Active Skills Class
    /// </summary>
    class ActiveSkill : Skill, ISkillActive
    {
        // Constructor
        [JsonConstructor]
        public ActiveSkill(string name, string description, float coefficient, 
                           int manaCost, bool isTargetable) 
            : base(name, description, coefficient, manaCost, isTargetable) { }
        public ActiveSkill(ActiveSkill skill) 
            : base(skill.Name, skill.Description, skill.Coefficient, skill.ManaCost, skill.IsTargetable) { }

        // Methods
        /// <summary>
        /// Active Skill Method
        /// </summary>
        /// <param name="character"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool OnActive(Character character, Monster target)
        {
            if (character.MagicPoint < ManaCost)
            {
                Console.WriteLine($"| {character.Name}가 시전하기엔 MP가 부족합니다! |");
                return false;
            }
            
            character.OnMagicPointConsume(ManaCost);
            Console.WriteLine($"| 스킬_{Name}을 {target.Name}에 시전하였습니다! |");
            
            AttackType? type = character.EquippedWeapon?.AttackType;
            switch (type)
            {
                case AttackType.Close: target.OnDamage(AttackType.Close, Coefficient * character.AttackStat.Attack); break;
                case AttackType.Long: target.OnDamage(AttackType.Long, Coefficient * character.AttackStat.RangeAttack); break;
                case AttackType.Magic: target.OnDamage(AttackType.Magic, Coefficient * character.AttackStat.MagicAttack); break;
            }
            return true;
        }

        /// <summary>
        /// Active Skill Method -> All Targets
        /// </summary>
        /// <param name="character"></param>
        /// <param name="targets"></param>
        /// <returns></returns>
        public bool OnActive(Character character, LinkedList<Monster> targets)
        {
            if (character.MagicPoint < ManaCost)
            {
                Console.WriteLine($"| {character.Name}가 시전하기엔 MP가 부족합니다! |");
                return false;
            }
            
            character.OnMagicPointConsume(ManaCost);
            Console.WriteLine($"| 스킬_{Name}을 모든 몬스터에 시전하였습니다! |");

            AttackType? type = character.EquippedWeapon?.AttackType;
            switch (type)
            {
                case AttackType.Close:
                    var current = targets.First;
                    while(current != null)
                    {
                        var next = current.Next;
                        Monster target = current.Value;
                        target.OnDamage(AttackType.Close, Coefficient * character.AttackStat.Attack);
                        current = next;
                    }
                    break;
                case AttackType.Long:
                    current = targets.First;
                    while (current != null)
                    {
                        var next = current.Next;
                        Monster target = current.Value;
                        target.OnDamage(AttackType.Long, Coefficient * character.AttackStat.RangeAttack);
                        current = next;
                    }
                    break;
                case AttackType.Magic: 
                    current = targets.First;
                    while (current != null)
                    {
                        var next = current.Next;
                        Monster target = current.Value;
                        target.OnDamage(AttackType.Magic, Coefficient * character.AttackStat.MagicAttack);
                        current = next;
                    }
                    break;
            }
            return true;
        }
    }

    /// <summary>
    /// Buff Skills Class
    /// </summary>
    class BuffSkill : Skill, ISkillBuff
    {
        // Field
        private bool isActive;
        private AttackStat originalAttackStat;
        private DefendStat originalDefendStat;
        private int turnInterval;

        // Property
        public bool IsActive { get { return isActive; } set { isActive = value; } }
        public int TurnInterval { get { return turnInterval; } set { turnInterval = value; } }

        // Constructor
        [JsonConstructor]
        public BuffSkill(string name, string description, float coefficient,
                         int manaCost, int turnInterval, bool isTargetable)
            : base(name, description, coefficient, manaCost, isTargetable) { this.turnInterval = turnInterval; }
        public BuffSkill(BuffSkill skill) : base(skill.Name, skill.Description, skill.Coefficient, skill.ManaCost, skill.IsTargetable) { turnInterval = skill.turnInterval; }

        // Methods
        /// <summary>
        /// Buff Skill Method
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        public bool OnActive(Character character)
        {
            if (character.MagicPoint < ManaCost)
            {
                Console.WriteLine($"| {character.Name}가 시전하기엔 MP가 부족합니다! |");
                return false;
            }

            if (IsActive) { character.OnMagicPointConsume(ManaCost); UsedTurn = GameManager.CurrentTurn; return true; }

            character.OnMagicPointConsume(ManaCost);
            Console.WriteLine($"| 스킬_{Name}을 시전하였습니다! |");
            
            originalAttackStat = new(character.AttackStat);
            originalDefendStat = new(character.DefendStat);
            character.AttackStat *= Coefficient;
            character.DefendStat *= Coefficient;
            UsedTurn = GameManager.CurrentTurn;
            IsActive = true;

            return true;
        }

        /// <summary>
        /// Buff Skill Method -> Buff Expired
        /// </summary>
        /// <param name="character"></param>
        public void OnBuffExpired(Character character)
        {
            if (!isActive) return;
            character.AttackStat = originalAttackStat;
            character.DefendStat = originalDefendStat;
            IsActive = false;
            Console.WriteLine($"| 스킬_{Name} 의 효과가 사라졌습니다! |");
        }

        /// <summary>
        /// Description of Buff Skill
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());
            sb.Append($", 지속 턴 : {TurnInterval}턴");
            return sb.ToString();
        }
    }

    /// <summary>
    /// Skill Lists
    /// </summary>
    static class SkillLists
    {
        public static Skill[] ActiveSkills =
        {
            new ActiveSkill("파워 스트라이크","단일 타겟에 매우 큰 데미지를 입힌다.", 2.0f, 20, true),
            new ActiveSkill("파이어 에로우", "모든 타켓에 데미지를 입힌다.", 1.5f, 15, false),
            new ActiveSkill("썬더 볼트", "모든 타겟에 매우 큰 데미지를 입힌다.", 1.8f, 25, false),
        };

        public static Skill[] BuffSkills =
        {
            new BuffSkill("명상", "모든 스텟을 증가시킨다.", 1.8f, 20, 3, false),
        };
    }
}