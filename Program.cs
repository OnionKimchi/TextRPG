using System.Text.Json;

namespace TextRPG
{
    internal class Program
    {
        static int baseAttack = 10;
        static int baseDefense = 5;
        static int playerGold = 1500;
        static int playerLevel = 1;
        static int hp = 100;
        static Random random = new Random();

        static int GetTotalAttack()
        {
            return baseAttack + GetEquippedStat("ATK") + ((playerLevel - 1) / 2); //총 공격력 = 기본 공격력 + 장착한 아이템의 공격력 + 레벨에 따른 추가 공격력
        }

        static int GetTotalDefense()
        {
            return baseDefense + GetEquippedStat("DEF") + (playerLevel - 1); //총 방어력 = 기본 방어력 + 장착한 아이템의 방어력 + 레벨에 따른 추가 방어력
        }
        static void Main(string[] args)
        {
            ShowMainMenu();
        }

        public class Item
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int Price { get; set; }
            public int ATKStat { get; set; }
            public int DEFStat { get; set; }
            public bool IsOwned { get; set; }
            public bool IsEquipped { get; set; }

            public bool IsPurchased { get; set; }

            public ItemCategory Category { get; set; }

            public string Stat
            {

                get
                {

                    if (ATKStat > 0 && DEFStat == 0)
                    {
                        return $"공격력 +{ATKStat}";
                    }
                    else if (DEFStat > 0 && ATKStat == 0)
                    {
                        return $"방어력 +{DEFStat}";
                    }
                    else if (ATKStat > 0 && DEFStat > 0)
                    {
                        return $"공격력 +{ATKStat} | 방어력 +{DEFStat}";
                    }
                    else
                    {
                        return "스탯 없음";
                    }
                }
            }
        }
        static int GetEquippedStat(string statType)
        {
            int totalStat = 0;

            foreach (var item in inventory)
            {
                if (item.IsEquipped)
                {
                    if (statType == "ATK")
                    {
                        totalStat += item.ATKStat;
                    }
                    else if (statType == "DEF")
                    {
                        totalStat += item.DEFStat;
                    }
                }
            }

            return totalStat;
        }

        public enum ItemCategory
        {
            Weapon, // 무기
            Armor   // 방어구
        }

