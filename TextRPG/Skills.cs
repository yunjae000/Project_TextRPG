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
        public void OnActive(Character character, Monster target)
        {
            if (character.MagicPoint < ManaCost)
            {
                Console.WriteLine($"| {character.Name} doesn't have enough magic point! |");
                return;
            }
            
            character.MagicPoint -= ManaCost;
            AttackType? type = character.EquippedWeapon?.AttackType;
            if (type == null) { target.OnDamage(AttackType.Close, Coefficient * character.AttackStat.Attack); return; }
            
            switch (type)
            {
                case AttackType.Close: target.OnDamage(AttackType.Close, Coefficient * character.AttackStat.Attack); break;
                case AttackType.Long: target.OnDamage(AttackType.Long, Coefficient * character.AttackStat.RangeAttack); break;
                case AttackType.Magic: target.OnDamage(AttackType.Magic, Coefficient * character.AttackStat.MagicAttack); break;
            }
        }
    }

    /// <summary>
    /// Buff Skills Class
    /// </summary>
    class BuffSkill : Skill, ISkillBuff
    {
        // Field
        private bool isActive;
        private AttackStat originalStat;
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
        public void OnActive(Character character)
        {
            if (character.MagicPoint < ManaCost)
            {
                Console.WriteLine($"| {character.Name} doesn't have enough magic point! |");
                return;
            }

            if (IsActive) { character.MagicPoint -= ManaCost; UsedTurn = GameManager.CurrentTurn; return; }

            character.MagicPoint -= ManaCost;
            originalStat = new(character.AttackStat);
            character.AttackStat *= Coefficient;
            UsedTurn = GameManager.CurrentTurn;
            IsActive = true;
        }

        public void OnBuffExpired(Character character)
        {
            if (!isActive) return;
            character.AttackStat = originalStat;
            IsActive = false;
        }

        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());
            sb.Append($", Interval : {TurnInterval} turn");
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
            new ActiveSkill("Power Strike","Give single target massive damage", 2.0f, 20, true),
            new ActiveSkill("Fire Arrow", "Give all target increased damage", 1.5f, 15, false),
            new ActiveSkill("Thunder Bolt", "Give all target massive damage", 1.8f, 25, false),
        };

        public static Skill[] BuffSkills =
        {
            new BuffSkill("Meditation", "Increase Attack Stat.", 1.8f, 20, 3, false),
        };
    }
}