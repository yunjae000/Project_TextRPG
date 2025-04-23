using System.IO;

namespace TextRPG
{
    class InGame
    {
        // Property
        public GameManager GameManager { get; private set; }
        public SpawnManager SpawnManager { get; private set; }
        public QuestManager QuestManager { get; private set; }

        // Constructor
        public InGame(GameManager gameManager, SpawnManager spawnManager, QuestManager questManager)
        {
            GameManager = gameManager;
            SpawnManager = spawnManager;
            QuestManager = questManager;
        }

        /// <summary>
        /// Player can start the game or end the game.
        /// </summary>
        /// <returns>Returns true, when player chooses to start the game.</returns>
        private bool InMainMenu()
        {
            int option;
            while (true)
            {
                Console.Clear();
                UIManager.GameOptionUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(true); }
                else if (opt < 1 || opt > Enum.GetValues(typeof(GameOption)).Length) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(true); }
                else { option = Math.Clamp(opt, 1, Enum.GetValues(typeof(GameOption)).Length); break; }
            }

            switch ((GameOption)(option - 1))
            {
                case GameOption.NewGame:
                    GameManager.SelectJob(); break;
                case GameOption.Continue:
                    GameManager.LoadGame(); Console.Write("Press any key to continue...");
                    Console.ReadKey(); break;
                case GameOption.Exit: return false;
            }