        static Item[] inventory = new Item[]
        {
        new Item { Name = "무쇠갑옷", ATKStat = 0, DEFStat = 5, Description = "무쇠로 만들어져 튼튼한 갑옷입니다.", IsOwned = true, IsEquipped = true, IsPurchased = true, Price = 1000, Category = ItemCategory.Armor },
        new Item { Name = "스파르타의 창", ATKStat = 7,DEFStat = 0, Description = "스파르타의 전사들이 사용했다는 전설의 창입니다.", IsOwned = true, IsEquipped = true, IsPurchased = true, Price = 3500, Category = ItemCategory.Weapon },
        new Item { Name = "낡은 검", ATKStat = 2,DEFStat = 0, Description = "쉽게 볼 수 있는 낡은 검 입니다.", IsOwned = true, IsEquipped = false , IsPurchased = true, Price = 600, Category = ItemCategory.Weapon },
        new Item { Name = "청동 도끼", ATKStat = 5, DEFStat = 0, Description = "어디선가 사용됐던거 같은 도끼입니다.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 1500, Category = ItemCategory.Weapon },
        new Item { Name = "스파르타의 갑옷", ATKStat = 0, DEFStat = 15, Description = "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 3500, Category = ItemCategory.Armor },
        new Item { Name = "수련자 갑옷", ATKStat = 0, DEFStat = 5, Description = "수련에 도움을 주는 갑옷입니다.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 1000, Category = ItemCategory.Armor },
        new Item { Name = "최강의 검", ATKStat = 99, DEFStat = 99, Description = "비싸지만 최강의 검입니다.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 9999, Category = ItemCategory.Weapon },
        new Item { Name = "쓸모없는 방패", ATKStat = 0, DEFStat = 0, Description = "비싸지만 쓸모없는 장식용 방패입니다.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 9999, Category = ItemCategory.Armor },
        };

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
                Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("1. 상태 보기");
                Console.WriteLine("2. 인벤토리");
                Console.WriteLine("3. 상점");
                Console.WriteLine("4. 던전 입장");
                Console.WriteLine("5. 휴식하기");
                Console.WriteLine("6. 세이브하기");
                Console.WriteLine("7. 로드하기");
                Console.WriteLine("0. 종료하기");
                Console.WriteLine();
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        ShowStatus();
                        break;
                    case "2":
                        ShowInventory();
                        break;
                    case "3":
                        ShowShop();
                        break;
                    case "4":
                        EnterDungeon();
                        break;
                    case "5":
                        Rest();
                        break;
                    case "6":
                        SaveData saveData = new SaveData
                        {
                            PlayerGold = playerGold,
                            PlayerLevel = playerLevel,
                            HP = hp,
                            Inventory = inventory
                        };
                        SaveManager.SaveGame(saveData);
                        break;
                    case "7":
                        SaveData loadedData = SaveManager.LoadGame();
                        if (loadedData != null)
                        {
                            playerGold = loadedData.PlayerGold;
                            playerLevel = loadedData.PlayerLevel;
                            hp = loadedData.HP;
                            inventory = loadedData.Inventory;
                        }
                        break;
                    case "0":
                        Console.WriteLine("게임을 종료합니다.");
                        return; // 프로그램 종료
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                        break;
                }
            }
        }

        static void ShowStatus()
        {
            int totalAttack = GetTotalAttack();
            int totalDefense = GetTotalDefense();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("상태 보기");
                Console.WriteLine("캐릭터의 정보가 표시됩니다.");
                Console.WriteLine();
                Console.WriteLine($"Lv. {playerLevel}");
                Console.WriteLine("Chad ( 전사 )");
                Console.WriteLine($"공격력 : {totalAttack} (+{GetEquippedStat("ATK")})");
                Console.WriteLine($"방어력 : {totalDefense} (+{GetEquippedStat("DEF")})");
                Console.WriteLine($"체 력 : {hp} / 100");
                Console.WriteLine($"Gold : {playerGold} G");
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                string input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine(" 메인 메뉴로 돌아갑니다.");
                    return;//메인 메뉴로 돌아가기
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리");
                Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (!item.IsOwned) continue; // 보유하지 않은 아이템은 표시하지 않음
                    string equippedMark = item.IsEquipped ? "[E]" : ""; // 장착 여부에 따라 [E] 표시
                    Console.WriteLine($"- {equippedMark}{item.Name} | {item.Stat} | {item.Description}");
                }
                Console.WriteLine("1. 장착 관리");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return;
                }
                else if (input == "1")
                {
                    ManageEquipment();
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
            }
        }

        static void ManageEquipment()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("인벤토리 - 장착 관리");
                Console.WriteLine("보유 중인 아이템을 장착 또는 해제할 수 있습니다.");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (!item.IsOwned) continue; // 보유하지 않은 아이템은 표시하지 않음
                    string equippedMark = item.IsEquipped ? "[E]" : ""; // 장착 여부에 따라 [E] 표시
                    Console.WriteLine($"{i + 1}. {equippedMark}{item.Name} | {item.Stat} | {item.Description}");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("장착/해제할 아이템 번호를 입력해주세요.\r\n>> ");
                string Input = Console.ReadLine();

