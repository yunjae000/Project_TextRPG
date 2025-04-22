using System.Text.Json.Serialization;

namespace TextRPG
{
    /// <summary>
    /// Base Class of Quests
    /// </summary>
    abstract class Quest : IContractable
    {
        // Field
        private string name;
        private string description;
        private QuestDifficulty difficulty;
        private QuestType questType;
        private int questGoal;
        private int questProgress;
        private int rewardExp;
        private int rewardGold;

        // Property
        public string Name { get { return name; } set { name = value; } }
        public string Description { get { return description; } set { description = value; } }
        public QuestDifficulty Difficulty { get { return difficulty; } set { difficulty = value; } }
        public QuestType QuestType { get { return questType; } set { questType = value; } }
        public int QuestGoal { get { return questGoal; } set { questGoal = value; } }
        public int QuestProgress { get { return questProgress; } set { questProgress = value; } }
        public int RewardExp { get { return rewardExp; } set { rewardExp = value; } }
        public int RewardGold { get { return rewardGold; } set { rewardGold = value; } }
        [JsonInclude] public bool IsContracted { get; protected set; } = false;
        [JsonInclude] public bool IsCompleted { get; protected set; } = false;
        [JsonInclude] public bool IsSpecial { get; protected set; } = false;

        // Constructor
        public Quest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                     int questGoal, int rewardExp, int rewardGold)
        {
            this.name = name;
            this.description = description;
            this.difficulty = difficulty;
            this.questType = questType;
            this.questGoal = questGoal;
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
        }

        public Quest(Quest quest)
        {
            name = quest.name;
            description = quest.description;
            difficulty = quest.difficulty;
            questType = quest.questType;
            questGoal = quest.questGoal;
            rewardExp = quest.rewardExp;
            rewardGold = quest.rewardGold;
            IsCompleted = quest.IsCompleted;
            IsSpecial = quest.IsSpecial;
        }

        [JsonConstructor]
        public Quest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                     int questProgress, int questGoal, int rewardExp, int rewardGold, 
                     bool isCompleted, bool isSpecial)
        {
            this.name = name;
            this.description = description;
            this.difficulty = difficulty;
            this.questType = questType;
            this.questProgress = questProgress;
            this.questGoal = questGoal;
            this.rewardExp = rewardExp;
            this.rewardGold = rewardGold;
            IsCompleted = isCompleted;
            IsSpecial = isSpecial;
        }
        
        /// <summary>
        /// Called when the quest is contracted.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnContracted(Character character)
        {
            IsContracted = true;
            IsCompleted = false;
            QuestProgress = 0;
            Console.WriteLine($"\n| Quest '{Name}' contracted! |");
        }

        public virtual void OnProgress() { } 

        public virtual void OnProgress(Character character) { }
        
        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnCompleted(Character character)
        {
            IsCompleted = true;
            IsContracted = false;
            character.OnEarnExp(RewardExp);
            character.Currency += RewardGold;
            Console.WriteLine($"| Quest '{Name}' completed! |");
        }

        /// <summary>
        /// Show the quest progress.
        /// </summary>
        public void ShowProgress()
        {
            Console.WriteLine($"\n| Quest '{Name}' |");
            Console.WriteLine($"| Progress: {QuestProgress}/{QuestGoal} |");
        }
    }

    /// <summary>
    /// NormalQuest Class -> Available in all days.
    /// </summary>
    class NormalQuest : Quest, ICancelable
    {
        // Constructor
        public NormalQuest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                           int questGoal, int rewardExp, int rewardGold) 
            : base(name, description, difficulty, questType, questGoal, rewardExp, rewardGold) { IsSpecial = false; }
        public NormalQuest(NormalQuest quest) : base(quest) { IsSpecial = false; }
        [JsonConstructor]
        public NormalQuest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                           int questProgress, int questGoal, int rewardExp, int rewardGold, 
                           bool isCompleted, bool isSpecial) 
            : base(name, description, difficulty, questType, questProgress, questGoal, rewardExp, rewardGold, isCompleted, isSpecial) { }

        /// <summary>
        /// Called when the quest is canceled.
        /// </summary>
        /// <param name="character"></param>
        public void OnCanceled(Character character)
        {
            IsContracted = false;
            IsCompleted = false;
            QuestProgress = 0;
        }
    }

    /// <summary>
    /// KillMonsterQuest Class -> Kill a monster.
    /// </summary>
    class KillMonsterQuest : NormalQuest
    {
        // Constructor
        public KillMonsterQuest(string name, string description, QuestDifficulty difficulty,
                                int questGoal, int rewardExp, int rewardGold) 
            : base(name, description, difficulty, QuestType.KillMonster, questGoal, rewardExp, rewardGold) 
        { 
            IsSpecial = false;
        }
        public KillMonsterQuest(KillMonsterQuest quest) : base(quest) 
        { 
            IsSpecial = false;
        }
        [JsonConstructor]
        public KillMonsterQuest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                                int questProgress, int questGoal, int rewardExp, int rewardGold, 
                                bool isCompleted, bool isSpecial) 
            : base(name, description, difficulty, questType, questProgress, questGoal, rewardExp, rewardGold, isCompleted, isSpecial) { }

        /// <summary>
        /// Called when the quest is contracted.
        /// </summary>
        /// <param name="character"></param>
        public override void OnContracted(Character character)
        {
            base.OnContracted(character);
        }

        /// <summary>
        /// Called when the quest is in progress.
        /// </summary>
        /// <param name="character"></param>
        public override void OnProgress()
        {
            if(IsContracted && !IsCompleted) { 
                if (QuestProgress < QuestGoal) QuestProgress++; 
                else { IsCompleted = true; } 
            }
        }

        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        public override void OnCompleted(Character character)
        {
            base.OnCompleted(character);
        }
    }

    /// <summary>
    /// SpecialQuest Class -> Only available in single day.
    /// </summary>
    class SpecialQuest : Quest
    {
        // Constructor
        public SpecialQuest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                            int questGoal, int rewardExp, int rewardGold) 
            : base(name, description, difficulty, questType, questGoal, rewardExp, rewardGold) { IsSpecial = true; }
        public SpecialQuest(SpecialQuest quest) : base(quest) { IsSpecial = true; }
        [JsonConstructor]
        public SpecialQuest(string name, string description, QuestDifficulty difficulty, QuestType questType, 
                            int questProgress, int questGoal, int rewardExp, int rewardGold, 
                            bool isCompleted, bool isSpecial) 
            : base(name, description, difficulty, questType, questProgress, questGoal, rewardExp, rewardGold, isCompleted, isSpecial) { }
        
        // Methods
        public override void OnCompleted(Character character)
        {
            base.OnCompleted(character);
            // TODO: Add special quest completion logic here
        }
        public override void OnContracted(Character character)
        {
            base.OnContracted(character);
            // TODO: Add special quest contracting logic here
        }
    }
}