            return true;
        }

        #region Town Method and Misc. Methods -> Rest, Inventory, Status, Option, Shop
        /// <summary>
        /// Player can go to cabin and choose to how much health to be restored.
        /// </summary>
        private void InRest()
        {
            bool isSelected = false;
            int option = 1;

            while (!isSelected)
            {
                Console.Clear();
                UIManager.CabinUI();

                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); continue; }
                else if (opt < 1 || opt > 4) { Console.WriteLine("| 잘못된 입력입니다! |"); continue; }
                else { option = opt; }

                switch (Math.Clamp(option, 1, 4))
                {
                    case 1: Console.WriteLine("| 좋은 하루 되세요! |"); return;
                    case 2:
                        if (GameManager.SelectedCharacter.Currency < 40) { Console.WriteLine("| 돈이 부족합니다! |"); }
                        else { isSelected = true; }
                        break;
                    case 3:
                        if (GameManager.SelectedCharacter.Currency < 60) { Console.WriteLine("| 돈이 부족합니다! |"); }
                        else { isSelected = true; }
                        break;
                    case 4:
                        if (GameManager.SelectedCharacter.Currency < 80) { Console.WriteLine("| 돈이 부족합니다! |"); }
                        else { isSelected = true; }
                        break;
                }
            }

            Console.WriteLine("| 좋은 꿈 꾸세요! |");

            GameManager.SelectedCharacter.Currency -= (option * 20);
            GameManager.SelectedCharacter.OnHeal(GameManager.SelectedCharacter.MaxHealth * (0.25f + (0.25f * (option - 2))));
            GameManager.SelectedCharacter.OnMagicPointHeal(GameManager.SelectedCharacter.MaxMagicPoint * (0.25f + (0.25f * (option - 2))));
            if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
            else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
        }

        /// <summary>
        /// Player can look at the inventory of the character.
        /// </summary>
        private void InInventory()
        {
            while (true)
            {
                // Prints Inventory UI
                Console.Clear();
                UIManager.InventoryUI(GameManager.SelectedCharacter);

                // Get Input from user
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(); continue; }
                else if (opt < 1 || opt > 2) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(); continue; }

                // If "Back" option selected, go back to main game.
                if (opt == 1) return;

                // Select Category and Index of Item
                Console.Write("Type item category and index ( Type [ Category,Index ] ) : ");
                string[]? vals = Console.ReadLine()?.Split(new char[] { ',', ' ', '|' });
                if (vals == null || vals.Length < 2) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(); continue; }
                if (!int.TryParse(vals[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(); continue; }
                if (!int.TryParse(vals[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press any key to continue..."); Console.ReadKey(); continue; }

                ItemCategory category = (ItemCategory)(Math.Clamp(cat, 1, Enum.GetValues(typeof(ItemCategory)).Length) - 1);

                // Get interfaces from selected item to change its state
                InInventory_SelectItem(category, ind, out IWearable? wearable, out IPickable? pickable, out IUseable? useable);

                // Check If there is item in array
                if (wearable == null && useable == null && pickable == null) { Console.WriteLine("| Selected Category is empty! |"); break; }

                // Select Item and Modify
                if (!InInventory_ChangeStateOfItem(category, wearable, useable, pickable)) continue;

                Console.Write("Press any key to continue...");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// This method is used to extract selected item's interfaces.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ind"></param>
        /// <param name="wearable"></param>
        /// <param name="pickable"></param>
        /// <param name="useable"></param>
        private void InInventory_SelectItem(ItemCategory category, int ind, out IWearable? wearable, out IPickable? pickable, out IUseable? useable)
        {
            Character character = GameManager.SelectedCharacter;
            wearable = null; pickable = null; useable = null;

            switch (category)
            {
                case ItemCategory.Armor:
                    if (character.Armors.Count < 1) break;
                    wearable = character.Armors[Math.Clamp(ind - 1, 0, character.Armors.Count - 1)];
                    pickable = character.Armors[Math.Clamp(ind - 1, 0, character.Armors.Count - 1)];
                    break;
                case ItemCategory.Weapon:
                    if (character.Weapons.Count < 1) break;
                    wearable = character.Weapons[Math.Clamp(ind - 1, 0, character.Weapons.Count - 1)];
                    pickable = character.Weapons[Math.Clamp(ind - 1, 0, character.Weapons.Count - 1)];
                    break;
                case ItemCategory.Consumable:
                    if (character.Consumables.Count < 1) break;
                    useable = character.Consumables[Math.Clamp(ind - 1, 0, character.Consumables.Count - 1)];
                    break;
                case ItemCategory.Misc:
                    if (character.ImportantItems.Count < 1) break;
                    pickable = character.ImportantItems[Math.Clamp(ind - 1, 0, character.ImportantItems.Count - 1)];
                    break;
            }
        }

        /// <summary>
        /// This Method is used to change selected item state.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="wearable"></param>
        /// <param name="useable"></param>
        /// <param name="pickable"></param>
        /// <returns></returns>
        private bool InInventory_ChangeStateOfItem(ItemCategory category, IWearable? wearable, IUseable? useable, IPickable? pickable)
        {
            // Select Item and Modify
            if (category == ItemCategory.Armor || category == ItemCategory.Weapon)
            {
                UIManager.InventoryUI_Equipment();
                if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| 잘못된 입력입니다! |"); return false; }

                switch (Math.Clamp(index, 1, 4))
                {
                    case 1: break;
                    case 2: wearable?.OnEquipped(GameManager.SelectedCharacter); break;
                    case 3: wearable?.OnUnequipped(GameManager.SelectedCharacter); break;
                    case 4: pickable?.OnDropped(GameManager.SelectedCharacter); break;
                }
            }
            else if (category == ItemCategory.Consumable)
            {
                UIManager.InventoryUI_Consumable();
                if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| 잘못된 입력입니다! |"); return false; }

                switch (Math.Clamp(index, 1, 2))
                {
                    case 1: break;
                    case 2: useable?.OnUsed(GameManager.SelectedCharacter); break;
                }
            }
            else
            {
                UIManager.InventoryUI_Misc();
                if (!int.TryParse(Console.ReadLine(), out int index)) { Console.WriteLine("| 잘못된 입력입니다! |"); return false; }
                switch (Math.Clamp(index, 1, 2))
                {
                    case 1: break;
                    case 2: pickable?.OnDropped(GameManager.SelectedCharacter); break;
                }
            }
            return true;
        }

        /// <summary>
        /// Player can look at the current status of the character
        /// </summary>
        private void InStatus()
        {
            Console.Clear();
            UIManager.StatusUI(GameManager.SelectedCharacter);
        }

        /// <summary>
        /// Gives interface that player can save, load, or end game.
        /// </summary>
        private void InOption()
        {
            while (true)
            {
                Console.Clear();
                UIManager.SettingOptionUI();

                if (!int.TryParse(Console.ReadLine(), out int opt))
                {
                    Console.WriteLine("| 잘못된 입력입니다! |");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey(true);
                    continue;
                }

                switch ((SettingOptions)(opt - 1))
                {
                    case SettingOptions.Back: return;
                    case SettingOptions.Save: GameManager.SaveGame(); break;
                    case SettingOptions.Load: GameManager.LoadGame(); break;
                    case SettingOptions.EndGame: GameManager.GameState = GameState.MainMenu; Console.WriteLine(); return;
                    default:
                        Console.WriteLine("| 잘못된 입력입니다! |"); break;
                }
                Console.Write("Press any key to continue...");
                Console.ReadKey(true);
            }
        }

        /// <summary>
        /// Gives interface what player can buy or sell items.
        /// </summary>
        private void InShop()
        {
            Character character = GameManager.SelectedCharacter;

            while (true)
            {
                Console.Clear();
                UIManager.ShopUI(character);
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); continue; }
                else if (opt < 0 || opt > 4) { Console.WriteLine("| 잘못된 입력입니다! |"); continue; }
                switch (Math.Clamp(opt, 0, 4))
                {
                    // Exit from shop
                    case 0: Console.WriteLine("| Have a nice day! |"); return;

                    // Buy Armor in shop
                    case 1:
                        UIManager.ShowShopList(ItemCategory.Armor);
                        if (!int.TryParse(Console.ReadLine(), out int ind1)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        else if (ind1 <= 0) break;
                        InShop_Buy(ItemCategory.Armor, ind1); break;

                    // Buy Weapon in shop
                    case 2:
                        UIManager.ShowShopList(ItemCategory.Weapon);
                        if (!int.TryParse(Console.ReadLine(), out int ind2)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        else if (ind2 <= 0) break;
                        InShop_Buy(ItemCategory.Weapon, ind2);
                        break;

                    // Buy Consumable in shop
                    case 3:
                        UIManager.ShowShopList(ItemCategory.Consumable);
                        if (!int.TryParse(Console.ReadLine(), out int ind3)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        else if (ind3 <= 0) break;
                        InShop_Buy(ItemCategory.Consumable, ind3);
                        break;

                    // Sell Item in inventory
                    case 4:
                        UIManager.ShowItemList(character);
                        string? input = Console.ReadLine(); if (input == null || input.Equals("exit")) break;

                        string[]? vals = input.Split(new char[] { ',', ' ', '|' });
                        if (vals == null) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        if (!int.TryParse(vals[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        if (!int.TryParse(vals[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind4)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        InShop_Sell((ItemCategory)Math.Clamp(cat - 1, 0, Enum.GetValues(typeof(ItemCategory)).Length - 1), ind4);
                        break;
                }
            }
        }

        /// <summary>
        /// Purchase Mechanism of item in shop.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ind"></param>
        private void InShop_Buy(ItemCategory category, int ind)
        {
            switch (category)
            {
                case ItemCategory.Armor:
                    if (ItemLists.Armors.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    ItemLists.Armors[ind - 1].OnPurchased(GameManager.SelectedCharacter); break;
                case ItemCategory.Weapon:
                    if (ItemLists.Weapons.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    ItemLists.Weapons[ind - 1].OnPurchased(GameManager.SelectedCharacter); break;
                case ItemCategory.Consumable:
                    if (ItemLists.Consumables.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    ItemLists.Consumables[ind - 1].OnPurchased(GameManager.SelectedCharacter); break;
            }
            Console.Write("\nPress any key to continue..."); Console.ReadKey(true);
        }

        /// <summary>
        /// Selling Mechanism of item in player inventory.
        /// </summary>
        /// <param name="category"></param>
        /// <param name="ind"></param>
        private void InShop_Sell(ItemCategory category, int ind)
        {
            switch (category)
            {
                case ItemCategory.Armor:
                    if (GameManager.SelectedCharacter.Armors.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.Armors[ind - 1].OnSold(GameManager.SelectedCharacter); break;
                case ItemCategory.Weapon:
                    if (GameManager.SelectedCharacter.Weapons.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.Weapons[ind - 1].OnSold(GameManager.SelectedCharacter); break;
                case ItemCategory.Consumable:
                    if (GameManager.SelectedCharacter.Consumables.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.Consumables[ind - 1].OnSold(GameManager.SelectedCharacter); break;
                case ItemCategory.Misc:
                    if (GameManager.SelectedCharacter.ImportantItems.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.ImportantItems[ind - 1].OnSold(GameManager.SelectedCharacter); break;
            }
            Console.Write("\nPress any key to continue..."); Console.ReadKey(true);
        }

        /// <summary>
        /// Gives interface what player can do in quest.
        /// </summary>
        private void InQuest()
        {
            while (true)
            {
                Console.Clear();
                UIManager.QuestUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); continue; }
                else if (opt < 1 || opt > 6) { Console.WriteLine("| 잘못된 입력입니다! |"); continue; }

                switch (Math.Clamp(opt, 1, 6))
                {
                    case 1: return;
                    case 2: ContractQuest(); break;
                    case 3: CompleteQuest(GameManager.SelectedCharacter); break;
                    case 4: ShowQuests(QuestStatus.NotStarted); break;
                    case 5: ShowQuests(QuestStatus.InProgress); break;
                    case 6: ShowQuests(QuestStatus.Completed); break;
                }
            }
        }

        /// <summary>
        /// Contract Quest Mechanism
        /// </summary>
        private void ContractQuest()
        {
            UIManager.QuestUI_Contract();

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            else if (opt < 1 || opt > QuestManager.GetContractableQuests().Count()) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }

            while (true)
            {
                Console.Write("이 Quest를 정말로 수주할 것입니까? (Y/N) : ");
                char key = char.ToLower(Console.ReadKey(true).KeyChar);

                if (key.Equals('n')) return;
                else if (key.Equals('y')) break;
                else { Console.WriteLine("| 잘못된 입력입니다! |"); }
            }

            var quest = QuestManager.GetContractableQuests().ElementAt(opt - 1);
            if (quest is KillMonsterQuest) ((KillMonsterQuest)quest).OnContracted();
            else ((CollectItemQuest)quest).OnContracted(GameManager.SelectedCharacter);

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Complete Quest Mechanism
        /// </summary>
        /// <param name="character"></param>
        private void CompleteQuest(Character character)
        {
            UIManager.QuestUI_Complete();

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            else if (opt < 1 || opt > QuestManager.GetCompletableQuests().Count()) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }

            while (true)
            {
                Console.Write("이 Quest를 정말로 완료할 것입니까? (Y/N) : ");
                char key = char.ToLower(Console.ReadKey(true).KeyChar);

                if (key.Equals('n')) return;
                else if (key.Equals('y')) break;
                else { Console.WriteLine("| 잘못된 입력입니다! |"); }
            }

            var quest = QuestManager.GetCompletableQuests().ElementAt(opt - 1);
            if(quest is KillMonsterQuest) ((KillMonsterQuest)quest).OnCompleted(character);
            else if (quest is CollectItemQuest) ((CollectItemQuest)quest).OnCompleted(character);

            Console.Write("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Shows quests by type in the game.
        /// </summary>
        private void ShowQuests(QuestStatus type)
        {
            if (type == QuestStatus.NotStarted) { foreach (var quest in QuestManager.GetContractableQuests()) Console.WriteLine($"{quest.ToString()}"); }
            else if (type == QuestStatus.InProgress) { foreach (var quest in QuestManager.GetContractedQuests()) Console.WriteLine($"{quest.ToString()}"); }
            else if (type == QuestStatus.Completable) { foreach (var quest in QuestManager.GetCompletableQuests()) Console.WriteLine($"{quest.ToString()}"); }
            else { foreach (var quest in QuestManager.GetCompletedQuests()) Console.WriteLine($"{quest.ToString()}"); }
            Console.WriteLine("| Press any key to continue... |");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Gives interface what player can do in town(Shop, Rest, Dungeon, Inventory, Status, Option).
        /// </summary>
        private void InTown()
        {
            Console.Clear();

            UIManager.BaseUI(GameManager.SelectedCharacter, "The Town of Adventurers", typeof(IdleOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            else if (opt < 1 || opt > Enum.GetValues(typeof(IdleOptions)).Length) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            switch ((IdleOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(IdleOptions)).Length - 1)))
            {
                case IdleOptions.Shop: InShop(); break;
                case IdleOptions.Quest: InQuest(); break;
                case IdleOptions.Dungeon: GameManager.GameState = GameState.Dungeon; break;
                case IdleOptions.Rest: InRest(); break;
                case IdleOptions.Inventory: InInventory(); break;
                case IdleOptions.Status: InStatus(); break;
                case IdleOptions.Option: InOption(); break;
                default: Console.WriteLine("| Something is wrong! |"); break;
            }
        }
        #endregion

        #region Dungeon Methods
        /// <summary>
        /// Gives interface what player can do in dungeon(Path selection, Inventory, Status, BackToTown).
        /// </summary>
        private void InDungeon()
        {
            // Check for Quota completion
            if (GameManager.KilledMonsterCount >= GameManager.Quota)
            {
                GameManager.GoToNextLevel();
            }

            // Print UI of Kill Count and Player Options
            Console.Clear();
            int[] pathOptions = RandomPathOption();
            UIManager.KillCountUI(GameManager.KilledMonsterCount, GameManager.Quota);
            UIManager.DungeonUI(GameManager.SelectedCharacter, GameManager, pathOptions);

            // TODO: Insert Dungeon Path UI

            Random rand = new Random();
            int index = rand.Next(Miscs.path.Length);

            foreach (string line in Miscs.path[index].Split('\n'))
            {
                Console.WriteLine(line);
            }
            // Try parsing, if successed clamp Parsed Input
            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            else if (opt < 1 || opt > (pathOptions.Length + Enum.GetValues(typeof(DungeonOptions)).Length - 4)) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            opt = Math.Clamp(opt - 1, 0, (pathOptions.Length + Enum.GetValues(typeof(DungeonOptions)).Length - 4) - 1);

            // Choices
            if (opt >= 0 && opt < pathOptions.Length) { InDungeon_MonsterEncounter((DungeonOptions)pathOptions[opt]); }
            else if (opt <= pathOptions.Length) { InInventory(); }
            else if (opt <= pathOptions.Length + 1) { InStatus(); }
            else { InDungeon_ReturnToTown(); }
        }

        /// <summary>
        /// Randomly generates path options in dungeon.
        /// </summary>
        /// <returns></returns>
        private int[] RandomPathOption()
        {
            int random = new Random().Next(0, 7);
            while(random == GameManager.prevPath) { random = new Random().Next(0, 7); }
            GameManager.prevPath = random;
            
            // TODO: Print Path UI

            return random switch
            {
                0 => new int[] { (int)DungeonOptions.Forward, (int)DungeonOptions.Backward },
                1 => new int[] { (int)DungeonOptions.Left, (int)DungeonOptions.Backward },
                2 => new int[] { (int)DungeonOptions.Right, (int)DungeonOptions.Backward },
                3 => new int[] { (int)DungeonOptions.Forward, (int)DungeonOptions.Left, (int)DungeonOptions.Backward },
                4 => new int[] { (int)DungeonOptions.Forward, (int)DungeonOptions.Right, (int)DungeonOptions.Backward },
                5 => new int[] { (int)DungeonOptions.Left, (int)DungeonOptions.Right, (int)DungeonOptions.Backward },
                6 => new int[] { (int)DungeonOptions.Forward, (int)DungeonOptions.Left, (int)DungeonOptions.Right, (int)DungeonOptions.Backward },
                _ => new int[] { (int)DungeonOptions.Forward, (int)DungeonOptions.Backward },
            };
        }

        /// <summary>
        /// Mechanism of Monster Encounter in dungeon.
        /// </summary>
        /// <param name="option"></param>
        private void InDungeon_MonsterEncounter(DungeonOptions option)
        {
            int random = new Random().Next(0, 10);
            if (random >= ((int)option * 2) && random < ((int)option * 2 + 2 + ((int)option % 2)))
            {
                SpawnManager.SpawnMonsters(GameManager.SelectedCharacter, GameManager.GroundLevel);
                UIManager.MonsterEncounterUI(SpawnManager);
                GameManager.GameState = GameState.Battle;
            }
            else { UIManager.NoMonsterFoundUI(); }
        }

        /// <summary>
        /// Exit from dungeon and return to town.
        /// </summary>
        private void InDungeon_ReturnToTown()
        {
            GameManager.GameState = GameState.Town;
            if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
            else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
            Console.WriteLine("| 마을로 무사귀환하였습니다... |");
            Console.Write("Press any key to continue..."); Console.ReadKey(true);
        }
        #endregion

        #region Battle Methods
        /// <summary>
        /// Gives interface what player can do in battle(Attack, Inventory, Status, Escape).
        /// </summary>
        private void InBattle()
        {
            Console.Clear();
            UIManager.BaseUI(GameManager.SelectedCharacter, "Kill the monsters", typeof(BattleOptions));
            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }
            else if (opt < 1 || opt > Enum.GetValues(typeof(BattleOptions)).Length) { Console.WriteLine("| 잘못된 입력입니다! |"); return; }

            // Player Options
            switch ((BattleOptions)Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(BattleOptions)).Length - 1))
            {
                case BattleOptions.Attack: if (!InBattle_Attack()) return; break;
                case BattleOptions.Skill: 
                    if (!InBattle_Skill()) return; break;
                case BattleOptions.Inventory: InInventory(); return;
                case BattleOptions.Status: InStatus(); return;
                case BattleOptions.Escape:
                    SpawnManager.RemoveAllMonsters();
                    InBattle_EscapeFromBattle("전투에서 도망쳤습니다!");
                    return;
                default: Console.WriteLine("| Something is wrong! |"); return;
            }

            // Check if all monsters are dead
            if (SpawnManager.GetMonsterCount() <= 0)
            {
                InBattle_EscapeFromBattle("모든 몬스터들을 무찔렀습니다!");
                return;
            }

            // Monster Attack Mechanism
            foreach (Monster monster in SpawnManager.spawnedMonsters)
            {
                if (monster.AttackType == AttackType.Close) GameManager.SelectedCharacter.OnDamage(AttackType.Close, monster.AttackStat.Attack);
                else if (monster.AttackType == AttackType.Long) GameManager.SelectedCharacter.OnDamage(AttackType.Long, monster.AttackStat.RangeAttack);
                else GameManager.SelectedCharacter.OnDamage(AttackType.Magic, monster.AttackStat.MagicAttack);
            }

            GameManager.CurrentTurn++;
            InBattle_RemoveBuffSkills(true);

            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
        }

        /// <summary>
        /// Attack Mechanism of player
        /// </summary>
        private bool InBattle_Attack()
        {
            UIManager.ShowMonsterList(SpawnManager);
            int opt;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out int ind)) Console.WriteLine("| 잘못된 입력입니다! |");
                else if (ind < 0 || ind > SpawnManager.GetMonsterCount()) Console.WriteLine("| 잘못된 입력입니다! |");
                else { opt = Math.Clamp(ind, 0, SpawnManager.GetMonsterCount()); break; }
            }

            if (opt <= 0) return false;

            if (opt > 0 && opt <= SpawnManager.GetMonsterCount())
            {
                AttackType? type = GameManager.SelectedCharacter.EquippedWeapon?.AttackType;
                switch (type)
                {
                    case AttackType.Close: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.Attack); break;
                    case AttackType.Long: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Long, GameManager.SelectedCharacter.AttackStat.RangeAttack); break;
                    case AttackType.Magic: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Magic, GameManager.SelectedCharacter.AttackStat.MagicAttack); break;
                    default: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Close, GameManager.SelectedCharacter.AttackStat.Attack); break;
                }
            }
            return true;
        }

        /// <summary>
        /// Skill Mechanism of player
        /// </summary>
        private bool InBattle_Skill()
        {
            UIManager.ShowSkillList(GameManager.SelectedCharacter);

            int skillOpt;
            while (true)
            {
                if (!int.TryParse(Console.ReadLine(), out int ind)) Console.WriteLine("| 잘못된 입력입니다! |");
                else if (ind < 0 || ind > GameManager.SelectedCharacter.Skills.Count) Console.WriteLine("| 잘못된 입력입니다! |");
                else { skillOpt = Math.Clamp(ind, 0, GameManager.SelectedCharacter.Skills.Count); break; }
                Console.Write("Press any key to continue...");
                Console.ReadKey(true);
            }

            if (skillOpt <= 0) return false;

            var skill = GameManager.SelectedCharacter.Skills[skillOpt - 1];
            if (skill is ActiveSkill attackSkill)
            {
                if (GameManager.SelectedCharacter.EquippedWeapon == null)
                {
                    Console.WriteLine("\n| 무기를 장착해야 액티브 스킬을 사용할 수 있습니다! |");
                    Console.Write("Press any key to continue...");
                    Console.ReadKey(true);
                    return false;
                }

                if (!attackSkill.IsTargetable)
                {
                    bool isSuccess = attackSkill.OnActive(GameManager.SelectedCharacter, SpawnManager.spawnedMonsters);
                    if (!isSuccess)
                    {
                        Console.WriteLine("| 마나가 부족합니다! |");
                        Console.Write("Press any key to continue...");
                        Console.ReadKey(true);
                    }
                    return isSuccess;
                }
                    

                UIManager.ShowMonsterList(SpawnManager);
                int monsterOpt;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out int ind)) Console.WriteLine("| 잘못된 입력입니다! |");
                    else if (ind < 0 || ind > SpawnManager.GetMonsterCount()) Console.WriteLine("| 잘못된 입력입니다! |");
                    else { monsterOpt = Math.Clamp(ind, 0, SpawnManager.GetMonsterCount()); break; }
                }
                if (monsterOpt <= 0) return false;

                return attackSkill.OnActive(GameManager.SelectedCharacter, SpawnManager.spawnedMonsters.ElementAt(monsterOpt - 1));
            }
            else if (skill is BuffSkill buffSkill) return buffSkill.OnActive(GameManager.SelectedCharacter);
            return false;
        }

        /// <summary>
        /// Removes buffs from character. 
        /// If checkTurn is true, it checks if the buff is expired or not.
        /// </summary>
        /// <param name="checkTurn"></param>
        private void InBattle_RemoveBuffSkills(bool checkTurn)
        {
            var buffSkills = from skill in GameManager.SelectedCharacter.Skills
                             where skill is BuffSkill
                             select (BuffSkill)skill;
            foreach (BuffSkill skill in buffSkills)
            {
                if (checkTurn) { if (GameManager.CurrentTurn - skill.UsedTurn > skill.TurnInterval) skill.OnBuffExpired(GameManager.SelectedCharacter); }
                else skill.OnBuffExpired(GameManager.SelectedCharacter);
            }
        }

        /// <summary>
        /// Escape from battle and return to dungeon.
        /// </summary>
        /// <param name="headLine"></param>
        private void InBattle_EscapeFromBattle(string headLine)
        {
            InBattle_RemoveBuffSkills(false);

            Console.WriteLine($"\n| {headLine} |");
            Console.Write("| Press any key to continue... |");
            Console.ReadKey(true);

            GameManager.CurrentTurn = 1;
            GameManager.GameState = GameState.Dungeon;
        }
        #endregion

        /// <summary>
        /// Runs the game.
        /// </summary>
        public void MainGame()
        {
            while (true)
            {
                switch (GameManager.GameState)
                {
                    case GameState.MainMenu: if (!InMainMenu()) return; break;
                    case GameState.Town: InTown(); break;
                    case GameState.Dungeon: InDungeon(); break;
                    case GameState.Battle: InBattle(); break;
                    case GameState.GameOver: break;
                }
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // Game Start UI
            UIManager.StartUI();
            InGame inGame = new(new GameManager(), new SpawnManager(), new QuestManager());

            // Main Game
            inGame.MainGame();
        }
    }
}