                if (Input == "0")
                {
                    return; // 인벤토리로 돌아가기                  
                }
                else if (int.TryParse(Input, out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Length)
                {
                    var selectedItem = inventory[itemIndex - 1];
                    if (selectedItem.IsOwned)
                    {
                        // 무기와 방어구 중 하나만 장착 가능하도록 조건 추가
                        if (selectedItem.IsEquipped)
                        {
                            // 이미 장착된 아이템은 해제
                            selectedItem.IsEquipped = false;
                        }
                        else
                        {
                            // 같은 분류의 다른 아이템이 장착되어 있는지 확인
                            foreach (var item in inventory)
                            {
                                if (item.IsEquipped && item.Category == selectedItem.Category)
                                {
                                    item.IsEquipped = false; // 기존 장착 해제
                                }
                            }
                            selectedItem.IsEquipped = true; // 선택한 아이템 장착
                        }
                    }
                    else
                    {
                        Console.WriteLine("보유하지 않은 아이템입니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                    }
                }
            }
        }

        static void ShowShop()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("상점");
                Console.WriteLine("필요한 아이템을 얻거나 판매할 수 있는 상점입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{playerGold} G");
                Console.WriteLine();
                Console.WriteLine("1. 구매창");
                Console.WriteLine("2. 판매창");
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // 상점 종료
                }
                else if (input == "1")
                {
                    ShowPurchaseMenu(); // 구매창으로 이동
                }
                else if (input == "2")
                {
                    ShowSellMenu(); // 판매창으로 이동
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowPurchaseMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("구매창");
                Console.WriteLine("구매 가능한 아이템 목록입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{playerGold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (item.IsPurchased) continue; // 이미 구매한 아이템은 표시하지 않음
                    Console.WriteLine($"{i + 1}. {item.Name} | {item.Stat} | {item.Description} | {item.Price} G");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("구매할 아이템 번호를 입력해주세요.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // 구매창 종료
                }
                else if (int.TryParse(input, out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Length)
                {
                    var selectedItem = inventory[itemIndex - 1];
                    if (selectedItem.IsPurchased)
                    {
                        Console.WriteLine("이미 구매한 아이템입니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                    }
                    else if (playerGold < selectedItem.Price)
                    {
                        Console.WriteLine("Gold가 부족합니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                    }
                    else
                    {
                        selectedItem.IsOwned = true;
                        selectedItem.IsEquipped = false;
                        selectedItem.IsPurchased = true;
                        playerGold -= selectedItem.Price;
                        Console.WriteLine($"{selectedItem.Name}을(를) 구매했습니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowSellMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("판매창");
                Console.WriteLine("판매 가능한 아이템 목록입니다.");
                Console.WriteLine();
                Console.WriteLine("[보유 골드]");
                Console.WriteLine($"{playerGold} G");
                Console.WriteLine();
                Console.WriteLine("[아이템 목록]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (!item.IsOwned) continue; // 보유하지 않은 아이템은 표시하지 않음
                    Console.WriteLine($"{i + 1}. {item.Name} | {item.Stat} | {item.Description} | 판매 가격: {(item.Price * 0.85):F0} G");
                }
                Console.WriteLine();
                Console.WriteLine("0. 나가기");
                Console.WriteLine();
                Console.Write("판매할 아이템 번호를 입력해주세요.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // 판매창 종료
                }
                else if (int.TryParse(input, out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Length)
                {
                    var selectedItem = inventory[itemIndex - 1];
                    if (!selectedItem.IsOwned)
                    {
                        Console.WriteLine("보유하지 않은 아이템입니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                    }
                    else
                    {
                        selectedItem.IsOwned = false;
                        selectedItem.IsEquipped = false;
                        selectedItem.IsPurchased = false;
                        playerGold += (int)(selectedItem.Price * 0.85); // 판매 가격은 구매 가격의 85%
                        Console.WriteLine($"{selectedItem.Name}을(를) 판매했습니다.");
                        Console.WriteLine("아무 키나 눌러주세요.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
            }
        }
        static void EnterDungeon()
        {
            int totalAttack = GetTotalAttack();
            int totalDefense = GetTotalDefense();

            Console.Clear();
            Console.WriteLine("던전입장");
            Console.WriteLine("던전 입장");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 쉬운 던전 | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전 | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전 | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                return; // 메인 메뉴로 돌아가기
            }
            else if (input == "1")
            {
                EasyDungeon();
            }
            else if (input == "2")
            {
                NormalDungeon();
            }
            else if (input == "3")
            {
                HardDungeon();
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
            }

        }

        static void Rest()
        {
            Console.Clear();
            Console.WriteLine("휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {playerGold} G)");
            Console.WriteLine("1. 체력 회복하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\r\n>> ");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return; // 메인 메뉴로 돌아가기
            }
            else if (input == "1" && hp >= 100)
            {
                Console.WriteLine("체력이 이미 만땅입니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
            }
            else if (input == "1" && hp < 100)
            {
                if (playerGold < 500)
                {
                    Console.WriteLine("Gold가 부족합니다.");
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
                else
                {
                    playerGold -= 500;
                    Console.WriteLine("체력을 회복했습니다.");
                    hp = 100;
                    Console.WriteLine("아무 키나 눌러주세요.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
            }

        }
        static void EasyDungeon()
        {
            int baseReward = 1000; // 기본 보상
            int bonusReward = random.Next(GetTotalAttack(), GetTotalAttack() * 2) / 100; // 보너스 보상
            int damage = random.Next(20, 35) + 5; // 던전 권장 방어력 만큼 데미지 증가
            int totalDamage = damage - GetTotalDefense(); // 총 데미지
            if (totalDamage <= 1) // 총 데미지가 1보다 작으면 1로 설정
            {
                totalDamage = 1;
            }


            Console.Clear();
            if (hp <= 0)
            {
                Console.WriteLine("체력이 없어서 던전 입장에 실패했습니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("쉬운 던전");
            Console.WriteLine("쉬운 던전으로 입장합니다.");
            if (GetTotalDefense() < 5 && random.Next(0, 100) < 40)
            {

                Console.WriteLine("방어력이 부족하여 40% 확률로 던전 클리어에 실패했습니다.");
                Console.WriteLine("체력 절반 감소 ");
                hp = hp / 2;
                Console.WriteLine($"현재 체력 : {hp}");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("던전 클리어에 성공했습니다.");//1000 + (공격력% ~ (공격력 * 2)%) 만큼 추가 보상 획득 가능
            Console.WriteLine($"보상으로 {baseReward + bonusReward} G를 획득했습니다.");
            Console.WriteLine($"기본 보상 {baseReward} G, 추가 보상 {bonusReward} G");
            playerGold += baseReward + bonusReward;
            Console.WriteLine($"현재 보유 골드 : {playerGold} G");
            Console.WriteLine($"체력이 {totalDamage} 만큼 감소했습니다.");
            if (totalDamage < hp)
            {
                hp -= totalDamage;
                Console.WriteLine($"현재 체력 : {hp}");
            }
            else if (totalDamage >= hp)
            {
                Console.WriteLine("체력이 0이 되었습니다.");
                hp = 0;
            }
            Console.WriteLine("던전에서 퇴장합니다.");
            Console.ReadKey();
            return;
        }
        static void NormalDungeon()
        {
            int baseReward = 1700; // 기본 보상
            int bonusReward = random.Next(GetTotalAttack(), GetTotalAttack() * 2) / 100; // 보너스 보상
            int damage = random.Next(20, 35) + 10; // 던전 권장 방어력 만큼 데미지 증가
            int totalDamage = damage - GetTotalDefense(); // 총 데미지
            if (totalDamage <= 1) // 총 데미지가 1보다 작으면 1로 설정
            {
                totalDamage = 1;
            }

            Console.Clear();
            if (hp <= 0)
            {
                Console.WriteLine("체력이 없어서 던전 입장에 실패했습니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("일반 던전");
            Console.WriteLine("일반 던전으로 입장합니다.");
            if (GetTotalDefense() < 11 && random.Next(0, 100) < 40)
            {
                Console.WriteLine("방어력이 부족하여 40% 확률로 던전 클리어에 실패했습니다.");
                Console.WriteLine("체력 절반 감소 ");
                hp = hp / 2;
                Console.WriteLine($"현재 체력 : {hp}");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("던전 클리어에 성공했습니다.");//1700 + (공격력% ~ (공격력 * 2)%) 만큼 추가 보상 획득 가능
            Console.WriteLine($"보상으로 {baseReward + bonusReward} G를 획득했습니다.");
            Console.WriteLine($"기본 보상 {baseReward} G, 추가 보상 {bonusReward} G");
            playerGold += baseReward + bonusReward;
            Console.WriteLine($"현재 보유 골드 : {playerGold} G");
            Console.WriteLine($"체력이 {totalDamage} 만큼 감소했습니다.");
            if (totalDamage < hp)
            {
                hp -= totalDamage;
                Console.WriteLine($"현재 체력 : {hp}");
            }
            else if (totalDamage >= hp)
            {
                Console.WriteLine("체력이 0이 되었습니다.");
                hp = 0;
            }
            Console.WriteLine("던전에서 퇴장합니다.");
            Console.ReadKey();
            return;
        }
        static void HardDungeon()
        {
            int baseReward = 2500; // 기본 보상
            int bonusReward = random.Next(GetTotalAttack(), GetTotalAttack() * 2) / 100; // 보너스 보상
            int damage = random.Next(20, 35) + 15; // 던전 권장 방어력 만큼 데미지 증가
            int totalDamage = damage - GetTotalDefense(); // 총 데미지
            if (totalDamage <= 1) // 총 데미지가 1보다 작으면 1로 설정
            {
                totalDamage = 1;
            }

            Console.Clear();
            if (hp <= 0)
            {
                Console.WriteLine("체력이 없어서 던전 입장에 실패했습니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("어려운 던전");
            Console.WriteLine("어려운 던전으로 입장합니다.");
            if (GetTotalDefense() < 17 && random.Next(0, 100) < 40)
            {
                Console.WriteLine("방어력이 부족하여 40% 확률로 던전 클리어에 실패했습니다.");
                Console.WriteLine("체력 절반 감소 ");
                hp = hp / 2;
                Console.WriteLine($"현재 체력 : {hp}");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("던전 클리어에 성공했습니다.");//2500 + (공격력% ~ (공격력 * 2)%) 만큼 추가 보상 획득 가능
            Console.WriteLine($"보상으로 {baseReward + bonusReward} G를 획득했습니다.");
            Console.WriteLine($"기본 보상 {baseReward} G, 추가 보상 {bonusReward} G");
            playerGold += baseReward + bonusReward;
            Console.WriteLine($"현재 보유 골드 : {playerGold} G");
            Console.WriteLine($"체력이 {totalDamage} 만큼 감소했습니다.");
            if (totalDamage < hp)
            {
                hp -= totalDamage;
                Console.WriteLine($"현재 체력 : {hp}");
            }
            else if (totalDamage >= hp)
            {
                Console.WriteLine("체력이 0이 되었습니다.");
                hp = 0;
            }
            Console.WriteLine("던전에서 퇴장합니다.");
            Console.ReadKey();
            return;
        }
    }
    class SaveData
    {
        public int PlayerGold { get; set; }
        public int PlayerLevel { get; set; }
        public int HP { get; set; }
        public Program.Item[] Inventory { get; set; }
    }
    class SaveManager
    {
        private static string saveDirectory = "./saves";
        private static string saveFilePath = "./saves/save1.json";

        public static void SaveGame(SaveData saveData)
        {
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            string json = JsonSerializer.Serialize(saveData);
            File.WriteAllText(saveFilePath, json);
            Console.WriteLine("게임이 저장되었습니다.");
            Console.WriteLine("아무 키나 눌러주세요.");
            Console.ReadKey();
        }

        public static SaveData LoadGame()
        {
            if (!File.Exists(saveFilePath))
            {
                Console.WriteLine("저장된 파일이 없습니다.");
                Console.WriteLine("아무 키나 눌러주세요.");
                Console.ReadKey();
                return null;
            }

            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonSerializer.Deserialize<SaveData>(json);
            Console.WriteLine("게임이 로드되었습니다.");
            Console.WriteLine($"보유 골드 : {saveData.PlayerGold} G");
            Console.WriteLine($"레벨 : {saveData.PlayerLevel}");
            Console.WriteLine($"체력 : {saveData.HP} / 100");
            Console.WriteLine("[인벤토리]");
            foreach (var item in saveData.Inventory)
            {
                if (item.IsOwned)
                {
                    Console.WriteLine($"- {item.Name} | {item.Stat} | {item.Description}");
                }
            }
            Console.WriteLine("아무 키나 눌러주세요.");
            Console.ReadKey();
            return saveData;
        }
    }
}

