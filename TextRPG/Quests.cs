using System.Text;
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
            IsContracted = quest.IsContracted;
            IsCompleted = quest.IsCompleted;
            IsSpecial = quest.IsSpecial;
        }

        [JsonConstructor]
        public Quest(string name, string description, QuestDifficulty difficulty, QuestType questType,
                     int questProgress, int questGoal, int rewardExp, int rewardGold, bool isContracted,
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
            IsContracted = isContracted;
            IsCompleted = isCompleted;
            IsSpecial = isSpecial;
        }

        /// <summary>
        /// Called when the quest is contracted. -> For monster quests.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnContracted()
        {
            IsContracted = true;
            QuestProgress = 0;
            Console.WriteLine($"\n| Quest '{Name}' 을 수주하였습니다! |");
        }

        /// <summary>
        /// Called when the quest is contracted. -> For item quests.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnContracted(Character character)
        {
            IsContracted = true;
            QuestProgress = 0;
            Console.WriteLine($"\n| Quest '{Name}' 을 수주하였습니다! |");
        }

        /// <summary>
        /// Called when the quest is in progress. -> For monster quests.
        /// </summary>
        public virtual void OnProgress() { }

        /// <summary>
        /// Called when the quest is in progress. -> For item quests.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnProgress(Character character) { }

        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        public virtual void OnCompleted(Character character)
        {
            IsContracted = false;
            character.OnEarnExp(RewardExp);
            character.Currency += RewardGold;
            Console.WriteLine($"| Quest '{Name}' 을 완료하였습니다! |");
        }

        /// <summary>
        /// Show the quest progress.
        /// </summary>
        public void ShowProgress()
        {
            Console.WriteLine($"\n| Quest '{Name}' |");
            Console.WriteLine($"| 진행도: {QuestProgress}/{QuestGoal} |");
        }

        public abstract new string ToString();
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
                           int questProgress, int questGoal, int rewardExp, int rewardGold, bool isContracted,
                           bool isCompleted, bool isSpecial)
            : base(name, description, difficulty, questType, questProgress, questGoal, rewardExp, rewardGold, isContracted, isCompleted, isSpecial) { }

        /// <summary>
        /// Called when the quest is canceled.
        /// </summary>
        public void OnCanceled()
        {
            IsContracted = false;
            IsCompleted = false;
            QuestProgress = 0;
        }

        /// <summary>
        /// Describe the quest.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new();
            _ = IsSpecial == true ? sb.Append("| [★] ") : sb.Append("| [] ");
            sb.AppendLine($"Quest : '{Name}' |")
              .AppendLine($"| 상세설명 |")
              .AppendLine($"| {Description}")
              .AppendLine($"| 난이도 : '{Difficulty}', 타입 : '{QuestType}' |")
              .AppendLine($"| Exp : '{RewardExp}', Gold : '{RewardGold} |");
            return sb.ToString();
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
                                int questProgress, int questGoal, int rewardExp, int rewardGold, bool isContracted,
                                bool isCompleted, bool isSpecial)
            : base(name, description, difficulty, questType, questProgress, questGoal, rewardExp, rewardGold, isContracted, isCompleted, isSpecial) { }

        /// <summary>
        /// Called when the quest is in progress.
        /// </summary>
        /// <param name="character"></param>
        public override void OnProgress()
        {
            if (!IsContracted || IsCompleted) return;

            QuestProgress++;
            if (QuestProgress >= QuestGoal) { IsCompleted = true; }
        }

        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        public override void OnCompleted(Character character)
        {
            base.OnCompleted(character);
        }

        /// <summary>
        /// Describe the quest.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());
            if (IsContracted) sb.AppendLine($"| 진행도 : '{QuestProgress}/{QuestGoal}' |");
            return sb.ToString();
        }
    }

    /// <summary>
    /// CollectItemQuest Class -> Collect an item.
    /// </summary>
    class CollectItemQuest : NormalQuest
    {
        // Field
        private string itemName;

        // Property
        public string ItemName { get { return itemName; } set { itemName = value; } }

        // Constructor
        public CollectItemQuest(CollectItemQuest quest) : base(quest)
        {
            ItemName = quest.ItemName;
            IsSpecial = false;
        }

        public CollectItemQuest(string name, string itemName, string description, QuestDifficulty difficulty,
                                int questGoal, int rewardExp, int rewardGold)
            : base(name, description, difficulty, QuestType.CollectItem, questGoal, rewardExp, rewardGold)
        {
            IsSpecial = false;
            ItemName = itemName;
        }

        [JsonConstructor]
        public CollectItemQuest(string name, string itemName, string description, QuestDifficulty difficulty, QuestType questType,
                                int questProgress, int questGoal, int rewardExp, int rewardGold, bool isContracted,
                                bool isCompleted, bool isSpecial)
            : base(name, description, difficulty, questType, questProgress, questGoal, rewardExp, rewardGold, isContracted, isCompleted, isSpecial)
        {
            IsSpecial = isSpecial;
            ItemName = itemName;
        }

        // Methods

        /// <summary>
        /// Called when the quest is contracted.
        /// </summary>
        public override void OnContracted(Character character)
        {
            base.OnContracted();
            foreach (var item in character.ImportantItems)
            {
                if (!item.GetType().Name.Equals(ItemName)) continue;
                QuestProgress++;
                if (QuestProgress >= QuestGoal) { IsCompleted = true; break; }
            }
        }

        /// <summary>
        /// Called when the quest is in progress.
        /// </summary>
        /// <param name="character"></param>
        public override void OnProgress(Character character)
        {
            if (!IsContracted || IsCompleted) return;

            QuestProgress = 0;
            foreach (var item in character.ImportantItems)
            {
                if (!item.GetType().Name.Equals(ItemName)) continue;
                QuestProgress++;
                if (QuestProgress >= QuestGoal) { IsCompleted = true; break; }
            }
        }

        /// <summary>
        /// Called when the quest is completed.
        /// </summary>
        /// <param name="character"></param>
        public override void OnCompleted(Character character)
        {
            base.OnCompleted(character);
            RemoveQuestItems(character);
        }

        /// <summary>
        /// Describe the quest.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new(base.ToString());
            if (IsContracted) sb.AppendLine($"| 진행도 : '{QuestProgress}/{QuestGoal}' |");
            return sb.ToString();
        }

        /// <summary>
        /// Remove quest items from the character's inventory.
        /// </summary>
        /// <param name="character"></param>
        private void RemoveQuestItems(Character character)
        {
            var current = character.ImportantItems.First;
            int count = 0;
            while(current != null)
            {
                var next = current.Next;
                if (count >= QuestGoal) break;
                if (current.Value.GetType().Name.Equals(ItemName))
                {
                    character.ImportantItems.Remove(current);
                    Console.WriteLine($"| '{current.Value.Name}' 아이템을 제거하였습니다! |");
                    count++;
                }
                current = next;
            }
        }
    }
}

