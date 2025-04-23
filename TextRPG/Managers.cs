using System.Text;
using System.Text.Json;

namespace TextRPG
{
    /// <summary>
    /// Contains UI Materials
    /// </summary>
    static class UIManager
    {
        public static void StartUI()
        {
            foreach (string line in Miscs.GameStart) Console.WriteLine(line);
            Console.ReadKey();
        }

        public static void JobSelectionUI()
        {
            Console.WriteLine();
            foreach (string line in Miscs.CharacterSelection) Console.WriteLine(line);
            Console.WriteLine("\n| 직업 선택 (뒤로가려면 0을 누르세요) |");
            Console.Write("원하는 직업을 선택하세요 : ");
        }

        public static void InventoryUI(Character character)
        {
            foreach (string line in Miscs.Inventory) Console.WriteLine(line);
            Console.WriteLine("| ----- 인벤토리 ----- |");
            Console.WriteLine("| \"방어구\" |");
            int i = 1;
            foreach (Armor armor in character.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            Console.WriteLine("\n| \"무기\" |");
            i = 1;
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            Console.WriteLine("\n| \"포션\" |");
            i = 1;
            foreach (Consumables potion in character.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("\n| \"잡동사니\"|");
            i = 1;
            foreach (ImportantItem item in character.ImportantItems) { Console.WriteLine($"{i++}. {item.ToString()}"); }
            Console.WriteLine("| ------------- |");
            Console.WriteLine("\n| 1. 뒤로가기 |");
            Console.WriteLine("| 2. 아이템 선택 |");
            Console.Write("원하는 기능을 선택하세요 : ");
        }

        public static void InventoryUI_Equipment()
        {
            Console.WriteLine("\n| ----- \"장비\" ----- |");
            Console.WriteLine("| 1. 뒤로가기 |");
            Console.WriteLine("| 2. 착용하기 |");
            Console.WriteLine("| 3. 해제하기 |");
            Console.WriteLine("| 4. 버리기 |");
            Console.Write("원하는 기능을 선택하세요 : ");
        }

        public static void InventoryUI_Consumable()
        {
            Console.WriteLine("\n| ----- \"소모품\" ----- |");
            Console.WriteLine("| 1. 뒤로가기 |");
            Console.WriteLine("| 2. 사용하기  |");
            Console.Write("원하는 기능을 선택하세요 : ");
        }

        public static void InventoryUI_Misc()
        {
            Console.WriteLine("\n| ----- \"잡동사니\" ----- |");
            Console.WriteLine("| 1. 뒤로가기 |");
            Console.WriteLine("| 2. 버리기  |");
            Console.Write("원하는 기능을 선택하세요 : ");
        }

        public static void ShopUI(Character character)
        {
            Console.WriteLine("| ----- Welcome to Henry's Shop! ----- |");
            foreach (string line in Miscs.Henry) Console.WriteLine(line);
            Console.WriteLine("| ---------------------------------- |");

            Console.WriteLine($"\n| Gold : {character.Currency} |");
            Console.WriteLine("\n| 0. 뒤로가기 |");
            Console.WriteLine("| 1. 방어구 구매 |");
            Console.WriteLine("| 2. 무기 구매 |");
            Console.WriteLine("| 3. 포션 구매 |");
            Console.WriteLine("| 4. 아이템 판매 |");
            Console.Write("\n원하는 기능을 선택하세요 : ");
        }

        public static void ShowShopList(ItemCategory category)
        {
            Console.WriteLine("\n| ----- 아이템 구매 ----- |");
            switch (category)
            {
                case ItemCategory.Armor:
                    foreach (string line in Miscs.ArmorDesign) Console.WriteLine(line);
                    Console.WriteLine("| \"방어구\" |");
                    int i = 1;
                    foreach (Armor armor in ItemLists.Armors) { Console.WriteLine($"{i++}. {armor}"); }
                    break;
                case ItemCategory.Weapon:
                    foreach (string line in Miscs.WeaponDesign) Console.WriteLine(line);
                    Console.WriteLine("| \"무기\" |");
                    i = 1;
                    foreach (Weapon weapon in ItemLists.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
                    break;
                case ItemCategory.Consumable:
                    foreach (string line in Miscs.PotionDesign) Console.WriteLine(line);
                    Console.WriteLine("| \"포션\" |");
                    i = 1;
                    foreach (Consumables potion in ItemLists.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
                    break;
            }
            Console.Write("\n구매할 상품 번호 입력 (취소하려면 0을 입력하세요) : ");
        }

        public static void ShowItemList(Character character)
        {
            Console.WriteLine("\n| ----- 아이템 판매 ----- |");
            Console.WriteLine("| 1. \"방어구\" |");
            int i = 1;
            foreach (Armor armor in character.Armors) { Console.WriteLine($"{i++}. {armor}"); }
            Console.WriteLine("| 2. \"무기\" |");
            i = 1;
            foreach (Weapon weapon in character.Weapons) { Console.WriteLine($"{i++}. {weapon}"); }
            Console.WriteLine("| 3. \"포션\" |");
            i = 1;
            foreach (Consumables potion in character.Consumables) { Console.WriteLine($"{i++}. {potion}"); }
            Console.WriteLine("| 4. \"잡동사니\" |");
            i = 1;
            foreach (ImportantItem item in character.ImportantItems) { Console.WriteLine($"{i++}. {item.ToString()}"); }
            Console.WriteLine("| ---------------------- |");
            Console.Write("\n무엇을 판매하겠습니까? ( Type [ Category,Index ], 취소하려면 exit을 입력하세요) : ");
        }

        public static void ShowSkillList(Character character)
        {
            int i = 1;
            Console.WriteLine("\n| \"Skills\" |");
            foreach (Skill skill in character.Skills) Console.WriteLine($"| {i++}. {skill.ToString()} |");

            Console.Write("\n원하는 스킬을 고르세요 (취소하려면 0을 입력하세요) : ");
        }

        public static void ShowMonsterList(SpawnManager spawnManager)
        {
            Console.WriteLine("\n| ----- Battle ----- |");
            Console.WriteLine("| \"몬스터\" |");
            int i = 1;
            foreach (Monster monster in spawnManager.spawnedMonsters)
                Console.WriteLine($"| {i++}. {monster.Name} | HP : {monster.Health} |");
            Console.WriteLine("| ------------------ |");
            Console.Write("\n공격할 몬스터를 선택하세요 (취소하려면 0을 입력하세요) : ");
        }

        public static void CabinUI()
        {
            Console.WriteLine("| ----- Welcome to Alby's Cabin! ----- |");
            foreach (string line in Miscs.Alby) Console.WriteLine(line);
            Console.WriteLine("\n| 1. 뒤로가기 |");
            Console.WriteLine("| 2. 스탠다드 룸 (최대 체력 25% 회복, 40G) |");
            Console.WriteLine("| 3. 디럭스 룸 (최대 체력 50% 회복, 60G) |");
            Console.WriteLine("| 4. 스위트 룸 (최대 체력 75% 회복, 80G)");
            Console.WriteLine("| ------------------------------------ |");
            Console.Write("\n룸 옵션을 선택하세요 : ");
        }

        public static void QuestUI()
        {
            Console.WriteLine("| ----- Welcome to Adventurers' Guild ----- |");
            Console.WriteLine("\n| 1. 뒤로가기 |");
            Console.WriteLine("| 2. Quest 수주 |");
            Console.WriteLine("| 3. Quest 완료 |");
            Console.WriteLine("| 4. 수주 가능한 Quest 목록 |");
            Console.WriteLine("| 5. 수주한 Quest 목록 |");
            Console.WriteLine("| 6. 완료한 Quest 목록 |");
            Console.WriteLine("| ------------------------------------------ |");
            Console.Write("\n원하는 기능을 선택하세요 : ");
        }

        public static void QuestUI_Contract()
        {
            var questList = QuestManager.GetContractableQuests();
            Console.WriteLine("\n| ----- 수주 가능한 Quest 목록 ----- |");
            if (questList == null) { Console.WriteLine("| 수주 가능한 Quest가 없습니다! |"); return; }

            foreach (var quest in questList)
                if (quest is KillMonsterQuest) Console.WriteLine($"{((KillMonsterQuest)quest).ToString()}");
                else if (quest is CollectItemQuest) Console.WriteLine($"{((CollectItemQuest)quest).ToString()}");
            Console.Write("\nQuest를 선택하세요 : ");
        }

        public static void QuestUI_Complete()
        {
            var questList = QuestManager.GetCompletableQuests();
            Console.WriteLine("\n| ----- 완료 가능한 Quest 목록 ----- |");
            if (questList == null) { Console.WriteLine("| 완료 가능한 Quest가 없습니다! |"); return; }

            foreach (var quest in questList)
                if (quest is KillMonsterQuest) Console.WriteLine($"{((KillMonsterQuest)quest).ToString()}");
                else if (quest is CollectItemQuest) Console.WriteLine($"{((CollectItemQuest)quest).ToString()}");
            Console.Write("\nQuest를 선택하세요: ");
        }

        public static void StatusUI(Character character)
        {
            if (character.GetType().Equals(typeof(Warrior)))
            {
                foreach (string line in Miscs.WarriorDesign) Console.WriteLine(line);
            }
            else if (character.GetType().Equals(typeof(Archer)))
            {
                foreach (string line in Miscs.ArcherDesign) Console.WriteLine(line);
            }
            else if (character.GetType().Equals(typeof(Wizard)))
            {
                foreach (string line in Miscs.MazeDesign) Console.WriteLine(line);
            }
            Console.WriteLine("| ----- \"캐릭터 정보\" ----- |");
            Console.WriteLine($"\n| \"Name\" : {character.Name} |");
            Console.WriteLine($"| \"Lv {character.Level:D2}\" |");
            Console.WriteLine($"| \"Exp\" : {character.Exp:F2} |");
            Console.WriteLine($"| \"HP\" : {character.Health:F2}/{character.MaxHealth} |");
            Console.WriteLine($"| \"MP\" : {character.MagicPoint:F2}/{character.MaxMagicPoint} |");
            Console.WriteLine($"| \"Gold\" : {character.Currency} |");

            Console.WriteLine("\n| ----- \"캐릭터 상세\" ----- |");
            Console.WriteLine($"| \"Atk.\" : {character.AttackStat.Attack:F2} |");
            Console.WriteLine($"| \"Range Atk.\" : {character.AttackStat.RangeAttack:F2} |");
            Console.WriteLine($"| \"Magic Atk.\" : {character.AttackStat.MagicAttack:F2} |");
            Console.WriteLine($"| \"Def.\" : {character.DefendStat.Defend:F2} |");
            Console.WriteLine($"| \"Range Def.\" : {character.DefendStat.RangeDefend:F2} |");
            Console.WriteLine($"| \"Magic Def.\" : {character.DefendStat.MagicDefend:F2} |");

            Console.WriteLine("\n| ----- \"장착한 방어구\" ----- |");
            foreach (Armor armor in character.Armors) { if (armor.IsEquipped) Console.WriteLine($"| {armor} |"); }
            Console.WriteLine("| ----- \"장착한 무기\" ----- |");
            foreach (Weapon weapon in character.Weapons) { if (weapon.IsEquipped) Console.WriteLine($"| {weapon} |"); }
            Console.WriteLine("| ----- \"포션\" ----- |");
            foreach (Consumables consumable in character.Consumables) { Console.WriteLine($"| {consumable} |"); }
            Console.WriteLine("| ----- \"잡동사니\" ----- |");
            foreach (ImportantItem item in character.ImportantItems) { Console.WriteLine($"| {item} |"); }
            Console.WriteLine("\n| Press any key to continue... |");
            Console.ReadKey();
        }

        public static void KillCountUI(int KillCount, int Quota)
        {
            Console.WriteLine("\n| --------------------------------------------- |");
            Console.WriteLine($"| 사냥한 몬스터 : {KillCount}, 목표량 : {Quota} |");
            Console.WriteLine("| ----------------------------------------------- |");
        }

        public static void MonsterEncounterUI(SpawnManager spawnManager)
        {
            StringBuilder sb = new();
            int i = 0;

            Console.WriteLine("\n| ---------------------------------- |");
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (Monster monster in spawnManager.spawnedMonsters)
            {
                if (i != 0) sb.Append(", ");
                sb.Append(monster.Name); i++;

                if (monster.AttackType == AttackType.Close) foreach (string line in Miscs.Goblin) Console.WriteLine(line);
                else if (monster.AttackType == AttackType.Long) foreach (string line in Miscs.GoblinArcher) Console.WriteLine(line);
                else foreach (string line in Miscs.GoblinWizard) Console.WriteLine(line);
            }
            Console.ResetColor();
            Console.WriteLine("| ---------------------------------- |");

            Console.WriteLine($"\n| Warning! : 포악한 {sb} 을 조우했다! |");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void NoMonsterFoundUI()
        {
            int ind = new Random().Next(Miscs.Quotes.Length);
            Console.WriteLine($"\n| 아무 일도 없었다..., {Miscs.Quotes[ind]}");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        public static void GameOverUI(Character character)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (string line in Miscs.GameOver) Console.WriteLine(line);
            Console.ResetColor();

            Console.WriteLine($"\nGold : {character.Currency}");
            if (character.Currency >= 100)
            {
                Console.WriteLine("100G를 지불하면 부활할 수 있습니다...");
                Console.Write("부활하겠습니까? (Y/N) : ");
            }
            else
            {
                Console.WriteLine("부활할 수 없습니다...");
                Console.WriteLine("메인 화면으로 돌아갑니다...");
            }
        }

        public static void GameOptionUI()
        {
            Console.WriteLine($"\n| ----- Game Options ----- |");
            Console.WriteLine();
            int i = 1;
            foreach (var opt in Enum.GetValues(typeof(GameOption)))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.Write("\n원하는 기능을 선택하세요 : ");
        }

        public static void SettingOptionUI()
        {
            Console.WriteLine($"\n| ----- 옵션 ----- |");
            int i = 1;
            foreach (var opt in Enum.GetValues(typeof(SettingOptions)))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.Write("\n원하는 기능을 선택하세요 : ");
        }

        public static void DungeonUI(Character character, GameManager gameManager, int[] pathOptions)
        {
            Console.WriteLine($"\n| ----- 던전 Lv{gameManager.GroundLevel} ----- |");
            Console.WriteLine($"| 현재 시간 : {GameManager.GameTime} |");
            Console.WriteLine($"| HP : {character.Health} | MP : {character.MagicPoint} |");
            Console.WriteLine($"| Gold : {character.Currency} |");
            Console.WriteLine();
            for (int i = 1; i <= pathOptions.Length; i++)
            {
                Console.WriteLine($"| {i}. {(DungeonOptions)pathOptions[i - 1]} |");
            }
            for (int i = 4, j = 0; i < Enum.GetValues(typeof(DungeonOptions)).Length; i++, j++)
            {
                Console.WriteLine($"| {pathOptions.Length + j + 1}. {(DungeonOptions)(4 + j)} |");
            }
            Console.Write("\n원하는 기능을 선택하세요 : ");
        }

        public static void BaseUI(Character character, string headLine, Type type)
        {
            Console.WriteLine($"\n| ----- {headLine} ----- |");
            Console.WriteLine($"| 현재 시간 : {GameManager.GameTime} |");
            Console.WriteLine($"| HP : {character.Health} | MP : {character.MagicPoint} |");
            Console.WriteLine($"| Gold : {character.Currency} |");
            Console.WriteLine();
            int i = 1;
            foreach (var opt in Enum.GetValues(type))
            {
                Console.WriteLine($"| {i++}. {opt} |");
            }
            Console.Write("\n원하는 기능을 선택하세요 : ");
        }
    }

    /// <summary>
    /// Contains Quotes and Ascii Arts
    /// </summary>
    static class Miscs
    {
        public static string[] Quotes = { "It's cold and silent.", "My body starts shivering", "I miss the alby's cabin.",
                                          "I should not come down here.", "It feels like someone is watching...",
                                          "I think i'm lost...", "It's dark and moist...", "This place is freaking me out...",
                                          "I hate bats...", "Something feels wrong...", "I miss the henry's shop."};

        public static string[] HardEntrance = {
            "88888888888888888888888888888888888888888888888888888888888888888888888",
            "88.._|      | `-.  | `.  -_-_ _-_  _-  _- -_ -  .'|   |.'|     |  _..88",
            "88   `-.._  |    |`!  |`.  -_ -__ -_ _- _-_-  .'  |.;'   |   _.!-'|  88",
            "88      | `-!._  |  `;!  ;. _______________ ,'| .-' |   _!.i'     |  88",
            "88..__  |     |`-!._ | `.| |_______________||.\"'|  _!.;'   |     _|..88",
            "88   |``\"..__ |    |`\";.| i|_|MMMMMMMMMMM|_|'| _!-|   |   _|..-|'    88",
            "88   |      |``--..|_ | `;!|l|MMoMMMMoMMM|l|.'j   |_..!-'|     |     88",
            "88   |      |    |   |`-,!_|_|MMMMP'YMMMM|_||.!-;'  |    |     |     88",
            "88___|______|____!.,.!,.!,!|0|MMMo * loMM|d|,!,.!.,.!..__|_____|_____88",
            "88      |     |    |  |  | |_|MMMMb,dMMMM|_|| |   |   |    |      |  88",
            "88      |     |    |..!-;'i|9|MPYMoMMMMoM|u| |`-..|   |    |      |  88",
            "88      |    _!.-j'  | _!,\"|_|M<>MMMMoMMM|_||!._|  `i-!.._ |      |  88",
            "88     _!.-'|    | _.\"|  !;|l|MbdMMoMMMMM|n|`.| `-._|    |``-.._  |  88",
            "88..-i'     |  _.''|  !-| !|_|MMMoMMMMoMM|_|.|`-. | ``._ |     |``\"..88",
            "88   |      |.|    |.|  !| |l|MoMMMMoMMMM|l||`. |`!   | `\".    |     88",
            "88   |  _.-'  |  .'  |.' |/|_|MMMMoMMMMoM|_|! |`!  `,.|    |-._|     88",
            "88  _!\"'|     !.'|  .'| .'|[@]MMMMMMMMMMM[@] \\|  `. | `._  |   `-._  88",
            "88-'    |   .'   |.|  |/| /                 \\|`.  |`!    |.|      |`-88",
            "88      |_.'|   .' | .' |/                   \\  \\ |  `.  | `._    |  88",
            "88     .'   | .'   |/|  /                     \\ |`!   |`.|    `.  |  88",
            "88  _.'     !'|   .' | /                       \\|  `  |  `.    |`.|  88",
            "88 ---Hard Stage-- 888888888888888888888888888888888888888888888(FL)888"
        };

        public static string[] EasyEntrance = {
            "|____________________________________________|",
            "|__||  ||___||  |_|___|___|__|  ||___||  ||__|",
            "||__|  |__|__|  |___|___|___||  |__|__|  |__||",
            "|__||  ||___||  |_|___|___|__|  ||___||  ||__|",
            "||__|  |__|__|  |    || |    |  |__|__|  |__||",
            "|__||  ||___||  |    || |    |  ||___||  ||__|",
            "||__|  |__|__|  |    || |    |  |__|__|  |__||",
            "|__||  ||___||  | 9조|| | Dun|  ||___||  ||__|",
            "||__|  |__|__|  |    || |    |  |__|__|  |__||",
            "|__||  ||___||  |   O|| |O   |  ||___||  ||__|",
            "||__|  |__|__|  |    || |    |  |__|__|  |__||",
            "|__||  ||___||  |    || |    |  ||___||  ||__|",
            "||__|  |__|__|__|____||_|____|  |__|__|  |__||",
            "|LLL|  |LLLLL|______________||  |LLLLL|  |LLL|",
            "|LLL|  |LLL|______________|  |  |LLLLL|  |LLL|",
            "|LLL|__|L|______________|____|__|LLLLL|__|LLL|"
        };

        public static string[] Town = {
            "                                                           |>>>",
            "                   _                      _                |",
            "    ____________ .' '.    _____/----/-\\ .' './========\\   / \\",
            "   //// ////// /V_.-._\\  |.-.-.|===| _ |-----| u    u |  /___\\",
            "  // /// // ///==\\ u |.  || | ||===||||| |T| |   ||   | .| u |_ _ _ _ _ _",
            " ///////-\\////====\\==|:::::::::::::::::::::::::::::::::::|u u| U U U U U",
            " |----/\\u |--|++++|..|'''''''''''::::::::::::::''''''''''|+++|+-+-+-+-+-+",
            " |u u|u | |u ||||||..|              '::::::::'           |===|>=== _ _ ==",
            " |===|  |u|==|++++|==|              .::::::::.           | T |....| V |..",
            " |u u|u | |u ||HH||         \\|/    .::::::::::.",
            " |===|_.|u|_.|+HH+|_              .::::::::::::.              _",
            "                __(_)___         .::::::::::::::.         ___(_)__",
            "---------------/  / \\  /|       .:::::;;;:::;;:::.       |\\  / \\  \\-------",
            "______________/_______/ |      .::::::;;:::::;;:::.      | \\_______\\________",
            "|       |     [===  =] /|     .:::::;;;::::::;;;:::.     |\\ [==  = ]   |",
            "|_______|_____[ = == ]/ |    .:::::;;;:::::::;;;::::.    | \\[ ===  ]___|____",
            "     |       |[  === ] /|   .:::::;;;::::::::;;;:::::.   |\\ [=  ===] |",
            "_____|_______|[== = =]/ |  .:::::;;;::::::::::;;;:::::.  | \\[ ==  =]_|______",
            " |       |    [ == = ] /| .::::::;;:::::::::::;;;::::::. |\\ [== == ]      |",
            "_|_______|____[=  == ]/ |.::::::;;:::::::::::::;;;::::::.| \\[  === ]______|_",
            "   |       |  [ === =] /.::::::;;::::::::::::::;;;:::::::.\\ [===  =]   |",
            "___|_______|__[ == ==]/.::::::;;;:::::::::::::::;;;:::::::.\\[=  == ]___|_____"
        };

        public static string[] Rest1 = {
            "       _____",
            "      /      \\",
            "     (____/\\  )",
            "      |___  U?(____",
            "      _\\L.   |      \\     ___",
            "    / /\"\"\"\\ /.-'     |   |\\  |",
            "   ( /  _/u     |    \\___|_)_|",
            "    \\|  \\\\      /   / \\_(___ __)",
            "     |   \\\\    /   /  |  |    |",
            "     |    )  _/   /   )  |    |",
            "     _\\__/.-'    /___(   |    |    Contemplation or Constipation ?",
            "  _/  __________/     \\  |    |",
            " //  /  (              ) |    |",
            "( \\__|___\\    \\______ /__|____|",
            " \\    (___\\   |______)_/",
            "  \\   |\\   \\  \\     /",
            "   \\  | \\__ )  )___/",
            "    \\  \\  )/  /__(       ",
            "___ |  /_//___|   \\_________",
            "  _/  ( / OUuuu    \\",
            " `----'(____________)"
        };

        public static string[] Rest2 = {
            "                           ||||||",
            "                           | o o |",
            "                           |  >  |",
            "                           | \\_/ |",
            "                            \\___/",
            "                           __| |__",
            "                          /       \\",
            "                         | |     | |",
            "        _________________| |     | |_____________---__",
            "       /                 | |_____| |         /  /  / /|",
            "      /                  /_|  _  |_\\        /  /  / / |",
            "     /                    / / / /          /  /__/ / /|",
            "    /____________________/ / / /__________/___\\_/_/ / |",
            "    |____________________| |_| |__________________|/  |",
            "    |____________________| |_| |__________________|   /",
            "____|              |     | | | | ||               |  /",
            "    | o          o | o         o || o           o | /",
            "    |______________|_____________||_______________|/",
            "_______________________________________________________"
        };
        public static string[] Rest3 = {
            "                  __..-----')",
            "        ,.--._ .-'_..--...-'",
            "       '-\"'. _/_ /  ..--''\"\"'-.",
            "       _.--\"\"...:._:(_ ..:\"::. \\",
            "    .-' ..::--\"\"_(##)#)\"':. \\ \\)    \\ _|_ /",
            "   /_:-:'/  :__(##)##)    ): )   '-./'   '\\.-'",
            "   \"  / |  :' :/\"\"\\///)  /:.'    --(       )--",
            "     / :( :( :(   (#//)  \"       .-'\\.___./'-.",
            "    / :/|\\ :\\_:\\   \\#//\\            /  |  \\",
            "    |:/ | \"\"--':\\   (#//)              '",
            "    \\/  \\ :|  \\ :\\  (#//)",
            "         \\:\\   '.':. \\#//\\",
            "          ':|    \"--'(#///)",
            "                     (#///)",
            "                     (#///)         ___/\"\"\\     ",
            "                      \\#///\\           oo##",
            "                      (##///)         `-6 #",
            "                      (##///)          ,'`.",
            "                      (##///)         // `.\\",
            "                      (##///)        ||o   \\\\",
            "                       \\##///\\        \\-+--//",
            "                       (###///)       :_|_(/",
            "                       (sjw////)__...--:: :...__",
            "                       (#/::'''        :: :     \"\"--.._",
            "                  __..-'''           __;: :            \"-._",
            "          __..--\"\"                  `---/ ;                '._",
            " ___..--\"\"                             `-'                    \"-..___",
            "",
            "   (_ \"\"---....___                                     __...--\"\" _)",
            "     \"\"\"--...  ___\"\"\"\"\"-----......._______......----\"\"\"     --\"\"\"",
            "                   \"\"\"\"       ---.....   ___....----"
        };

        public static string[] Quest = {
            "   ______________________________",
            " / \\          -Quest-            \\.",
            "|   |   ____________________     |.",
            " \\_ |   ____________________     |.",
            "    |   ____________________     |.",
            "    |   __________               |.",
            "    |                            |.",
            "    |                            |.",
            "    |                            |.",
            "    |                            |.",
            "    |                            |.",
            "    |                            |.",
            "    |                            |.",
            "    |                            |.",
            "    |   _________________________|___",
            "    |  /                            /.",
            "    \\_/dc__________________________/."
        };

        public static string[] HighLevelGoblinWarrior = {
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@=  +@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@   @@@@@@@@@@%       @@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                  @@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@             #@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@          @@@ @@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@           @  %@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                  @@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                   @@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                    @@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@                     @@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@        @@            @    @@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@       @@@@@         @@@     @@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@      @@@@@@@@        @@@@     @@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@     @ @@@@@@@=          @@@:   :@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@+ %@@@@@@@@@              @@@    @@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@  :   @ @@@@ @                :@@   @@",
            "@@@@@@@@@@@@@@@@@@@@@@@@*         %@@@@                  @:   @@",
            "@@@@@@@@@@@@@@@@@@@@             @@@@@@    @@@@         @     @@",
            "@@@@@@@@@@@@@@@@              @@@@@@@@@    @@@@@@@@@   @  @@  @@",
            "@@@@@@                       @@@@@@@@@@@   @@@@@@@@   %  @@  @@@",
            "@@@                    @@@@  @@@@@@@@@@@@@  @@@@@@@   @@@@  #@@@",
            "@@@@               @@@@@@@@@@@@@@@@@@@@@@@    @@@@@@  @@@@@@@@@@",
            "@@@@@         %@@@@@@@@@@@@@@@@@@@@@@@@     @@@@@@@    @@@@@@@@@",
            "@@@@@@   =@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@" };

        public static string[] HighLevelGoblinWizard = {
            "@@@@@@@@@@@@@*=@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@         @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@            -@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@%      *%       @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@     @@@@@@@     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@       :@@@@@    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@        .@@@%    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@       -@@    @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@.   :@@   @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@   @@@@@@  @@@@@@@@@@@  @@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@  @@@@@@   @@     @#           @@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@  @@@@@@@   :#                  @@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@  @@@@@@@                      @@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@  @@@@@@@@                    @@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@  @@@@@@-                @@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@ @@@@@@@@@                 @@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@   @@@@@@@.                  @@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@    @@@@@@@                    @@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@     @@@                       @@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@                               @@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@  @@@@@@@                    #@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@  @@@@@@@                     @@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@ @@@@@@         @       .     @@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@  @@@@                 +  =   %@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@  @@@@                @@@: @=  @@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@ @@@@@.    #   @+   @@@@@.@@  @@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@  @@@      @@ @@@    @@@@@@@@ *@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@#  +@@      @@@@@     @@@@@@@@ @@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@  @@@@@=@@@@@@@@@   %@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"
        };
        public static string[] HighLevelGoblinArcher = {
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@*@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@* @@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@# %@@@@@@@@@@@@@@@",
            "@@@@@@@@@@%   +@@@#     =@@@@@@@@@@%+-#@@@@@@@@@@@",
            "@@@@@@@@@@@@%             -@@@@@@@@#@% =@@@@@@@@@@",
            "@@@@@@@@@@@@@@            =%@@@@@@@%@@@ +%@@@@@@@@",
            "@@@@@@@@@@@@@@@@:           +@@@@@@@@@@@@ *%@@@@@@",
            "@@@@@@@@@@@@@@*+.           :@@@@@%@@@@@@ +%@@@@@@",
            "@@@@@@@@@@@@@#.=.         *=@@@@@@%@@@@@@= #@@@@@@",
            "@@@@@@@@@@-@@-           %@@@@@@@@*@@@@@@% *#@@@@@",
            "@@@@@@@@:            .*-%@*=+@@@@@@%@@@@@@= ##@@@@",
            "@@@+       =         *  :+=*@@@@@%#@%@   =*%   =@@",
            "@@+                 :#.     -@:     : +###-  +@@@@",
            "@@+.        *-       +%@            .-=++*=  #@@@@",
            "@@@@@@@@@@@+-**       @@ :     :   --++#@ =*@@@@@@",
            "@@@@@@@@@@@@      :   * @@@@@@@@**@@@@@@# **@@@@@@",
            "@@@@@@@@@@@@%    :     @@@@@@@@@@%@@@@@@ :*#@@@@@@",
            "@@@@@@@@@@@@.         @%*==%@@@@*@@@@@@@ **@@@@@@@",
            "@@@@@@%-@@  = :       : **-#@@@@*@@@@@# +#@@@@@@@@",
            "@@@@@@ :-+=-..        +*:-**@@@@#@@@  +#@@@@@@@@@@",
            "@@@@@# ++*=: -:----      =+@@@@@@@ -*@@@@@@@@@@@@@",
            "@@@@@* **%:  -:.:.:     %@*@@@@@:-*@@@@@@@@@@@@@@@",
            "@@@@@% *@:   :--:+      @@@@@@@.-@@@@@@@@@@@@@@@@@",
            "@@@@@@.*@=    :--#      @*%@@@#-%@@@@@@@@@@@@@@@@@",
            "@@@@@@#=*     .==#      #=%%%%# %%@@@@@@@@@@@@@@@@",
            "@@@@@@@@#     .++*      .%@@@@%=@@@@@@@@@@@@@@@@@@",
            "@@@@@@@+      #@*      :@@@@@@@%%%%@@@@@@@@@@@@@@@",
            "@@@@@@@+      %@%      .#%@@@@%@%@@@@@#@@@@@@@@@@@",
            "@@@@@@@%      %@@       #%@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@%+    %@@%#      #%%%%@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@%     %@@%%@:      %#*@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@%      #%%%%%%.      +##@@@@@@@@@@@@@@@@@@@",
            "@@@@@@%-:    =*#######     .:###+-*##@@@@@@@@@@@@@",
            "@@@@@@@#+-==+###*###+=-.::+***+==+#@%%@%@@@@@@@@@@",
            "@@@@@@#**=**#+*#*#*#+---=+**#*==+*@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@",
            "@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@"
        };
        public static string[] Goblin = new string[]
        {@"                                    ^                     
                                              -+.       
                                            .++*.       
                                    =+#.   .=+#+-       
                      .-+++++:.    :-+=-   +++++:       
    .....          .::..:--::::=-..:=+.   .++=++.       
  .--===---..     .....:--::::::.::.=+    =*+==+.       
   .:+++===-:....=........:-:::..=#===.  .*+++*-        
      .++====-:.::-..=+*%+..::..=*-==+.  -#+=+*.        
        -+====++-::.-+#%#+=-::.:**-==-  .+++=+=.        
         :+=====+=:..:-@@=:.::..-+-=:   :*++=*:         
          .-+**+=::..-=--:...::......  .+++=++.         
             -==-.::...-=-....:=-.:.   .*++-*=.         
                :=-=+=-::..:-::-+=-.   -++==+-          
               .*=----=++***#*=-+*#+.  +++++=.          
              .=+#+++**#++++++=#*+-+-:.:++=-.           
              :**-:::=**+=+*+=-+*=::=:=***+=:.          
             .#*=:::-=+#*-=+++=*#=::=+=+-:-...          
             #+=:..:==+***=---=+**==+*++--:             
            #*+**+-**+**=+***+++******==.               
           .******+**#**+#+:#:*****+*+:-:               
           +*#+==***+##*#*=:+-+*###++*..                
          -*+-::..-=----=**=-----=**++=                 
         .+++:::-=-*++++**#*+===+**++++=.               
          .::=+=--*=.:.-*+**+::::++--=:.                
       ..:::::-=:. :...:.*+.:...-                       
         .....   .:-:..:   .:...-=-:.                   
             ..:*#.#-==+....:-+===+==-.......           
                    ...................                 
                   
"


        };

        public static string[] GoblinArcher = new string[]
        {@"
                                            ..
                           ...::::..   .=+. ..
             -*+*=..     .+:::::::::+..=*: .*-
               =++*+:++.:-::::=-:::-:--=+: .#+.        
                .++++*=:--:+%@#:::::#@#*+... -#.       
                 -++++#%#::=.%@@#=-%@=*++..   :#:      
                  .+++***-:-=#@--==+@===:.     +%.     
                    .:+++=:-*+--:-=-=-=:.      -#:     
                       .**##-:-+==+=+#+        *#.     
                       =##%#*#%%##%%*%#=.    .*%.      
                     .+##*+*%#*#***##*-=....-=%#:.     
                    .+%-=++##*:--#@###*#%%%%#*#%#-.    
                    +%=::+**:::=+%##*@@##*#+=+-=.      
                  .=#@@%%%%%##=###+%#@@+.  .*%=.       
                 .*#%%%@%%##%%####*#%#%%:   -#.        
                 =*%%%%%%##**#**%#***#%%#. .+#.        
                 .=:.=*-*+=+#%%#%%##+*#-*..+#-         
                .=+==+::-+--::...-::+... =#:.          
                  .......--:-.  .=:--:..#-.            
                  ....:***#+=::..--=+#++#:...  
                         .  ...   .    .     "
        };

        public static string[] GoblinWizard = new string[]
        { @"         

            ...:--:..    :+=. .-==:..   
             .*==+=..    .=::::::::::-.:-+. .=:.::=.   
               .*==*::+..-:::::-:-=-::-:*=. ---:::-:   
                .-=++++::-:-=#@-:-::+%+#+=.  ##++*..   
                 .=++++%#-:=.%@%+-:+@*=*+-   .*##*.    
                   :=+++*-::-==:=-====:=:    .*#.      
                     .-===::-*---:::-+:-.    .**.      
                       .#**#*-:::=-:=%*=.    .#*.      
                       =####+**#%%%#**##.    :#-       
                      .**#*####*****###=+..:-++=.      
                     .##*::*#***%@%***+-:+#=**==.      
                    .*@:::%#**%#%#**##@*:*#*+=:-.      
                   .#@##*#@%%%##=##*#%%%*...*%.        
                  .+#@=-=*#****#*#*#**%%#. .#+.        
                  **%*=-:=*****#*##****@#+..*=         
                ...****=-#%+*#%%##%##+%%#*:.#.         
                .-===+---=+----==--:--.....+#.         
                  ........--:=...-=--=:....#+          
                   ....:#**++=...---=*#+-..=:..      
 "
        };

        public static string[] Henry = new string[]
        {
            @"        /     \        ",
            @"       / (/@\) \       ",
            @"   \__/_________\__/   ",
            @"     |  O     O  |     ",
            @"     |     ^     |     ",
            @"     |   \___/   |     ",
            @"      \_________/      ",
            @"   ___/   |||   \___   ",
            @" /`    \       /    `\ ",
            @" \__.   |     |   .__/ ",
            @"     \  |     |  /     ",
            @"      \_|_____|_/      ",
        };

        public static string[] Alby = new string[]
        {
            @"        /     \        ",
            @"       / -(+)- \       ",
            @"    __/_________\__    ",
            @"   / |  O     O  | \   ",
            @"     |     ^     |     ",
            @"     |   '---'   |     ",
            @"      \_________/      ",
            @"   ___/   |||   \___   ",
            @" /`    \       /    `\ ",
            @" \__.   |     |   .__/ ",
            @"     \  |     |  /     ",
            @"      \_|_____|_/      ",
        };

        public static string[] GameStart = new string[]
        {@"
+=====================================================================================+ 
|                           `            .              .               .             | 
|           *                                                                *        | 
|     `         ` . **   `            .     |>>>     .                 *              | 
|   *     `      `                          |                    ...            .     | 
|                                       _  _|_  _     *                      *        | 
|       ..            `       |>>>     |*| |*| |*|         |>>>       .               | 
|              `              |        \         /         |          .               | 
|        `  *             _  _|_  _     \       /      _   |_  _                      | 
|      *                 |;|_|;|_|;|    ||     |      |;| |;|_|;|                     | 
|  .       .             \         /    ||     |      \         /                     | 
|               `         \    `  /     ||     |       \  ` `  /            *         | 
|         `               ||:`    |_   _ |_   _|  _   _ |     |                       | 
|                         ||:     ||| |}||}|_|}|_|}| |}||: .  |          .            | 
|                         ||:  .  ||       `           ||:    |                       | 
|                         ||:     ||:  `         `     ||:  ` |                       | 
|                         ||: .   ||       __**__      ||: .  |                       | 
|                         ||:     ||  `   /:: ': \     ||:    |                       | 
|                         ||:     || :   ||::::: |   ` ||:    |                       | 
|                         ||:     ||     ||':::::|     ||:    |                       | 
|                    ____ ||:     ||  `  ||::::: |    _||_    |                       | 
|               -+H+       -+H+-  ||     ||:  :::___         ___                      | 
|          ____     -+      _____    ____   ____  _____   ______                      | 
|     -+H+-             ___|\    \  |    | |    ||\    \ |\     \  ++-_____  _        | 
| __                   |    |\    \ |    | |    | \\    \| \     \               ____ | 
|                      |    | |    ||    | |    |  \|    \  \     |          -+H+_-+H+| 
|                      |    | |    ||    | |    |   |     \  |    |       -+          | 
|                      |    | |    ||    | |    |   |      \ |    |                   | 
|                      |    | |    ||    | |    |   |    |\ \|    |                   | 
|                      |____|/____/||\___\_|____|   |____||\_____/|                   | 
|                      |    /    | || |    |    |   |    |/ \|   ||                   | 
|                      |____|____|/  \|____|____|   |____|   |___|/                   | 
|                        \(    )/       \(   )/       \(       )/                     | 
|                         '    '         '   '         '       '                      | 
|                                                                                     | 
|                                 PRESS ANY KEY TO START!                             | 
|                                                                         CREATED BY. | 
|                                                                                     | 
|                                                                          PARK_DOUN  | 
|                                                                         CHO_YUNJAE  | 
|                                                                        PARK_JIHWAN  | 
|                                                                        KIM_KONGSIK  | 
|                                                                      BANG_EUNSEONG  | 
|                                                                                     | 
+=====================================================================================+
      "  };

        public static string[] CharacterSelection = new string[]
        {
            @"╔═══════════════════════════════════════════════════════════════╗",
            @"║                                                               ║",
            @"║    [1. WARRIOR]          [2. WIZARD ]          [3. ARCHER]    ║",
            @"║         /\                  '(**),                            ║",
            @"║        /__\                   ||                 |\           ║",
            @"║        |  |                   ||                 | '\         ║",
            @"║        |  |                   ||                 |   |        ║",
            @"║        |  |                   ||               >-------=>     ║",
            @"║        |__|                   ||                 |   |        ║",
            @"║         ||                    ||                 | ,/         ║",
            @"║        .||.                   ()                 |/           ║",
            @"║                                                               ║",
            @"║                                                               ║",
            @"║                        Select your Job!                       ║",
            @"╚═══════════════════════════════════════════════════════════════╝",
        };

        public static string[] GameOver = new string[]
        {
            @"| ======================================================================== |",
            @"|  ██████   █████  ███    ███ ███████     ██████  ██    ██ ███████ ███████ |",
            @"| ██       ██   ██ ████  ████ ██         ██    ██ ██    ██ ██      ██   ██ |",
            @"| ██   ███ ███████ ██ ████ ██ █████      ██    ██ ██    ██ ███████ █████   |",
            @"| ██    ██ ██   ██ ██  ██  ██ ██         ██    ██ ██    ██ ██      ██   ██ |",
            @"|  ██████  ██   ██ ██      ██ ███████     ██████   ██████  ███████ ██   ██ |",
            @"| ======================================================================== |",
        };

        public static string[] WarriorDesign = new string[]
        {
            "   |\\                     /)",
            " /\\_\\\\__               (_//",
            "|   `>\\-`     _._       //`)",
            " \\ /` \\\\  _.-`:::`-._  //",
            "  `    \\|`    :::    `|/",
            "        |     :::     |",
            "        |.....:::.....|",
            "        |:::::::::::::|",
            "        |     :::     |",
            "        \\     :::     /",
            "         \\    :::    /",
            "          `-. ::: .-'",
            "           //`:::`\\\\",
            "          //   '   \\\\",
            "         |/         \\\\"
        };


        public static string[] PotionDesign = new string[]
{
    "         @@@@@@@@         ",
    "         @@@@@@@@         ",
    "        @@@    @@@        ",
    "         @@@  @@@         ",
    "         @@@  @@@         ",
    "         @@@  @@@         ",
    "        @@@    @@@        ",
    "       @@@  @@@@@@@       ",
    "      @@@@@@@    @@@      ",
    "      @@@         @@      ",
    "     @@@@          @@     ",
    "    @@@@            @@    ",
    "     @@            @@@    ",
    "      @@@@@@@@@@@@@@ "
};


        public static string[] ArcherDesign = new string[]
{
    "#####                              ##",
    "%%%%#                          ##### ",
    "%%%%####################    #######  ",
    "@@%%%%%%%%%%%%%%%%%%%%####   #####   ",
    "@@   %%%%%%%%%%%%%%%%%%%##%### ##    ",
    "@@                 %%%%%%###         ",
    " @@                  %%%#######%     ",
    " @@                  %##%%%%%%###    ",
    " @@                 ###%%%%%%%%##%   ",
    "  @               ###      %%%%%##   ",
    "  @@            ###        %%%%%##   ",
    "  @@          ###           %%%%##   ",
    "  @@        ####            %%%%##   ",
    "  @@   %% ###*              %%%%#%   ",
    "   @  %%%###                %%%%#    ",
    "   @%%%##%%%                %%%##    ",
    "   %%%##%%%                 %%%##    ",
    "   @@%%%%@                   %%######",
    "      %%@@@@@@@@@@@@@@@       %%%%%##",
    "                       @@@@@@@@@%%%%#"
};

        public static string[] MazeDesign = new string[]
{
    "                                   ",
    "                   #########       ",
    "                ##############     ",
    "               #####*******#####   ",
    "              ####***+++++**#####  ",
    "             ####****+**+****####  ",
    "             ####*************###  ",
    "             ####************####  ",
    "              ####**********#####  ",
    "             #######*******####    ",
    "           #########               ",
    "         #########                 ",
    "        #########                  ",
    "      ########                     ",
    "    #########                      ",
    "  #########                        ",
    "  #######                          ",
    "   ####                           "
};
        public static string[] Inventory = new string[]
 {
    "               ##% ###               ",
    "               ##   #%               ",
    "          #################          ",
    "        ##########%##########        ",
    "       #########     #########       ",
    "      ###########% ############      ",
    "      #########################      ",
    "      #########################      ",
    "      #### ############### ####      ",
    "  %## ####                 #### ###  ",
    "  ### #### ############### #### #### ",
    "      #### ############### ####      ",
    "  ### #### ############### #### #### ",
    "  ### ####################%#### #### ",
    "  ### ######################### #### ",
    "  ###                           #### ",
    "  %## ######################### ###  ",
    "      %########################      ",
    "        %###################%        "
 };


        public static string[] ArmorDesign = new string[]
 {
    "==09조==!====!=====!=====!====!===!===!=====!===!===!====",
    "      /`\\__/`\\   /`\\   /`\\  |~| |~|  /)=I=(\\  /`\"\"\"`\\",
    "     |        | |   `\"`   | | | | |  |  :  | |   :   |",
    "     '-|    |-' '-|     |-' )/\\ )/\\  |  T  \\ '-| : |-'",
    "       |    |     |     |  / \\// \\/  (  |\\  |  '---'",
    "       '.__.'     '.___.'  \\_/ \\_/   |  |/  /",
    "                                     |  /  /",
    "                                     |  \\ /",
    "                                     '--'`"
 };
        public static string[] path = {
            @"+==================================================================================+
            |                                 |               ||   ____  ____  ____  ____      |
            |     _.-=-.     ____  ____  ____|            '   ||  /\   \/\   \/\   \/\   \     |
            |               /\   \/\   \/\   \     .           | /  \___\ \___\ \___\ \___\    |
            |              /  \___\ \___\ \___\  ┌──────────┐  | \  /   / /   / /   / /   /    |
            |              \  /   / /   / /   /  │ 1. FRONT │  |  \/___/\/___/\/___/\/___/     |
            |               \/___/\/___/\/___/   └──────────┘  ||                              |
            |                                 |                 |    _.-=-._                   |
            |                                ||                 |                              |
            |                                |         '        |                              |
            |                                |                   |                             |
            |                                ||            .     |                             |
            |                                 |                  ||                            |
            |            _.-=-._.             |     .             |                            |
            |                                  |                  |                            |
            |                           ____  ____                |                            |
            |                          /\   \/\   \               ||               _.-=-.      |
            |                         /  \___\ \___\    '         |                            |
            |                         \  /   / /   /       .       |____  ____                 |
            |                          \/___/\/___/                /\   \/\   \                |
            |                                  |                  /  \___\ \___\               |
            |                                  ||        .        \  /   / /   /               |
            |     _.-=-._                       ||   '             \/___/\/___/                |
            |                                    |          '     |                            |
            |                                   ||                ||                           |
            |                                    |                |                            |
            |                                    |      '        ||                            |
            |                                  |||               |                             |
            |                                  |           '    ||                             |
            +==================================================================================+",
            @"+=========================================|========================================+
            |                                  ||                            _.                |
            |     .               .               ||||                                 /\      |
            |                  ,      ,              ||||                             O  O     |
            |        ,                                  |||                            \/      |
            |                                      .       |||       /\ /\ /\                  |
            |                 ,            ┌─────────┐       ||     O  O  O  O                 |
            |||                            │ 1. LEFT │         ||    \/ \/ \ /\ /\             |
            | |||| |||| ||||           .   └─────────┘          |           O  O  O            |
            |     |          ||||||                         .    |           \/ \/             |
            |                     || ||                           |                            |
            |                          ||||||        ,            ||                           |
            |                               ||||               .   |                           |
            |            _.-=-._.             |     .               |                          |
            |                                  |                  | |                          |
            |      /\                          ||                 ||              _.""._.""._.   |
            |     O /\ /\                      ||                  |                           |
            |      O  O  O                      |       '         |                            |
            |       \/ \/                       |          .       ||     _.""._.""              |
            |                                   |                  |                           |
            |                                  ||                  |                           |
            |      O                /\         ||        .         ||                          |
            |     _ -=-._          O  O         ||   '              |              /\          |
            |                       \/           |          '     |||       /\ /\ O  O         |
            |                                   ||                ||       O  O  O \/          |
            |                                    |                |         \/ \/              |
            |                                    |      '          |                           |
            |                 _..-              ||                |                            |
            |                                  |           '      ||                           |
            +==================================================================================+",
            @"+================================================================================+
            |                                            |||| |||                            |                  
            |              *                         |||||                                   |    
            |                       *             ||||                       right           |
            |   *                               |||                                          | 
            |               *                  ||                                        || ||
            |                                  ||                                |  |||||    | 
            |                                ||                              ||              |
            |                               ||                           |||                 |
            |                              ||                          ||           *        |
            |                             ||                        ||                       |
            |                *           ||                     ||              *            |
            |                           ||                   ||                              |
            |                           |                  ||                                |
            |                          ||                 |                                  |
            |                          |                ||                                   |
            |                         ||               ||                                    |
            |                         |                |                                     |
            |                         |               ||                     .oOo.oOo.o      |
            |.oOo.oOo.oO             ||               |                                      |
            |                        |                ||                                     | 
            |        .oOo.oOo.oOo   ||                ||                                     |
            |                       ||                |                .oOo.oOo.oOo.oOo.     |
            |.oOo.oO                 | |             ||             .oOo.                    | 
            |                        |||             |        .oOo.oOo.oOo.o                 |
            |                        ||              | |                                     |
            +================================================================================+",
            @"+===============================+=================================================+
            |                                                   ||||                          |
            |             ^v                     ^v^        || |                              |
            |                                             |||                                 |
            ||                      ^v^               | |         ┌──────────┐                |
            | ||||                                    ||          │ 2. RIGHT │                |
            |    |||||||||||||||||||||               |            └──────────┘               ||
            |                        ||||||||||||   ||                                  |  || |
            |                                    |||||                              ||| ||    |
            |                                        |                    |    ||  |          |
            |                    ┌─────────┐                             ||| |||              |
            |                    │ 1. LEFT │              .           |||                     |
            |                    └─────────┘                        ||                        |
            ||||    ||||||                         .               |                          |
            |  ||||||    |||||||                                 | |              ^v^v^v^     |
            |                  ||||||                            ||                           |
            |                       ||  |                        ||                           |
            |                        |||||             '         |                            |
            |                               ||||          .       |                           |
            |                               | ||                  |                           |
            |                                ||                   |                           |
            |              ^v^v             ||          .         ||                          |
            |                               |       '              |                          |
            |                               ||             '     |||      ^v                  |
            |                                |                   ||                           |
            |                                |                   |                            |
            |    ^v^v^v^                     |         '        ||                            |
            |                                ||                 ||               ^v^v^        |
            |                                 |           '    | |                            |
            |                                 ||                 ||                           |
            |                                 |                   ||                          |
            |                                |||                   ||                         |
            +=================================================================================+",
            @"+==================================================================================+
            |                     *                  ||                 |                      |
            |         *                             ||                 ||             -+H+     |
            |                                     * |             .    |                       |
            |                            *         ||  ┌──────────┐   |                        |
            ||                   |                 |   │ 1. FRONT │  ||                     ||||
            | |||||      |||||||   |||            ||   └──────────┘ ||             |     |||   |
            |      || |              |||||||| |||| |                |         |||||  |||||     |
            |                                                      ||||||||||||                |
            |       *       *                                                         :        |
            |             *┌─────────┐                                  ┌──────────┐           |
            |              │ 2. LEFT │                                  │ 3. RIGHT │        ,  |
            ||             └─────────┘                                  └──────────┘        ||||
            | ||||||||||||||                                                         |  ||||   |
            |             |||||||||||                                            ||            |
            |        *              |||||||                             .    |||               |
            |                        ,    ||||                           = ||        -+H+-+H   |
            |                                |                          ||                     |
            |                    *           |                      ||     -+             .    |
            |                                |                   ||       .                    |
            |                 *             |                  ||                              |
            |     *                         |    .            |                                |
            |                              |                ||                                 |
            |                    *        ||               ||                                  |
            |                             |                |                                   |
            |                             |               ||                    -+H+-+         |
            |                            ||               |                                    |
            |         *                  |             .  ||                                   |
            |                           ||                ||                                   |
            |                   *       ||                |         -+H+-+H+-                  |
            |                            | |             ||                                    |
            |                            |||                                                   |
            +==================================================================================+",
        };
        public static string[] WarriorDesign = new string[]
{
    "           ,",
    "          / \\",
    "         {   }",
    "         p   !",
    "         ; : ;",
    "         | : |",
    "         | : |",
    "         l ; l",
    "         l ; l",
    "         I ; I",
    "         I ; I",
    "         I ; I",
    "         I ; I",
    "         d | b\t",
    "         H | H",
    "         H | H",
    "         H I H",
    " ,;,     H I H     ,;,",
    ";H@H;    ;_H_;,   ;H@H;",
    "`\\Y/d_,;|4H@HK|;,_b\\Y/'",
    " '\\;MMMMM$@@@$MMMMM;/'",
    "   \"~~~*;!8@8!;*~~~\"",
    "         ;888;",
    "         ;888;",
    "         ;888;",
    "         ;888;",
    "         d8@8b",
    "         O8@8O",
    "         T808T",
    "          `~` \t"
};
    }

    /// <summary>
    /// Manage quests
    /// </summary>
    class QuestManager
    {
        // Methods
        /// <summary>
        /// Get Contracted Quests of collecting item
        /// </summary>
        /// <param name="itemName"></param>
        /// <returns></returns>
        public static IEnumerable<CollectItemQuest> GetContractedQuests_CollectItem(string itemName)
        {
            var contracted = from quest in Quests
                             where quest.IsContracted == true && quest.IsCompleted == false && quest is CollectItemQuest
                             where ((CollectItemQuest)quest).ItemName.Equals(itemName)
                             select (CollectItemQuest)quest;
            return contracted;
        }

        /// <summary>
        /// Get Contracted Quests of killing monster
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<KillMonsterQuest> GetContractedQuests_KillMonster()
        {
            var contracted = from quest in Quests
                             where quest.IsContracted == true && quest.IsCompleted == false && quest.QuestType == QuestType.KillMonster
                             select (KillMonsterQuest)quest;
            return contracted;
        }

        /// <summary>
        /// Get Contracted Quests
        /// </summary>
        /// <returns> IEnumerable Array of Contracted Quests </returns>
        public static IEnumerable<Quest> GetContractedQuests()
        {
            var contracted = from quest in Quests
                             where quest.IsContracted == true && quest.IsCompleted == false
                             select quest;
            return contracted;
        }

        /// <summary>
        /// Get Contractable Quests
        /// </summary>
        /// <returns> IEnumerable Array of Contractable Quests </returns>
        public static IEnumerable<Quest> GetContractableQuests()
        {
            var contractables = from quest in Quests
                                where quest.IsContracted == false && quest.IsCompleted == false
                                select quest;
            return contractables;
        }

        /// <summary>
        /// Get Completed Quests
        /// </summary>
        /// <returns> IEnumerable Array of Completed Quests </returns>
        public static IEnumerable<Quest> GetCompletedQuests()
        {
            var completed = from quest in Quests
                            where quest.IsCompleted == true && quest.IsContracted == false
                            select quest;
            return completed;
        }

        /// <summary>
        /// Get Completable Quests
        /// </summary>
        /// <returns> IEnumerable Array of Completable Quests </returns>
        public static IEnumerable<Quest> GetCompletableQuests()
        {
            var completables = from quest in Quests
                               where quest.IsCompleted == true && quest.IsContracted == true
                               select quest;
            return completables;
        }

        /// <summary>
        /// Get all quests
        /// </summary>
        /// <returns> IEnumerable Array of Quests </returns>
        public static IEnumerable<Quest> GetQuests() { return Quests; }

        public static void SetQuests(Quest[] quests)
        {
            Quests = quests;
        }

        /// <summary>
        /// Quest List
        /// </summary>
        private static Quest[] Quests =
        {
            new KillMonsterQuest("Please save us from monsters' attack", "Kill 1 Goblins", QuestDifficulty.Normal, 1, 120,300),
            new CollectItemQuest("Please bring me some goblin's ears", nameof(GoblinEar), "Collect 1 Goblin's Ears", QuestDifficulty.Easy, 1, 100,300),
        };
    }

    /// <summary>
    /// Manage spawning monsters
    /// </summary>
    class SpawnManager
    {
        // Property
        public LinkedList<Monster> spawnedMonsters = new();

        // Constructor
        public SpawnManager() { }

        // Public Methods
        /// <summary>
        /// Spawn monsters
        /// </summary>
        /// <param name="character"></param>
        /// <param name="groundLevel"></param>
        public void SpawnMonsters(Character character, int groundLevel)
        {
            int count = new Random().Next(1, 5);
            for (int i = 0; i < count; i++)
            {
                int type = new Random().Next(MonsterLists.monsters.Length);

                if (MonsterLists.monsters[type].AttackType == AttackType.Close)
                {
                    GoblinWarrior monster = new GoblinWarrior((GoblinWarrior)MonsterLists.monsters[type]);
                    SetMonster(monster, character, groundLevel, 50);
                    AddMonster(monster);
                }
                else if (MonsterLists.monsters[type].AttackType == AttackType.Long)
                {
                    GoblinArcher monster = new GoblinArcher((GoblinArcher)MonsterLists.monsters[type]);
                    SetMonster(monster, character, groundLevel, 65);
                    AddMonster(monster);
                }
                else if (MonsterLists.monsters[type].AttackType == AttackType.Magic)
                {
                    GoblinMage monster = new GoblinMage((GoblinMage)MonsterLists.monsters[type]);
                    SetMonster(monster, character, groundLevel, 80);
                    AddMonster(monster);
                }
            }
        }

        /// <summary>
        /// Get all spawned monsters
        /// </summary>
        /// <returns>Returns spawned monster count</returns>
        public int GetMonsterCount() { return spawnedMonsters.Count; }

        /// <summary>
        /// Remove all spawned monsters
        /// </summary>
        public void RemoveAllMonsters() { spawnedMonsters.Clear(); }

        // Private Methods
        /// <summary>
        /// Set monster's level and stats
        /// </summary>
        /// <param name="monster"></param>
        /// <param name="character"></param>
        /// <param name="groundLevel"></param>
        /// <param name="currency"></param>
        private void SetMonster(Monster monster, Character character, int groundLevel, int currency)
        {
            monster.Level = groundLevel;
            monster.AttackStat += new AttackStat(monster.AttackStat.Attack * 0.2f * (monster.Level - 1),
                                                 monster.AttackStat.RangeAttack * 0.2f * (monster.Level - 1),
                                                 monster.AttackStat.MagicAttack * 0.2f * (monster.Level - 1));
            monster.DefendStat += new DefendStat(monster.DefendStat.Defend * 0.17f * (monster.Level - 1),
                                                 monster.DefendStat.RangeDefend * 0.17f * (monster.Level - 1),
                                                 monster.DefendStat.MagicDefend * 0.17f * (monster.Level - 1));
            monster.OnDeath += () =>
            {
                RemoveMonster(character, monster, currency);
                var quests = QuestManager.GetContractedQuests_KillMonster();
                foreach (var quest in quests) { quest.OnProgress(); }
            };
        }

        /// <summary>
        /// Add monster to the linked list
        /// </summary>
        /// <param name="monster"></param>
        private void AddMonster(Monster monster) { spawnedMonsters.AddLast(monster); }

        /// <summary>
        /// Remove monster from the linked list
        /// </summary>
        /// <param name="character"></param>
        /// <param name="monster"></param>
        /// <param name="currency"></param>
        private void RemoveMonster(Character character, Monster monster, int currency)
        {
            Console.WriteLine($"| {monster.Name} is dead! |");
            Console.WriteLine($"| {character.Name} gets {currency}G |");
            GameManager.KilledMonsterCount++;
            character.Currency += currency;
            character.OnEarnExp(monster.Exp);

            // Randomly drop items
            int ind = new Random().Next(0, 4);
            if (ind == 0) GetRandomArmor(monster.Level)?.OnPicked(character);
            else if (ind <= 1) GetRandomWeapon(monster.Level)?.OnPicked(character);
            else if (ind <= 2) GetRandomConsumable(monster.Level)?.OnPicked(character);
            else GetRandomImportantItem()?.OnPicked(character);
            spawnedMonsters.Remove(monster);
        }

        // Return random items
        private Armor? GetRandomArmor(int level)
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;

            IEnumerable<Armor> filteredItems;
            if (level > 0 && level <= 15)
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Common || item.Rarity == Rarity.Exclusive
                                select item;
            }
            else if (level > 15 && level <= 30)
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare
                                select item;
            }
            else if (level > 30 && level <= 50)
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero
                                select item;
            }
            else
            {
                filteredItems = from item in ItemLists.Armors
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero || item.Rarity == Rarity.Legend
                                select item;
            }
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
        private Weapon? GetRandomWeapon(int level)
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;
            IEnumerable<Weapon> filteredItems;
            if (level > 0 && level <= 15)
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Common || item.Rarity == Rarity.Exclusive
                                select item;
            }
            else if (level > 15 && level <= 30)
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare
                                select item;
            }
            else if (level > 30 && level <= 50)
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero
                                select item;
            }
            else
            {
                filteredItems = from item in ItemLists.Weapons
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero || item.Rarity == Rarity.Legend
                                select item;
            }
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
        private Consumables? GetRandomConsumable(int level)
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;

