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
            return baseAttack + GetEquippedStat("ATK") + ((playerLevel - 1) / 2); //�� ���ݷ� = �⺻ ���ݷ� + ������ �������� ���ݷ� + ������ ���� �߰� ���ݷ�
        }

        static int GetTotalDefense()
        {
            return baseDefense + GetEquippedStat("DEF") + (playerLevel - 1); //�� ���� = �⺻ ���� + ������ �������� ���� + ������ ���� �߰� ����
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
                        return $"���ݷ� +{ATKStat}";
                    }
                    else if (DEFStat > 0 && ATKStat == 0)
                    {
                        return $"���� +{DEFStat}";
                    }
                    else if (ATKStat > 0 && DEFStat > 0)
                    {
                        return $"���ݷ� +{ATKStat} | ���� +{DEFStat}";
                    }
                    else
                    {
                        return "���� ����";
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
            Weapon, // ����
            Armor   // ��
        }

        static Item[] inventory = new Item[]
        {
        new Item { Name = "���谩��", ATKStat = 0, DEFStat = 5, Description = "����� ������� ưư�� �����Դϴ�.", IsOwned = true, IsEquipped = true, IsPurchased = true, Price = 1000, Category = ItemCategory.Armor },
        new Item { Name = "���ĸ�Ÿ�� â", ATKStat = 7,DEFStat = 0, Description = "���ĸ�Ÿ�� ������� ����ߴٴ� ������ â�Դϴ�.", IsOwned = true, IsEquipped = true, IsPurchased = true, Price = 3500, Category = ItemCategory.Weapon },
        new Item { Name = "���� ��", ATKStat = 2,DEFStat = 0, Description = "���� �� �� �ִ� ���� �� �Դϴ�.", IsOwned = true, IsEquipped = false , IsPurchased = true, Price = 600, Category = ItemCategory.Weapon },
        new Item { Name = "û�� ����", ATKStat = 5, DEFStat = 0, Description = "��𼱰� ���ƴ��� ���� �����Դϴ�.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 1500, Category = ItemCategory.Weapon },
        new Item { Name = "���ĸ�Ÿ�� ����", ATKStat = 0, DEFStat = 15, Description = "���ĸ�Ÿ�� ������� ����ߴٴ� ������ �����Դϴ�.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 3500, Category = ItemCategory.Armor },
        new Item { Name = "������ ����", ATKStat = 0, DEFStat = 5, Description = "���ÿ� ������ �ִ� �����Դϴ�.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 1000, Category = ItemCategory.Armor },
        new Item { Name = "�ְ��� ��", ATKStat = 99, DEFStat = 99, Description = "������� �ְ��� ���Դϴ�.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 9999, Category = ItemCategory.Weapon },
        new Item { Name = "������� ����", ATKStat = 0, DEFStat = 0, Description = "������� ������� ��Ŀ� �����Դϴ�.", IsOwned = false, IsEquipped = false , IsPurchased = false, Price = 9999, Category = ItemCategory.Armor },
        };

        static void ShowMainMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("���ĸ�Ÿ ������ ���� ������ ȯ���մϴ�.");
                Console.WriteLine("�̰����� �������� ���� �� Ȱ���� �� �� �ֽ��ϴ�.");
                Console.WriteLine();
                Console.WriteLine("1. ���� ����");
                Console.WriteLine("2. �κ��丮");
                Console.WriteLine("3. ����");
                Console.WriteLine("4. ���� ����");
                Console.WriteLine("5. �޽��ϱ�");
                Console.WriteLine("6. ���̺��ϱ�");
                Console.WriteLine("7. �ε��ϱ�");
                Console.WriteLine("0. �����ϱ�");
                Console.WriteLine();
                Console.WriteLine("���Ͻô� �ൿ�� �Է����ּ���.");
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
                        Console.WriteLine("������ �����մϴ�.");
                        return; // ���α׷� ����
                    default:
                        Console.WriteLine("�߸��� �Է��Դϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
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
                Console.WriteLine("���� ����");
                Console.WriteLine("ĳ������ ������ ǥ�õ˴ϴ�.");
                Console.WriteLine();
                Console.WriteLine($"Lv. {playerLevel}");
                Console.WriteLine("Chad ( ���� )");
                Console.WriteLine($"���ݷ� : {totalAttack} (+{GetEquippedStat("ATK")})");
                Console.WriteLine($"���� : {totalDefense} (+{GetEquippedStat("DEF")})");
                Console.WriteLine($"ü �� : {hp} / 100");
                Console.WriteLine($"Gold : {playerGold} G");
                Console.WriteLine();
                Console.WriteLine("0. ������");
                Console.WriteLine();
                string input = Console.ReadLine();

                if (input == "0")
                {
                    Console.WriteLine(" ���� �޴��� ���ư��ϴ�.");
                    return;//���� �޴��� ���ư���
                }
                else
                {
                    Console.WriteLine("�߸��� �Է��Դϴ�.");
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowInventory()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("�κ��丮");
                Console.WriteLine("���� ���� �������� ������ �� �ֽ��ϴ�.");
                Console.WriteLine();
                Console.WriteLine("[������ ���]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (!item.IsOwned) continue; // �������� ���� �������� ǥ������ ����
                    string equippedMark = item.IsEquipped ? "[E]" : ""; // ���� ���ο� ���� [E] ǥ��
                    Console.WriteLine($"- {equippedMark}{item.Name} | {item.Stat} | {item.Description}");
                }
                Console.WriteLine("1. ���� ����");
                Console.WriteLine("0. ������");
                Console.WriteLine();
                Console.Write("���Ͻô� �ൿ�� �Է����ּ���.\r\n>> ");
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
                    Console.WriteLine("�߸��� �Է��Դϴ�.");
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
            }
        }

        static void ManageEquipment()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("�κ��丮 - ���� ����");
                Console.WriteLine("���� ���� �������� ���� �Ǵ� ������ �� �ֽ��ϴ�.");
                Console.WriteLine();
                Console.WriteLine("[������ ���]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (!item.IsOwned) continue; // �������� ���� �������� ǥ������ ����
                    string equippedMark = item.IsEquipped ? "[E]" : ""; // ���� ���ο� ���� [E] ǥ��
                    Console.WriteLine($"{i + 1}. {equippedMark}{item.Name} | {item.Stat} | {item.Description}");
                }
                Console.WriteLine();
                Console.WriteLine("0. ������");
                Console.WriteLine();
                Console.Write("����/������ ������ ��ȣ�� �Է����ּ���.\r\n>> ");
                string Input = Console.ReadLine();

                if (Input == "0")
                {
                    return; // �κ��丮�� ���ư���                  
                }
                else if (int.TryParse(Input, out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Length)
                {
                    var selectedItem = inventory[itemIndex - 1];
                    if (selectedItem.IsOwned)
                    {
                        // ����� �� �� �ϳ��� ���� �����ϵ��� ���� �߰�
                        if (selectedItem.IsEquipped)
                        {
                            // �̹� ������ �������� ����
                            selectedItem.IsEquipped = false;
                        }
                        else
                        {
                            // ���� �з��� �ٸ� �������� �����Ǿ� �ִ��� Ȯ��
                            foreach (var item in inventory)
                            {
                                if (item.IsEquipped && item.Category == selectedItem.Category)
                                {
                                    item.IsEquipped = false; // ���� ���� ����
                                }
                            }
                            selectedItem.IsEquipped = true; // ������ ������ ����
                        }
                    }
                    else
                    {
                        Console.WriteLine("�������� ���� �������Դϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
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
                Console.WriteLine("����");
                Console.WriteLine("�ʿ��� �������� ��ų� �Ǹ��� �� �ִ� �����Դϴ�.");
                Console.WriteLine();
                Console.WriteLine("[���� ���]");
                Console.WriteLine($"{playerGold} G");
                Console.WriteLine();
                Console.WriteLine("1. ����â");
                Console.WriteLine("2. �Ǹ�â");
                Console.WriteLine("0. ������");
                Console.WriteLine();
                Console.Write("���Ͻô� �ൿ�� �Է����ּ���.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // ���� ����
                }
                else if (input == "1")
                {
                    ShowPurchaseMenu(); // ����â���� �̵�
                }
                else if (input == "2")
                {
                    ShowSellMenu(); // �Ǹ�â���� �̵�
                }
                else
                {
                    Console.WriteLine("�߸��� �Է��Դϴ�.");
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowPurchaseMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("����â");
                Console.WriteLine("���� ������ ������ ����Դϴ�.");
                Console.WriteLine();
                Console.WriteLine("[���� ���]");
                Console.WriteLine($"{playerGold} G");
                Console.WriteLine();
                Console.WriteLine("[������ ���]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (item.IsPurchased) continue; // �̹� ������ �������� ǥ������ ����
                    Console.WriteLine($"{i + 1}. {item.Name} | {item.Stat} | {item.Description} | {item.Price} G");
                }
                Console.WriteLine();
                Console.WriteLine("0. ������");
                Console.WriteLine();
                Console.Write("������ ������ ��ȣ�� �Է����ּ���.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // ����â ����
                }
                else if (int.TryParse(input, out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Length)
                {
                    var selectedItem = inventory[itemIndex - 1];
                    if (selectedItem.IsPurchased)
                    {
                        Console.WriteLine("�̹� ������ �������Դϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                        Console.ReadKey();
                    }
                    else if (playerGold < selectedItem.Price)
                    {
                        Console.WriteLine("Gold�� �����մϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                        Console.ReadKey();
                    }
                    else
                    {
                        selectedItem.IsOwned = true;
                        selectedItem.IsEquipped = false;
                        selectedItem.IsPurchased = true;
                        playerGold -= selectedItem.Price;
                        Console.WriteLine($"{selectedItem.Name}��(��) �����߽��ϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("�߸��� �Է��Դϴ�.");
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
            }
        }

        static void ShowSellMenu()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("�Ǹ�â");
                Console.WriteLine("�Ǹ� ������ ������ ����Դϴ�.");
                Console.WriteLine();
                Console.WriteLine("[���� ���]");
                Console.WriteLine($"{playerGold} G");
                Console.WriteLine();
                Console.WriteLine("[������ ���]");
                for (int i = 0; i < inventory.Length; i++)
                {
                    var item = inventory[i];
                    if (!item.IsOwned) continue; // �������� ���� �������� ǥ������ ����
                    Console.WriteLine($"{i + 1}. {item.Name} | {item.Stat} | {item.Description} | �Ǹ� ����: {(item.Price * 0.85):F0} G");
                }
                Console.WriteLine();
                Console.WriteLine("0. ������");
                Console.WriteLine();
                Console.Write("�Ǹ��� ������ ��ȣ�� �Է����ּ���.\r\n>> ");
                string input = Console.ReadLine();

                if (input == "0")
                {
                    return; // �Ǹ�â ����
                }
                else if (int.TryParse(input, out int itemIndex) && itemIndex >= 1 && itemIndex <= inventory.Length)
                {
                    var selectedItem = inventory[itemIndex - 1];
                    if (!selectedItem.IsOwned)
                    {
                        Console.WriteLine("�������� ���� �������Դϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                        Console.ReadKey();
                    }
                    else
                    {
                        selectedItem.IsOwned = false;
                        selectedItem.IsEquipped = false;
                        selectedItem.IsPurchased = false;
                        playerGold += (int)(selectedItem.Price * 0.85); // �Ǹ� ������ ���� ������ 85%
                        Console.WriteLine($"{selectedItem.Name}��(��) �Ǹ��߽��ϴ�.");
                        Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                        Console.ReadKey();
                    }
                }
                else
                {
                    Console.WriteLine("�߸��� �Է��Դϴ�.");
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
            }
        }
        static void EnterDungeon()
        {
            int totalAttack = GetTotalAttack();
            int totalDefense = GetTotalDefense();

            Console.Clear();
            Console.WriteLine("��������");
            Console.WriteLine("���� ����");
            Console.WriteLine("�̰����� �������� ���� �� Ȱ���� �� �� �ֽ��ϴ�.");
            Console.WriteLine();
            Console.WriteLine("1. ���� ���� | ���� 5 �̻� ����");
            Console.WriteLine("2. �Ϲ� ���� | ���� 11 �̻� ����");
            Console.WriteLine("3. ����� ���� | ���� 17 �̻� ����");
            Console.WriteLine("0. ������");
            Console.WriteLine();
            Console.Write("���Ͻô� �ൿ�� �Է����ּ���.\r\n>> ");
            string input = Console.ReadLine();

            if (input == "0")
            {
                return; // ���� �޴��� ���ư���
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
                Console.WriteLine("�߸��� �Է��Դϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
            }

        }

        static void Rest()
        {
            Console.Clear();
            Console.WriteLine("�޽��ϱ�");
            Console.WriteLine($"500 G �� ���� ü���� ȸ���� �� �ֽ��ϴ�. (���� ��� : {playerGold} G)");
            Console.WriteLine("1. ü�� ȸ���ϱ�");
            Console.WriteLine("0. ������");
            Console.WriteLine();
            Console.Write("���Ͻô� �ൿ�� �Է����ּ���.\r\n>> ");
            string input = Console.ReadLine();
            if (input == "0")
            {
                return; // ���� �޴��� ���ư���
            }
            else if (input == "1" && hp >= 100)
            {
                Console.WriteLine("ü���� �̹� �����Դϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
            }
            else if (input == "1" && hp < 100)
            {
                if (playerGold < 500)
                {
                    Console.WriteLine("Gold�� �����մϴ�.");
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
                else
                {
                    playerGold -= 500;
                    Console.WriteLine("ü���� ȸ���߽��ϴ�.");
                    hp = 100;
                    Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("�߸��� �Է��Դϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
            }

        }
        static void EasyDungeon()
        {
            int baseReward = 1000; // �⺻ ����
            int bonusReward = random.Next(GetTotalAttack(), GetTotalAttack() * 2) / 100; // ���ʽ� ����
            int damage = random.Next(20, 35) + 5; // ���� ���� ���� ��ŭ ������ ����
            int totalDamage = damage - GetTotalDefense(); // �� ������
            if (totalDamage <= 1) // �� �������� 1���� ������ 1�� ����
            {
                totalDamage = 1;
            }


            Console.Clear();
            if (hp <= 0)
            {
                Console.WriteLine("ü���� ��� ���� ���忡 �����߽��ϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("���� ����");
            Console.WriteLine("���� �������� �����մϴ�.");
            if (GetTotalDefense() < 5 && random.Next(0, 100) < 40)
            {

                Console.WriteLine("������ �����Ͽ� 40% Ȯ���� ���� Ŭ��� �����߽��ϴ�.");
                Console.WriteLine("ü�� ���� ���� ");
                hp = hp / 2;
                Console.WriteLine($"���� ü�� : {hp}");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("���� Ŭ��� �����߽��ϴ�.");//1000 + (���ݷ�% ~ (���ݷ� * 2)%) ��ŭ �߰� ���� ȹ�� ����
            Console.WriteLine($"�������� {baseReward + bonusReward} G�� ȹ���߽��ϴ�.");
            Console.WriteLine($"�⺻ ���� {baseReward} G, �߰� ���� {bonusReward} G");
            playerGold += baseReward + bonusReward;
            Console.WriteLine($"���� ���� ��� : {playerGold} G");
            Console.WriteLine($"ü���� {totalDamage} ��ŭ �����߽��ϴ�.");
            if (totalDamage < hp)
            {
                hp -= totalDamage;
                Console.WriteLine($"���� ü�� : {hp}");
            }
            else if (totalDamage >= hp)
            {
                Console.WriteLine("ü���� 0�� �Ǿ����ϴ�.");
                hp = 0;
            }
            Console.WriteLine("�������� �����մϴ�.");
            Console.ReadKey();
            return;
        }
        static void NormalDungeon()
        {
            int baseReward = 1700; // �⺻ ����
            int bonusReward = random.Next(GetTotalAttack(), GetTotalAttack() * 2) / 100; // ���ʽ� ����
            int damage = random.Next(20, 35) + 10; // ���� ���� ���� ��ŭ ������ ����
            int totalDamage = damage - GetTotalDefense(); // �� ������
            if (totalDamage <= 1) // �� �������� 1���� ������ 1�� ����
            {
                totalDamage = 1;
            }

            Console.Clear();
            if (hp <= 0)
            {
                Console.WriteLine("ü���� ��� ���� ���忡 �����߽��ϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("�Ϲ� ����");
            Console.WriteLine("�Ϲ� �������� �����մϴ�.");
            if (GetTotalDefense() < 11 && random.Next(0, 100) < 40)
            {
                Console.WriteLine("������ �����Ͽ� 40% Ȯ���� ���� Ŭ��� �����߽��ϴ�.");
                Console.WriteLine("ü�� ���� ���� ");
                hp = hp / 2;
                Console.WriteLine($"���� ü�� : {hp}");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("���� Ŭ��� �����߽��ϴ�.");//1700 + (���ݷ�% ~ (���ݷ� * 2)%) ��ŭ �߰� ���� ȹ�� ����
            Console.WriteLine($"�������� {baseReward + bonusReward} G�� ȹ���߽��ϴ�.");
            Console.WriteLine($"�⺻ ���� {baseReward} G, �߰� ���� {bonusReward} G");
            playerGold += baseReward + bonusReward;
            Console.WriteLine($"���� ���� ��� : {playerGold} G");
            Console.WriteLine($"ü���� {totalDamage} ��ŭ �����߽��ϴ�.");
            if (totalDamage < hp)
            {
                hp -= totalDamage;
                Console.WriteLine($"���� ü�� : {hp}");
            }
            else if (totalDamage >= hp)
            {
                Console.WriteLine("ü���� 0�� �Ǿ����ϴ�.");
                hp = 0;
            }
            Console.WriteLine("�������� �����մϴ�.");
            Console.ReadKey();
            return;
        }
        static void HardDungeon()
        {
            int baseReward = 2500; // �⺻ ����
            int bonusReward = random.Next(GetTotalAttack(), GetTotalAttack() * 2) / 100; // ���ʽ� ����
            int damage = random.Next(20, 35) + 15; // ���� ���� ���� ��ŭ ������ ����
            int totalDamage = damage - GetTotalDefense(); // �� ������
            if (totalDamage <= 1) // �� �������� 1���� ������ 1�� ����
            {
                totalDamage = 1;
            }

            Console.Clear();
            if (hp <= 0)
            {
                Console.WriteLine("ü���� ��� ���� ���忡 �����߽��ϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("����� ����");
            Console.WriteLine("����� �������� �����մϴ�.");
            if (GetTotalDefense() < 17 && random.Next(0, 100) < 40)
            {
                Console.WriteLine("������ �����Ͽ� 40% Ȯ���� ���� Ŭ��� �����߽��ϴ�.");
                Console.WriteLine("ü�� ���� ���� ");
                hp = hp / 2;
                Console.WriteLine($"���� ü�� : {hp}");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("���� Ŭ��� �����߽��ϴ�.");//2500 + (���ݷ�% ~ (���ݷ� * 2)%) ��ŭ �߰� ���� ȹ�� ����
            Console.WriteLine($"�������� {baseReward + bonusReward} G�� ȹ���߽��ϴ�.");
            Console.WriteLine($"�⺻ ���� {baseReward} G, �߰� ���� {bonusReward} G");
            playerGold += baseReward + bonusReward;
            Console.WriteLine($"���� ���� ��� : {playerGold} G");
            Console.WriteLine($"ü���� {totalDamage} ��ŭ �����߽��ϴ�.");
            if (totalDamage < hp)
            {
                hp -= totalDamage;
                Console.WriteLine($"���� ü�� : {hp}");
            }
            else if (totalDamage >= hp)
            {
                Console.WriteLine("ü���� 0�� �Ǿ����ϴ�.");
                hp = 0;
            }
            Console.WriteLine("�������� �����մϴ�.");
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
            Console.WriteLine("������ ����Ǿ����ϴ�.");
            Console.WriteLine("�ƹ� Ű�� �����ּ���.");
            Console.ReadKey();
        }

        public static SaveData LoadGame()
        {
            if (!File.Exists(saveFilePath))
            {
                Console.WriteLine("����� ������ �����ϴ�.");
                Console.WriteLine("�ƹ� Ű�� �����ּ���.");
                Console.ReadKey();
                return null;
            }

            string json = File.ReadAllText(saveFilePath);
            SaveData saveData = JsonSerializer.Deserialize<SaveData>(json);
            Console.WriteLine("������ �ε�Ǿ����ϴ�.");
            Console.WriteLine($"���� ��� : {saveData.PlayerGold} G");
            Console.WriteLine($"���� : {saveData.PlayerLevel}");
            Console.WriteLine($"ü�� : {saveData.HP} / 100");
            Console.WriteLine("[�κ��丮]");
            foreach (var item in saveData.Inventory)
            {
                if (item.IsOwned)
                {
                    Console.WriteLine($"- {item.Name} | {item.Stat} | {item.Description}");
                }
            }
            Console.WriteLine("�ƹ� Ű�� �����ּ���.");
            Console.ReadKey();
            return saveData;
        }
    }
}

