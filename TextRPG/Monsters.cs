using System.Text;

namespace TextRPG
{
    /// <summary>
    /// Base Class of Monsters
    /// </summary>
    abstract class Monster : IDamagable
    {
        // Field
        private CharacterStat characterStat;
        private bool isAlive;
        private int exp;

        // Property
        public float MaxHealth { get { return characterStat.MaxHealth; } }
        public float Health { get { return characterStat.Health; } set { characterStat.Health = Math.Clamp(value, 0, MaxHealth); } }
        public float MaxMagicPoint { get { return characterStat.MaxMagicPoint; } }
        public float MagicPoint { get { return characterStat.MagicPoint; } set { characterStat.MagicPoint = Math.Clamp(value, 0, MaxMagicPoint); } }
        public string Name { get { return characterStat.Name; } set { characterStat.Name = value; } }
        public int CriticalHitChance { get { return characterStat.CriticalHitChance; } set { characterStat.CriticalHitChance = value; } }
        public float CriticalHitDamagePercentage { get { return characterStat.CriticalHitDamagePercentage; } set { characterStat.CriticalHitDamagePercentage = value; } }
        public int Level { get { return characterStat.Level; } set { characterStat.Level = value; } }
        public AttackStat AttackStat { get { return characterStat.AttackStat; } set { characterStat.AttackStat = value; } }
        public DefendStat DefendStat { get { return characterStat.DefendStat; } set { characterStat.DefendStat = value; } }
        public AttackType AttackType { get; protected set; }

        public int Exp { get { return exp; } set { exp = value; } }
        public bool IsAlive { get { return isAlive; } private set { isAlive = value; } }

        public event Action OnDeath;

        // Constructor
        public Monster(CharacterStat characterStat, int exp)
        {
            this.characterStat = new(characterStat);
            isAlive = true;
            this.exp = exp;
        }

        // Methods
        public CharacterStat GetStat() { return characterStat; }

        /// <summary>
        /// Apply damage to the monster based on the attack type and damage value.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="damage"></param>
        public void OnDamage(AttackType type, float damage, bool isSkill)
        {
            if (IsEvaded() && !isSkill) { Console.WriteLine($"| {Name}이 공격을 회피하였습니다! |"); return; }

            StringBuilder sb = new();
            sb.Append($"| {Name}이 ");
            if (IsCriticalHit())
            {
                damage *= CriticalHitDamagePercentage;
                sb.Append($"치명적인 공격에 맞아");
            }
            float calculatedDamage =
                type == AttackType.Close ? Math.Max(1f, (damage * (1f - DefendStat.Defend / 100f))) :
                (type == AttackType.Long ? Math.Max(1f, damage * (1f - DefendStat.RangeDefend / 100f)) :
                Math.Max(1f, (damage * (1f - DefendStat.MagicDefend / 100f))));

            sb.Append($" {calculatedDamage:F2}의 데미지를 받았습니다! |");
            Console.WriteLine(sb.ToString());
            Health -= calculatedDamage;

            if (Health < 1f && IsAlive) Die();
        }

        /// <summary>
        /// Check if the character evaded the attack.
        /// </summary>
        /// <returns></returns>
        private bool IsEvaded()
        {
            Random rand = new Random();
            float EvasionPercent = rand.Next(0, 100);
            return EvasionPercent < 10;
        }

        /// <summary>
        /// Check if the character's attack is a critical hit.
        /// </summary>
        /// <returns></returns>
        private bool IsCriticalHit()
        {
            Random rand = new Random();
            float CriticalPercent = rand.Next(0, 100);
            return CriticalPercent < CriticalHitChance;
        }

        private void Die()
        {
            IsAlive = false;
            OnDeath?.Invoke();
        }
    }

    /// <summary>
    /// GoblinWarrior Class -> Close Range Attack Monster
    /// </summary>
    class GoblinWarrior : Monster
    {
        public GoblinWarrior(CharacterStat characterStat, int exp) : base(characterStat, exp)
        {
            AttackType = AttackType.Close;
        }
        public GoblinWarrior(GoblinWarrior warrior) : base(warrior.GetStat(), warrior.Exp)
        {
            AttackType = warrior.AttackType;
        }
    }

    /// <summary>
    /// GoblinArcher Class -> Long Range Attack Monster
    /// </summary>
    class GoblinArcher : Monster
    {
        public GoblinArcher(CharacterStat characterStat, int exp) : base(characterStat, exp)
        {
            AttackType = AttackType.Long;
        }
        public GoblinArcher(GoblinArcher archer) : base(archer.GetStat(), archer.Exp)
        {
            AttackType = archer.AttackType;
        }
    }

    /// <summary>
    /// GoblinMage Class -> Magic Attack Monster
    /// </summary>
    class GoblinMage : Monster
    {
        public GoblinMage(CharacterStat characterStat, int exp) : base(characterStat, exp)
        {
            AttackType = AttackType.Magic;
        }
        public GoblinMage(GoblinMage mage) : base(mage.GetStat(), mage.Exp)
        {
            AttackType = mage.AttackType;
        }
    }

    static class MonsterLists
    {
        public static Monster[] monsters = {
            new GoblinWarrior(new CharacterStat("Normal Goblin Warrior", 150, 10, 15, 1.6f, 1, new AttackStat(20f, 1f, 1f), new DefendStat(18, 15, 3)), 20),
            new GoblinArcher(new CharacterStat("Normal Goblin Archer", 120, 30, 15, 1.6f, 1, new AttackStat(1f, 20f, 1f), new DefendStat(15, 18, 3)), 20),
            new GoblinMage(new CharacterStat("Normal Goblin Mage", 100, 50, 15, 1.6f, 1, new AttackStat(1f, 1f, 20f), new DefendStat(3, 15, 18)), 20),
        };
    }
}