            IEnumerable<Consumables> filteredItems;
            if (level > 0 && level <= 15)
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Common || item.Rarity == Rarity.Exclusive
                                select item;
            }
            else if (level > 15 && level <= 30)
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare
                                select item;
            }
            else if (level > 30 && level <= 50)
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero
                                select item;
            }
            else
            {
                filteredItems = from item in ItemLists.Consumables
                                where item.Rarity == Rarity.Exclusive || item.Rarity == Rarity.Rare || item.Rarity == Rarity.Hero || item.Rarity == Rarity.Legend
                                select item;
            }
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
        private ImportantItem? GetRandomImportantItem()
        {
            if (new Random().Next(1, 101) % 2 != 0) return null;

            var filteredItems = from item in ItemLists.ImportantItems
                                where item.Rarity == Rarity.Common
                                select item;
            int ind = new Random().Next(filteredItems.Count());
            return filteredItems.ElementAt(ind);
        }
    }

    /// <summary>
    /// Manage controls of overall game statements and functions
    /// </summary>
    class GameManager
    {
        // Static Field
        public static GameState GameState = GameState.MainMenu;
        public static GameTime GameTime = GameTime.Afternoon;
        public static int KilledMonsterCount = 0;
        public static int CurrentTurn = 1;
        public static int prevPath = 0;
        public static Queue<Consumables> Exposables = new();

        // Property
        public Character SelectedCharacter { get; private set; }
        public int GroundLevel { get; private set; }
        public int Quota { get; private set; } = 10;

        // Constructor
        public GameManager(int groundLevel = 1) { GroundLevel = groundLevel; }

        // Methods
        #region Game Mechanisms
        /// <summary>
        /// Job Selection UI will be displayed.
        /// This method will return true if the job selected successfully.
        /// If not, it will return false.
        /// </summary>
        /// <returns>Returns true, if job selected successfully. If not, returns false.</returns>
        public void SelectJob()
        {
            int option;
            while (true)
            {
                UIManager.JobSelectionUI();
                if (!int.TryParse(Console.ReadLine(), out int opt)) { Console.WriteLine("| Invalid Input! |"); }
                else { option = Math.Clamp(opt, 0, Enum.GetValues(typeof(Job)).Length); break; }
            }

            if (option <= 0) return;

            if (option <= 0) return;

            switch ((Job)(option - 1))
            {
                case Job.Warrior:
                    Console.WriteLine("| 전사를 선택하였습니다! |");
                    Console.Write("전사의 이름을 작성해주세요 : ");
                    SelectedCharacter = new Warrior(new CharacterStat(Console.ReadLine(), 150, 50, 15, 1.6f, 1, new AttackStat(30f, 6f, 1f), new DefendStat(25, 15, 5)), 250, 0);
                    break;
                case Job.Wizard:
                    Console.WriteLine("| 법사를 선택하였습니다! |");
                    Console.Write("법사의 이름을 작성해주세요 : ");
                    SelectedCharacter = new Wizard(new CharacterStat(Console.ReadLine(), 100, 80, 15, 1.6f, 1, new AttackStat(1f, 6f, 30f), new DefendStat(5, 10, 30)), 250, 0);
                    break;
                case Job.Archer:
                    Console.WriteLine("| 궁수를 선택하였습니다! |");
                    Console.Write("궁수의 이름을 작성해주세요 : ");
                    SelectedCharacter = new Archer(new CharacterStat(Console.ReadLine(), 120, 65, 15, 1.6f, 1, new AttackStat(6f, 30f, 1f), new DefendStat(15, 25, 5)), 250, 0);
                    break;
            }

            GiveBasicItems(SelectedCharacter);
            GiveBasicSkills(SelectedCharacter);
            SelectedCharacter.OnDeath += GameOver;

            GameState = GameState.Town;
        }

        /// <summary>
        /// Give basic items to the character.
        /// </summary>
        /// <param name="character"></param>
        private void GiveBasicItems(Character character)
        {
            // LINQ
            var basicHelmets = from armor in ItemLists.Armors
                               where armor is Helmet && armor.Rarity == Rarity.Common
                               select (Helmet)armor;
            var basicChestArmors = from armor in ItemLists.Armors
                                   where armor is ChestArmor && armor.Rarity == Rarity.Common
                                   select (ChestArmor)armor;

            var basicHealthPotions = from item in ItemLists.Consumables
                                     where item is HealthPotion && item.Rarity == Rarity.Common
                                     select (HealthPotion)item;
            var basicMagicPotions = from item in ItemLists.Consumables
                                    where item is MagicPotion && item.Rarity == Rarity.Common
                                    select (MagicPotion)item;

            if (basicHelmets.Count() > 0) { character.Armors.Add(new Helmet(basicHelmets.First())); }
            if (basicChestArmors.Count() > 0) { character.Armors.Add(new ChestArmor(basicChestArmors.First())); }
            if (basicHealthPotions.Count() > 0) { character.Consumables.Add(new HealthPotion(basicHealthPotions.First())); }
            if (basicMagicPotions.Count() > 0) { character.Consumables.Add(new MagicPotion(basicMagicPotions.First())); }

            if (character is Warrior)
            {
                var basicSwords = from sword in ItemLists.Weapons
                                  where sword is Sword && sword.Rarity == Rarity.Common
                                  select (Sword)sword;
                if (basicSwords.Count() > 0) { character.Weapons.Add(new Sword(basicSwords.First())); }
            }
            else if (character is Wizard)
            {
                var basicStaffs = from staff in ItemLists.Weapons
                                  where staff is Staff && staff.Rarity == Rarity.Common
                                  select (Staff)staff;
                if (basicStaffs.Count() > 0) { character.Weapons.Add(new Staff(basicStaffs.First())); }
            }
            else
            {
                var basicBows = from bow in ItemLists.Weapons
                                where bow is Bow && bow.Rarity == Rarity.Common
                                select (Bow)bow;
                if (basicBows.Count() > 0) { character.Weapons.Add(new Bow(basicBows.First())); }
            }
        }

        /// <summary>
        /// Give basic skills to the character.
        /// </summary>
        /// <param name="character"></param>
        private void GiveBasicSkills(Character character)
        {
            // Active Skills
            if (character is Warrior)
                character.Skills.Add(new ActiveSkill((ActiveSkill)SkillLists.ActiveSkills[0]));
            else if (character is Archer)
                character.Skills.Add(new ActiveSkill((ActiveSkill)SkillLists.ActiveSkills[1]));
            else character.Skills.Add(new ActiveSkill((ActiveSkill)SkillLists.ActiveSkills[2]));

            // Buff Skills
            character.Skills.Add(new BuffSkill((BuffSkill)SkillLists.BuffSkills[0]));
        }

        /// <summary>
        /// Game Over UI will be displayed.
        /// </summary>
        private void GameOver()
        {
            GameState = GameState.GameOver;
            UIManager.GameOverUI(SelectedCharacter);

            // Low currency -> Reset game and move to main menu
            if (SelectedCharacter.Currency < 100) { ResetGame(); return; }

            // Enough currency -> Give player option to revive
            char key = char.ToLower(Console.ReadKey(true).KeyChar);
            if (key.Equals('n')) { ResetGame(); return; }

            // If player choose to revive, revive the character and move to town
            SelectedCharacter.OnRevive();
            GameState = GameState.Town;
            GameTime = GameTime.Afternoon;
        }

        /// <summary>
        /// Reset the game to initial state.
        /// </summary>
        private void ResetGame()
        {
            GameState = GameState.MainMenu;
            GameTime = GameTime.Afternoon;

            Exposables.Clear();
            GroundLevel = 1;
            Quota = 10;
        }

        /// <summary>
        /// Remove all buffs applied to character when night passed.
        /// </summary>
        public void RemoveAllBuffs()
        {
            if (Exposables.Count <= 0) return;

            while (Exposables.Count > 0)
            {
                Consumables consumable = Exposables.Dequeue();
                if (consumable == null) continue;

                consumable.OnDeBuffed(SelectedCharacter);
            }
            Console.WriteLine("| 하루가 지나 모든 버프가 해제되었습니다... |");
        }

        /// <summary>
        /// Increase Dungeon level when character reached the quota.
        /// </summary>
        public void GoToNextLevel()
        {
            Console.WriteLine("| 목표량을 달성하였습니다, 던전 레벨과 목표량이 올라갑니다! |");
            Console.WriteLine("| Press any key to continue... |");
            Console.ReadKey();
            KilledMonsterCount = 0;
            Quota = 10 + (++GroundLevel - 1) * 5;
        }
        #endregion

        #region Save & Load Functions
        /// <summary>
        /// Save the character and game data to JSON files.
        /// </summary>
        public void SaveGame()
        {
            if (!Directory.Exists("data")) Directory.CreateDirectory("data");

            // Saving Character Data
            var characterOptions = new JsonSerializerOptions
            {
                Converters = {
                    new CharacterConverter(), new ArmorConverter(),
                    new WeaponConverter(), new ConsumableConverter(),
                    new ImportantItemConverter(), new SkillConverter(),
                },
                WriteIndented = true
            };

            string characterJson = JsonSerializer.Serialize(SelectedCharacter, characterOptions);
            File.WriteAllText("data/character.json", characterJson, new UTF8Encoding(true));

            // Saving Game Data
            var gameData = new GameData
            {
                GroundLevel = GroundLevel,
                Quota = Quota,
                KilledMonsterCount = KilledMonsterCount,
                GameState = GameState,
                GameTime = GameTime,
                Exposables = Exposables.ToList()
            };

            var gameOptions = new JsonSerializerOptions
            {
                Converters = {
                    new ConsumableConverter(),
                },
                WriteIndented = true
            };
            string gameJson = JsonSerializer.Serialize(gameData, gameOptions);
            File.WriteAllText("data/game.json", gameJson, new UTF8Encoding(true));

            //Saving Quest Data
            var questOptions = new JsonSerializerOptions
            {
                Converters = {
                     new QuestConverter(),
                },
                WriteIndented = true
            };

            string questJson = JsonSerializer.Serialize(QuestManager.GetQuests(), questOptions);
            File.WriteAllText("data/quest.json", questJson, new UTF8Encoding(true));

            Console.WriteLine("| 게임이 성공적으로 저장되었습니다! |");
        }

        /// <summary>
        /// Load the character and game data from JSON files.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        public void LoadGame()
        {
            if (!File.Exists("data/character.json") || !File.Exists("data/game.json") || !File.Exists("data/quest.json"))
            {
                Console.WriteLine("| 저장된 세이브 데이터가 없습니다! |");
                Console.Write("| Press any key to continue... |");
                Console.ReadKey();
                return;
            }

            // Loading Character Data
            var options = new JsonSerializerOptions
            {
                Converters = {
                    new CharacterConverter(), new ArmorConverter(),
                    new WeaponConverter(), new ConsumableConverter(),
                    new ImportantItemConverter(), new SkillConverter(),
                },
                WriteIndented = true
            };
            string characterJson = File.ReadAllText("data/character.json", Encoding.UTF8);
            var obj = JsonSerializer.Deserialize<Character>(characterJson, options);
            SelectedCharacter = obj ?? throw new InvalidOperationException("Failed to load character data.");

            // Loading Game Data
            var gameOptions = new JsonSerializerOptions
            {
                Converters = {
                    new ConsumableConverter(),
                },
                WriteIndented = true
            };
            string gameJson = File.ReadAllText("data/game.json", Encoding.UTF8);
            var gameData = JsonSerializer.Deserialize<GameData>(gameJson, gameOptions) ?? throw new InvalidOperationException("Failed to load game data.");
            SetGameData(gameData);

            // Loading Quest Data
            var questOptions = new JsonSerializerOptions
            {
                Converters = {
                    new QuestConverter(),
                },
                WriteIndented = true
            };
            string questJson = File.ReadAllText("data/quest.json", Encoding.UTF8);
            var quests = JsonSerializer.Deserialize<Quest[]>(questJson, questOptions) ?? throw new InvalidOperationException("Failed to load quest data.");
            QuestManager.SetQuests(quests);

            Console.WriteLine("| 게임을 성공적으로 불러왔습니다! |");
        }

        /// <summary>
        /// Set GameManager data from GameData.
        /// </summary>
        /// <param name="gameData"></param>
        private void SetGameData(GameData gameData)
        {
            GroundLevel = gameData.GroundLevel;
            Quota = gameData.Quota;
            KilledMonsterCount = gameData.KilledMonsterCount;
            GameState = gameData.GameState;
            GameTime = gameData.GameTime;
            Exposables = new Queue<Consumables>(gameData.Exposables);
        }
        #endregion
    }
}
