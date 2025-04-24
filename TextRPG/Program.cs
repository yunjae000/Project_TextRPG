using System.IO;

namespace TextRPG
{
    class InGame
    {
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
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); }
                else if (opt < 1 || opt > Enum.GetValues(typeof(GameOption)).Length) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); }
                else { option = Math.Clamp(opt, 1, Enum.GetValues(typeof(GameOption)).Length); break; }
            }

            switch ((GameOption)(option - 1))
            {
                case GameOption.NewGame:
                    GameManager.SelectJob(); break;
                case GameOption.Continue:
                    GameManager.LoadGame(); Console.Write("\nPress enter to continue...");
                    Console.ReadLine(); break;
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
                UIManager.CabinUI(GameManager.SelectedCharacter);

                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); continue; }
                else if (opt < 1 || opt > 4) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); continue; }
                else { option = opt; }

                switch (Math.Clamp(option, 1, 4))
                {
                    case 1: Console.WriteLine("| 좋은 하루 되세요! |"); Console.Write("Press enter to continue..."); Console.ReadLine(); return;
                    case 2:
                        if (GameManager.SelectedCharacter.Currency < 40) { Console.WriteLine("| 돈이 부족합니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); }
                        else 
                        {
                            foreach (string line in Miscs.Rest1) Console.WriteLine(line);
                            isSelected = true; 
                        }
                        break;
                    case 3:
                        if (GameManager.SelectedCharacter.Currency < 60) { Console.WriteLine("| 돈이 부족합니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); }
                        else 
                        { 
                            foreach(string line in Miscs.Rest2) Console.WriteLine(line);
                            isSelected = true; 
                        }
                        break;
                    case 4:
                        if (GameManager.SelectedCharacter.Currency < 80) { Console.WriteLine("| 돈이 부족합니다! |");Console.Write("\nPress enter to continue..."); Console.ReadLine(); }
                        else 
                        { 
                            foreach (string line in Miscs.Rest3) Console.WriteLine(line);
                            isSelected = true;
                        }
                        break;
                }
            }

            GameManager.SelectedCharacter.Currency -= (option * 20);
            GameManager.SelectedCharacter.OnHeal(GameManager.SelectedCharacter.MaxHealth * (0.25f + (0.25f * (option - 2))));
            GameManager.SelectedCharacter.OnMagicPointHeal(GameManager.SelectedCharacter.MaxMagicPoint * (0.25f + (0.25f * (option - 2))));
            if (GameManager.GameTime == GameTime.Afternoon) GameManager.GameTime = GameTime.Night;
            else { GameManager.GameTime = GameTime.Afternoon; GameManager.RemoveAllBuffs(); }
            
            Console.WriteLine("| 좋은 꿈 꾸세요! |");
            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
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
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press enter to continue..."); Console.ReadLine(); continue; }
                else if (opt < 1 || opt > 2) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press enter to continue..."); Console.ReadLine(); continue; }

                // If "Back" option selected, go back to main game.
                if (opt == 1) return;

                // Select Category and Index of Item
                Console.Write("Type item category and index ( Type [ Category,Index ] ) : ");
                string[]? vals = Console.ReadLine()?.Split(new char[] { ',', ' ', '|' });
                if (vals == null || vals.Length < 2) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press enter to continue..."); Console.ReadLine(); continue; }
                if (!int.TryParse(vals[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press enter to continue..."); Console.ReadLine(); continue; }
                if (!int.TryParse(vals[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("Press enter to continue..."); Console.ReadLine(); continue; }

                ItemCategory category = (ItemCategory)(Math.Clamp(cat, 1, Enum.GetValues(typeof(ItemCategory)).Length) - 1);

                // Get interfaces from selected item to change its state
                InInventory_SelectItem(category, ind, out IWearable? wearable, out IPickable? pickable, out IUseable? useable);

                // Check If there is item in array
                if (wearable == null && useable == null && pickable == null) { Console.WriteLine("| Selected Category is empty! |"); break; }

                // Select Item and Modify
                if (!InInventory_ChangeStateOfItem(category, wearable, useable, pickable)) continue;

                Console.Write("Press enter to continue...");
                Console.ReadLine();
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
                    pickable = character.ImportantItems.ElementAt(Math.Clamp(ind - 1, 0, character.ImportantItems.Count - 1));
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
                    case 2: wearable?.OnEquip(GameManager.SelectedCharacter); break;
                    case 3: wearable?.OnUnequip(GameManager.SelectedCharacter); break;
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
                    Console.Write("\nPress enter to continue...");
                    Console.ReadLine();
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
                Console.Write("\nPress enter to continue...");
                Console.ReadLine();
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
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); continue; }
                else if (opt < 0 || opt > 4) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); continue; }
                
                switch (Math.Clamp(opt, 0, 4))
                {
                    // Exit from shop
                    case 0: Console.WriteLine("| Have a nice day! |"); return;

                    // Buy Armor in shop
                    case 1:
                        UIManager.ShowShopList(character, ItemCategory.Armor);
                        if (!int.TryParse(Console.ReadLine(), out int ind1)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        else if (ind1 <= 0) break;
                        InShop_Buy(ItemCategory.Armor, ind1); break;

                    // Buy Weapon in shop
                    case 2:
                        UIManager.ShowShopList(character, ItemCategory.Weapon);
                        if (!int.TryParse(Console.ReadLine(), out int ind2)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        else if (ind2 <= 0) break;
                        InShop_Buy(ItemCategory.Weapon, ind2);
                        break;

                    // Buy Consumable in shop
                    case 3:
                        UIManager.ShowShopList(character, ItemCategory.Consumable);
                        if (!int.TryParse(Console.ReadLine(), out int ind3)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        else if (ind3 <= 0) break;
                        InShop_Buy(ItemCategory.Consumable, ind3);
                        break;

                    // Sell Item in inventory
                    case 4:
                        UIManager.ShowItemList(character);
                        string? input = Console.ReadLine(); if (input == null || input.Equals("exit")) break;

                        string[]? vals = input.Split(new char[] { ',', ' ', '|' });
                        if (vals == null || vals.Length < 2) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        if (!int.TryParse(vals[0].Trim(new char[] { '[', ']', ' ', ',' }), out int cat)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        if (!int.TryParse(vals[1].Trim(new char[] { '[', ']', ' ', ',' }), out int ind4)) { Console.WriteLine("| 잘못된 입력입니다! |"); break; }
                        InShop_Sell((ItemCategory)Math.Clamp(cat - 1, 0, Enum.GetValues(typeof(ItemCategory)).Length - 1), ind4);
                        break;
                }
                Console.Write("\nPress enter to continue..."); Console.ReadLine();
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
                    ItemLists.Armors[ind - 1].OnPurchase(GameManager.SelectedCharacter); break;
                case ItemCategory.Weapon:
                    if (ItemLists.Weapons.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    ItemLists.Weapons[ind - 1].OnPurchase(GameManager.SelectedCharacter); break;
                case ItemCategory.Consumable:
                    if (ItemLists.Consumables.Length < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    ItemLists.Consumables[ind - 1].OnPurchase(GameManager.SelectedCharacter); break;
            }
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
                    GameManager.SelectedCharacter.Armors[ind - 1].OnSell(GameManager.SelectedCharacter); break;
                case ItemCategory.Weapon:
                    if (GameManager.SelectedCharacter.Weapons.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.Weapons[ind - 1].OnSell(GameManager.SelectedCharacter); break;
                case ItemCategory.Consumable:
                    if (GameManager.SelectedCharacter.Consumables.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.Consumables[ind - 1].OnSell(GameManager.SelectedCharacter); break;
                case ItemCategory.Misc:
                    if (GameManager.SelectedCharacter.ImportantItems.Count < ind || ind < 1)
                    {
                        Console.WriteLine("| 아이템이 존재하지 않습니다! |"); break;
                    }
                    GameManager.SelectedCharacter.ImportantItems.ElementAt(ind - 1).OnSell(GameManager.SelectedCharacter); break;
            }
        }

        /// <summary>
        /// Gives interface what player can do in quest.
        /// </summary>
        private void InQuest()
        {
            while (true)
            {
                Console.Clear();
                foreach (string line in Miscs.Quest) Console.WriteLine(line);
                UIManager.QuestUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); continue; }
                else if (opt < 1 || opt > 7) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); continue; }

                switch (Math.Clamp(opt, 1, 7))
                {
                    case 1: return;
                    case 2: ContractQuest(); break;
                    case 3: CompleteQuest(GameManager.SelectedCharacter); break;
                    case 4: ShowQuests(QuestStatus.NotStarted); break;
                    case 5: ShowQuests(QuestStatus.Completable); break;
                    case 6: ShowQuests(QuestStatus.InProgress); break;
                    case 7: ShowQuests(QuestStatus.Completed); break;
                }
            }
        }

        /// <summary>
        /// Contract Quest Mechanism
        /// </summary>
        private void ContractQuest()
        {
            UIManager.QuestUI_Contract();

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }
            else if (opt < 1 || opt > QuestManager.GetContractableQuests().Count()) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }

            while (true)
            {
                Console.Write("이 Quest를 정말로 수주할 것입니까? (Y/N) : ");
                string key = Console.ReadLine();
                if (key.Equals("Y", StringComparison.OrdinalIgnoreCase)) { break; }
                else if (key.Equals("N", StringComparison.OrdinalIgnoreCase)) { return; }
                else { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter key to continue..."); Console.ReadLine(); }
            }

            var quest = QuestManager.GetContractableQuests().ElementAt(opt - 1);
            if (quest is KillMonsterQuest killMonsterQuest) killMonsterQuest.OnContracted();
            else if (quest is CollectItemQuest collectItemQuest) collectItemQuest.OnContracted(GameManager.SelectedCharacter);

            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Complete Quest Mechanism
        /// </summary>
        /// <param name="character"></param>
        private void CompleteQuest(Character character)
        {
            if (!UIManager.QuestUI_Complete()) { Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }
            else if (opt < 1 || opt > QuestManager.GetCompletableQuests().Count()) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }

            while (true)
            {
                Console.Write("이 Quest를 정말로 완료할 것입니까? (Y/N) : ");
                string key = Console.ReadLine();
                if (key.Equals("Y", StringComparison.OrdinalIgnoreCase)) { break; }
                else if (key.Equals("N", StringComparison.OrdinalIgnoreCase)) { return; }
                else { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); }
            }

            var quest = QuestManager.GetCompletableQuests().ElementAt(opt - 1);
            if(quest is KillMonsterQuest killMonsterQuest) killMonsterQuest.OnCompleted(character);
            else if (quest is CollectItemQuest collectItemQuest) collectItemQuest.OnCompleted(character);

            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Shows quests by type in the game.
        /// </summary>
        private void ShowQuests(QuestStatus type)
        {
            if (type == QuestStatus.NotStarted) { foreach (var quest in QuestManager.GetContractableQuests()) Console.WriteLine($"{quest.ToString()}"); }
            else if (type == QuestStatus.InProgress) { foreach (var quest in QuestManager.GetContractedQuests()) Console.WriteLine($"{quest.ToString()}"); }
            else if(type == QuestStatus.Completable) { foreach(var quest in QuestManager.GetCompletableQuests()) Console.WriteLine($"{quest.ToString()}"); }
            else { foreach (var quest in QuestManager.GetCompletedQuests()) Console.WriteLine($"{quest.ToString()}"); }
            Console.Write("\nPress enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Moves player to dungeon.
        /// </summary>
        private void InTown_MoveToDungeon()
        {
            Console.Clear();
            if (GameManager.GroundLevel < 10) foreach (string line in Miscs.EasyEntrance) Console.WriteLine(line);
            else foreach (string line in Miscs.HardEntrance) Console.WriteLine(line);
            GameManager.GameState = GameState.Dungeon;
            Console.WriteLine("\nPress enter to continue...");
            Console.ReadLine();
        }

        /// <summary>
        /// Gives interface what player can do in town(Shop, Rest, Dungeon, Inventory, Status, Option).
        /// </summary>
        private void InTown()
        {
            Console.Clear();
            foreach (string line in Miscs.Town) Console.WriteLine(line);
            UIManager.BaseUI(GameManager.SelectedCharacter, "The Town of Adventurers", typeof(IdleOptions));

            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.WriteLine("\nPress enter to continue..."); Console.ReadLine(); return; }
            else if (opt < 1 || opt > Enum.GetValues(typeof(IdleOptions)).Length) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.WriteLine("\nPress enter to continue..."); Console.ReadLine(); return; }
            switch ((IdleOptions)(Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(IdleOptions)).Length - 1)))
            {
                case IdleOptions.Shop: InShop(); break;
                case IdleOptions.Quest: InQuest(); break;
                case IdleOptions.Dungeon: InTown_MoveToDungeon(); break;
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
            // Print UI of Kill Count and Player Options
            Console.Clear();  
            int[] pathOptions = RandomPathOption(GameManager.IsPathSelected);
            UIManager.KillCountUI(GameManager.KilledMonsterCount, GameManager.Quota);
            UIManager.DungeonUI(GameManager.SelectedCharacter, pathOptions);

            // Try parsing, if successed clamp Parsed Input
            if (!int.TryParse(Console.ReadLine(), out int opt)) { 
                GameManager.IsPathSelected = false; 
                Console.WriteLine("| 잘못된 입력입니다! |"); 
                Console.Write("\nPress enter to continue..."); Console.ReadLine(); 
                return; 
            }
            else if (opt < 1 || opt > (pathOptions.Length + Enum.GetValues(typeof(DungeonOptions)).Length - 4)) { 
                GameManager.IsPathSelected = false; 
                Console.WriteLine("| 잘못된 입력입니다! |"); 
                Console.Write("\nPress enter to continue..."); Console.ReadLine(); 
                return; 
            }
            opt = Math.Clamp(opt - 1, 0, (pathOptions.Length + Enum.GetValues(typeof(DungeonOptions)).Length - 4) - 1);

            // Choices
            if (opt >= 0 && opt < pathOptions.Length) { InDungeon_MonsterEncounter((DungeonOptions)pathOptions[opt]); }
            else if (opt <= pathOptions.Length) { GameManager.IsPathSelected = false; InInventory(); }
            else if (opt <= pathOptions.Length + 1) { GameManager.IsPathSelected = false; InStatus(); }
            else { InDungeon_ReturnToTown(); }
            GameManager.IsPathSelected = true;
        }

        /// <summary>
        /// Randomly generates path options in dungeon.
        /// </summary>
        /// <returns></returns>
        private int[] RandomPathOption(bool IsPathSelected)
        {
            int index;

            if (!IsPathSelected) { index = GameManager.PrevPath; }
            else { 
                int random = new Random().Next(0, Miscs.path.Length);
                while(random == GameManager.PrevPath) { random = new Random().Next(0, Miscs.path.Length); }
                GameManager.PrevPath = random;
                index = random;
            }

            string path = Miscs.path[index];
            Console.WriteLine($"{path}");

            return index switch
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
                UIManager.MonsterEncounterUI();
                GameManager.CurrentTurn = 1;
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
            Console.Write("\nPress enter to continue..."); Console.ReadLine();
        }
        #endregion

        #region Battle Methods
        /// <summary>
        /// Gives interface what player can do in battle(Attack, Inventory, Status, Escape).
        /// </summary>
        private void InBattle()
        {
            Console.Clear();
            UIManager.BattleUI(GameManager.SelectedCharacter, "Kill the monsters");
            if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }
            else if (opt < 1 || opt > Enum.GetValues(typeof(BattleOptions)).Length) { Console.WriteLine("| 잘못된 입력입니다! |"); Console.Write("\nPress enter to continue..."); Console.ReadLine(); return; }

            // Player Options
            switch ((BattleOptions)Math.Clamp(opt - 1, 0, Enum.GetValues(typeof(BattleOptions)).Length - 1))
            {
                case BattleOptions.Attack: if (!InBattle_Attack()) return; break;
                case BattleOptions.Skill: if (!InBattle_Skill()) return; break;
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
                if (monster.AttackType == AttackType.Close) GameManager.SelectedCharacter.OnDamage(AttackType.Close, monster.AttackStat.Attack, false);
                else if (monster.AttackType == AttackType.Long) GameManager.SelectedCharacter.OnDamage(AttackType.Long, monster.AttackStat.RangeAttack, false);
                else GameManager.SelectedCharacter.OnDamage(AttackType.Magic, monster.AttackStat.MagicAttack, false);
            }

            GameManager.CurrentTurn++;
            InBattle_RemoveBuffSkills(true);

            Console.Write("\nPress enter to continue..."); Console.ReadLine();
        }

        /// <summary>
        /// Attack Mechanism of player
        /// </summary>
        private bool InBattle_Attack()
        {
            int opt;
            while (true)
            {
                Console.Clear();
                UIManager.ShowMonsterList();
                if (!int.TryParse(Console.ReadLine(), out int ind)) { Console.WriteLine("| 잘못된 입력입니다! |"); }
                else if (ind < 0 || ind > SpawnManager.GetMonsterCount()) { Console.WriteLine("| 잘못된 입력입니다! |"); }
                else { opt = Math.Clamp(ind, 0, SpawnManager.GetMonsterCount()); break; }
                Console.Write("\nPress enter to continue..."); Console.ReadLine();
            }

            if (opt <= 0) return false;

            if (opt > 0 && opt <= SpawnManager.GetMonsterCount())
            {
                AttackType? type = GameManager.SelectedCharacter.EquippedWeapon?.AttackType;
                AttackStat newAtkStat = InBattle_CalculateAtkStat(GameManager.SelectedCharacter);

                switch (type)
                {
                    case AttackType.Close: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Close, newAtkStat.Attack, false); break;
                    case AttackType.Long: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Long, newAtkStat.RangeAttack, false); break;
                    case AttackType.Magic: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Magic, newAtkStat.MagicAttack, false); break;
                    default: SpawnManager.spawnedMonsters.ElementAt(opt - 1).OnDamage(AttackType.Close, newAtkStat.Attack, false); break;
                }
            }
            return true;
        }

        /// <summary>
        /// Skill Mechanism of player
        /// </summary>
        private bool InBattle_Skill()
        {
            int skillOpt;
            while (true)
            {
                Console.Clear();
                UIManager.ShowSkillList(GameManager.SelectedCharacter);
                if (!int.TryParse(Console.ReadLine(), out int ind)) Console.WriteLine("| 잘못된 입력입니다! |");
                else if (ind < 0 || ind > GameManager.SelectedCharacter.Skills.Count) Console.WriteLine("| 잘못된 입력입니다! |");
                else { skillOpt = Math.Clamp(ind, 0, GameManager.SelectedCharacter.Skills.Count); break; }
                Console.Write("\nPress enter to continue..."); Console.ReadLine();
            }

            if (skillOpt <= 0) return false;

            var skill = GameManager.SelectedCharacter.Skills[skillOpt - 1];
            if (skill is ActiveSkill attackSkill)
            {
                if (GameManager.SelectedCharacter.EquippedWeapon == null)
                {
                    Console.WriteLine("\n| 무기를 장착해야 액티브 스킬을 사용할 수 있습니다! |");
                    Console.Write("\nPress enter to continue..."); Console.ReadLine();
                    return false;
                }

                if (!attackSkill.IsTargetable)
                {
                    bool isSuccess = attackSkill.OnActive(GameManager.SelectedCharacter, SpawnManager.spawnedMonsters);
                    if (!isSuccess)
                    {
                        Console.WriteLine("| 마나가 부족합니다! |");
                        Console.Write("\nPress enter to continue..."); Console.ReadLine();
                    }
                    return isSuccess;
                }

                UIManager.ShowMonsterList();
                int monsterOpt;
                while (true)
                {
                    if (!int.TryParse(Console.ReadLine(), out int ind)) { Console.WriteLine("| 잘못된 입력입니다! |"); }
                    else if (ind < 0 || ind > SpawnManager.GetMonsterCount()) { Console.WriteLine("| 잘못된 입력입니다! |"); }
                    else { monsterOpt = Math.Clamp(ind, 0, SpawnManager.GetMonsterCount()); break; }
                    Console.Write("\nPress enter to continue..."); Console.ReadLine();
                }
                if (monsterOpt <= 0) return false;

                bool isSuccess2 = attackSkill.OnActive(GameManager.SelectedCharacter, SpawnManager.spawnedMonsters.ElementAt(monsterOpt - 1));
                if (!isSuccess2)
                {
                    Console.WriteLine("| 마나가 부족합니다! |");
                    Console.Write("\nPress enter to continue..."); Console.ReadLine();
                }
                return isSuccess2;
            }
            else if (skill is BuffSkill buffSkill) return buffSkill.OnActive(GameManager.SelectedCharacter);
            return false;
        }

        /// <summary>
        /// Calculates AttackStat of character in battle.
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        private AttackStat InBattle_CalculateAtkStat(Character character)
        {
            AttackStat newAtkStat = new(character.AttackStat);
            if (character.EquippedWeapon != null) { newAtkStat += character.AttackStat; }
            foreach (var item in GameManager.Exposables)
            {
                if (item is AttackBuffPotion atkPotion) { newAtkStat += atkPotion.AttackStat; }
                else if (item is AllBuffPotion allPotion) { newAtkStat += allPotion.AttackStat; }
            }
            foreach (var skill in character.Skills)
            {
                if (skill is BuffSkill buffSkill)
                {
                    if (buffSkill.Name.Equals("명상") && buffSkill.IsActive) newAtkStat *= buffSkill.Coefficient;
                }
            }
            return newAtkStat;
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
            if (GameManager.KilledMonsterCount >= GameManager.Quota) GameManager.GoToNextLevel();
            Console.Write("\nPress enter to continue..."); Console.ReadLine();

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
                    case GameState.GameOver: GameManager.GameOverAction(); break;
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
            InGame inGame = new();

            // Main Game
            inGame.MainGame();
        }
    }
}