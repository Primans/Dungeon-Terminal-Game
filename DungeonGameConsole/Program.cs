using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.IO;
using System.Linq;

    public class Character
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int Damage { get; set; }

        public Character(string name, int health, int damage)
        {
            Name = name;
            Health = health;
            Damage = damage;
        }
    }

    public class Player : Character
    {
        public int Level { get; set; }
        public int Experience { get; set; }
        public int Gold { get; set; }
        public int Defence { get; set; }

        public Player(string name, int health, int damage, int level, int experience, int gold, int defence) : base(name, health, damage)
        {
            Level = level;
            Experience = experience;
            Gold = gold;
            Defence = defence;
        }
    }

    public class Enemy : Character
    {
        public int EnemyLevel { get; set; }
        public int GivesExperience { get; set; }
        public int GivesGold { get; set; }

        public Enemy(string name, int health, int damage, int enemyLevel, int givesExperience, int givesGold) : base(name, health, damage)
        {
            EnemyLevel = enemyLevel;
            GivesExperience = givesExperience;
            GivesGold = givesGold;
        }
    }

    public class ShopItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }

        public ShopItem(string name, string description, int price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }


    public class Program
    {
        static void SaveGame(Player player, List<ShopItem> InventoryList)
        {
            //Skapar en lista med spelarens information
            List<string> saveData = new List<string>();

            saveData.Add(player.Name);
            saveData.Add(player.Health.ToString());
            saveData.Add(player.Damage.ToString());
            saveData.Add(player.Level.ToString());
            saveData.Add(player.Experience.ToString());
            saveData.Add(player.Gold.ToString());
            saveData.Add(player.Defence.ToString());

            //Sparar varje föremål från inventory
            foreach (ShopItem item in InventoryList)
            {
                saveData.Add(item.Name);
            }

            //Skriver all information från listan till en fil, en rad per objekt
            string savePath = Path.GetFullPath("../../../../save.txt", AppContext.BaseDirectory);
            File.WriteAllLines(savePath, saveData);

            Console.WriteLine("Game saved successfully!");
            Console.WriteLine("Created 'save.txt' in DungeonGameConsole.");
            Console.WriteLine("Press Enter to return to the main menu...");
        }
        static bool LoadGame(Player player, List<ShopItem> ShopList, List<ShopItem> InventoryList)
        {
            //Kontrollerar om det finns en sparfil
            string savePath = Path.GetFullPath("../../../../save.txt", AppContext.BaseDirectory);

            if (!File.Exists(savePath))
            {
                Console.Clear();
                Console.WriteLine("No save file found!");
                Console.WriteLine("Exiting...");

                return false;
            }

            //Läser filen
            string[] saveData = File.ReadAllLines(savePath);

            //Laddar spelarens statistik
            player.Name = saveData[0];
            player.Health = Convert.ToInt32(saveData[1]);
            player.Damage = Convert.ToInt32(saveData[2]);
            player.Level = Convert.ToInt32(saveData[3]);
            player.Experience = Convert.ToInt32(saveData[4]);
            player.Gold = Convert.ToInt32(saveData[5]);
            player.Defence = Convert.ToInt32(saveData[6]);

            //Rensar inventory innan föremålen laddas för säkerhets skull
            InventoryList.Clear();

            //Laddar tillbaka föremålen från save filen
            for (int i = 7; i < saveData.Length; i++)
            {
                ShopItem item = ShopList.FirstOrDefault(x => x.Name == saveData[i]);

                if (item != null)
                {
                    InventoryList.Add(item);
                }
            }

            Console.Clear();
            Console.WriteLine("Game loaded!");
            Console.WriteLine("Welcome back, " + player.Name + "!");
            Thread.Sleep(2000);

            return true;
        }

        static void BuyItem(Player player, List<ShopItem> ShopList, List<ShopItem> InventoryList, int shopChoice, int buyQuantity)
    {
        if (ShopList[shopChoice - 1].Name == "Health Potion" || ShopList[shopChoice - 1].Name == "Super Health Potion")
        {
            for (int i = 0; i < buyQuantity; i++)
            {
                InventoryList.Add(ShopList[shopChoice - 1]);
            }

            player.Gold -= (ShopList[shopChoice - 1].Price * buyQuantity);

            Console.WriteLine("You have purchased " + buyQuantity + " " + ShopList[shopChoice - 1].Name + "!");
        }
        else if (ShopList[shopChoice - 1].Name == "Armor Upgrade")
        {
            for (int i = 0; i < buyQuantity; i++)
            {
                player.Defence += 5;
            }
            player.Gold -= (ShopList[shopChoice - 1].Price * buyQuantity);

            Console.WriteLine("Your defence has increased by " + buyQuantity * 5 + "!");
        }
        else if (ShopList[shopChoice - 1].Name == "Sword Upgrade")
        {
            for (int i = 0; i < buyQuantity; i++)
            {
                player.Damage += 5;
            }
            player.Gold -= (ShopList[shopChoice - 1].Price * buyQuantity);

            Console.WriteLine("Your damage has increased by " + buyQuantity * 5 + "!");
        }
    }
        static void LevelUp(Player player)
        {
            while (player.Experience >= 100)
            {
                player.Experience -= 100;
                player.Level++;

                Console.WriteLine("\nLEVEL UP!");
                Console.WriteLine("You are now level " + player.Level + "!");
                Console.WriteLine("Damage and defence have increased by 5 and 2 respectively!");

                // Öka spelarens stats
                player.Damage += 5;
                player.Defence += 2;
            }
        }

        static void Main(string[] args)
        {
            bool ShowMenu = true;
            bool ShowShop = true;
            string viewAnotherEnemy = "y";
            int shopChoice = 0;
            int healChoice = 0;
            string buyChoice = "n";
            string quitChoice = "n";
            string fightChoice;
            string attackChoice;
            int buyQuantity;


            // format name, health, damage, level, experience, gold, defence
            Player player = new Player("", 100, 15, 1, 0, 0, 0);

            List<Enemy> EnemyList = new List<Enemy>();
            // format name, health, damage, enemyLevel, givesExperience, givesGold
            EnemyList.Add(new Enemy("Goblin", 50, 10, 10, 20, 5));
            EnemyList.Add(new Enemy("Orc", 80, 20, 25, 20, 10));
            EnemyList.Add(new Enemy("Brute", 250, 20, 50, 60, 35));
            EnemyList.Add(new Enemy("Dragon", 500, 100, 200, 100, 100));

            List<ShopItem> ShopList = new List<ShopItem>();
            //format name, description, price
            ShopList.Add(new ShopItem("Health Potion", "Brewed from herbs found deep within the Whispering Forest. Adventurers swear it tastes worse than it smells.\nUsed in battles to restore ut to 25 health", 10));
            ShopList.Add(new ShopItem("Super Health Potion", "A rare crimson brew said to contain the blood of ancient beasts. One sip can bring even a dying warrior back to their feet.\nUsed in battles to restore up to 75 health ", 40));
            ShopList.Add(new ShopItem("Armor Upgrade", "Reinforced with fragments of old knight armor, each piece carries the scars of battles long forgotten.\nIncreases defence permanently by 5", 50));
            ShopList.Add(new ShopItem("Sword Upgrade", "Forged with a shard of enchanted steel, its edge grows sharper with every battle.\nIncreases damage dealt by the player permanently by 5", 50));

            List<ShopItem> InventoryList = new List<ShopItem>();

            //load or new game
            Console.Clear();
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game from local 'save.txt' file");
            Console.WriteLine("\nChoose an option: ");
            int saveChoice = Convert.ToInt32(Console.ReadLine());
            if (saveChoice == 1)
            {
                Console.Clear();
                Console.WriteLine("Choose player name: ");
                player.Name = Console.ReadLine();
            }
            if (saveChoice == 2)
            {
                bool loaded = LoadGame(player, ShopList, InventoryList);

                if (loaded == false)
                {
                    return;
                }
            }


            if (player.Name == "dev")
            {
                player.Gold = 100000;
                player.Experience = 90;
            }


            while (ShowMenu)
            {
                Console.Clear();

                Console.WriteLine("====================\n    DUNGEON GAME\n====================");
                Console.WriteLine("1. Fight random enemy");
                Console.WriteLine("2. Show player stats");
                Console.WriteLine("3. Show the enemies stats");
                Console.WriteLine("4. Open shop");
                Console.WriteLine("5. View inventory");
                Console.WriteLine("6. Credits");
                Console.WriteLine("7. View guide");
                Console.WriteLine("8. Save game");
                Console.WriteLine("9. Quit game");
                Console.WriteLine("\nChoose an option: ");
                int choice = Convert.ToInt32(Console.ReadLine());


                if (choice == 1) //fight random enemy
                {
                    // väljer en random enemy och skapar en variabel som du kan använda för att referera till den
                    int randomEnemyIndex = new Random().Next(EnemyList.Count);
                    Enemy enemy = EnemyList[randomEnemyIndex];
                    int originalHealth = enemy.Health;


                    Console.Clear();
                    Console.WriteLine("A wild LVL[" + enemy.EnemyLevel + "] " + enemy.Name + " has appeared!");
                    Console.WriteLine("Do you accept or decline the fight? (a/d)");

                    Console.WriteLine("\nChoose an option: ");
                    fightChoice = Console.ReadLine();

                    if (fightChoice.ToLower() == "a")
                    {
                        Console.Clear();
                        Console.WriteLine("You have accepted the fight!");
                        Console.WriteLine("The LVL[" + enemy.EnemyLevel + "] " + enemy.Name + " has " + enemy.Health + " health and deals " + enemy.Damage + " damage.");
                        Thread.Sleep(5000);


                        while (enemy.Health > 0 && player.Health > 0)
                        {
                            // enemy attacks
                            Console.Clear();
                            int damageTaken = Math.Max(enemy.Damage - player.Defence, 0);
                            Console.WriteLine("The LVL[" + enemy.EnemyLevel + "] " + enemy.Name + " swings and deals " + damageTaken + " damage to you!");
                            player.Health -= damageTaken;
                            Console.WriteLine("You have " + player.Health + " health left.");

                            if (player.Health <= 0)
                            {
                                // defeat!
                                Console.Clear();
                                Console.WriteLine("You don't have any health left! GG");
                                Console.WriteLine("You have been defeated by the LVL[" + enemy.EnemyLevel + "] " + enemy.Name + "!");
                                player.Health = 100;
                                enemy.Health = originalHealth;
                                Thread.Sleep(4000);
                                break;
                            }
                            else
                            {
                                // player chooses action
                                bool validChoice = false;

                                while (!validChoice)
                                {
                                    Console.WriteLine("\nQuick! What do you do?! Attack or Heal using a potion? (a/h)");
                                    attackChoice = Console.ReadLine();

                                    if (attackChoice.ToLower() == "a")
                                    {
                                        validChoice = true;

                                        Console.Clear();
                                        Console.WriteLine("You swing your sword and deal " + player.Damage + " damage to the LVL[" + enemy.EnemyLevel + "] " + enemy.Name + "!");
                                        enemy.Health -= player.Damage;

                                        if (enemy.Health <= 0)
                                        {
                                            enemy.Health = 0;
                                        }

                                        Console.WriteLine("The LVL[" + enemy.EnemyLevel + "] " + enemy.Name + " has " + enemy.Health + " health left.");
                                        Thread.Sleep(5000);
                                    }
                                    else if (attackChoice.ToLower() == "h")
                                    {
                                        if (player.Health >= 100)
                                        {
                                            Console.Clear();
                                            Console.WriteLine("You already have full health!");
                                            Thread.Sleep(1500);
                                        }

                                        else
                                        {
                                            Console.Clear();
                                            Console.WriteLine("You have " + player.Health + " health.");
                                            Console.WriteLine("1. Health Potion (Restores 25 health)");
                                            Console.WriteLine("2. Super Health Potion (Restores 75 health)");
                                            Console.WriteLine("Which potion would you like to use?: ");

                                            healChoice = Convert.ToInt32(Console.ReadLine());

                                            if (healChoice == 1)
                                            {
                                                if (InventoryList.Any(x => x.Name == "Health Potion"))
                                                {
                                                    validChoice = true;

                                                    var potion = InventoryList.First(x => x.Name == "Health Potion");
                                                    InventoryList.Remove(potion);

                                                    player.Health = Math.Min(player.Health + 25, 100);
                                                    Console.Clear();
                                                    Console.WriteLine("You have used a Health Potion!");
                                                    Console.WriteLine("You now have " + player.Health + " health.");
                                                    Thread.Sleep(3000);
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("You don't have that potion in your inventory! Try again!\n");
                                                    Thread.Sleep(2000);
                                                }
                                            }
                                            else if (healChoice == 2)
                                            {
                                                if (InventoryList.Any(x => x.Name == "Super Health Potion"))
                                                {
                                                    validChoice = true;

                                                    var potion = InventoryList.First(x => x.Name == "Super Health Potion");
                                                    InventoryList.Remove(potion);

                                                    player.Health = Math.Min(player.Health + 75, 100);
                                                    Console.Clear();
                                                    Console.WriteLine("You have used a Super Health Potion!");
                                                    Console.WriteLine("You now have " + player.Health + " health.");
                                                    Thread.Sleep(3000);
                                                }
                                                else
                                                {
                                                    Console.Clear();
                                                    Console.WriteLine("You don't have that potion in your inventory! Try again!\n");
                                                    Thread.Sleep(2000);
                                                }
                                            }
                                            else if (healChoice != 1 && healChoice != 2)
                                            {
                                                Console.Clear();
                                                Console.WriteLine("Invalid choice. Try again!\n");
                                                Thread.Sleep(2000);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Invalid choice. Try again!\n");
                                        Thread.Sleep(1500);
                                        Console.Clear();
                                    }
                                }
                            }
                        }

                        if (enemy.Health <= 0)
                        {
                            // victory!
                            Console.Clear();
                            Console.WriteLine("You have defeated the LVL[" + enemy.EnemyLevel + "] " + enemy.Name + "!");
                            Console.WriteLine("You have gained " + enemy.GivesExperience + " experience and " + enemy.GivesGold + " gold!");
                            player.Experience += enemy.GivesExperience;
                            player.Gold += enemy.GivesGold;

                            LevelUp(player);

                            enemy.Health = originalHealth;
                            player.Health = 100;
                            Console.WriteLine("You now have " + player.Experience + "/100 experience and " + player.Gold + " gold.");
                            Console.WriteLine("\nYour health has also been reset to 100!");
                            Console.WriteLine("\nPress enter to return to the main menu.");
                            Console.ReadLine();
                        }
                    }
                    else if (fightChoice.ToLower() == "d")
                    {
                        Console.Clear();
                        Console.WriteLine("You have declined the fight, come back when you have grown a pair of balls!");
                        Thread.Sleep(3000);
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine("Invalid choice. Returning to main menu.\n");
                        Thread.Sleep(1500);
                    }
                }
                if (choice == 2) //view player stats
                {
                    Console.Clear();

                    Console.WriteLine("Player Stats:");
                    Console.WriteLine("Name: " + player.Name);
                    Console.WriteLine("Health: " + player.Health);
                    Console.WriteLine("Defence: " + player.Defence);
                    Console.WriteLine("Damage: " + player.Damage);
                    Console.WriteLine("Gold: " + player.Gold);
                    Console.WriteLine("Level: " + player.Level);
                    Console.WriteLine("Experience: " + player.Experience + "/100");

                    Console.WriteLine("\nPress Enter to return...");
                    Console.ReadLine();
                }
                if (choice == 3) //view enemy stats
                {
                    viewAnotherEnemy = "y";

                    while (viewAnotherEnemy.ToLower() == "y")
                    {
                        Console.Clear();

                        Console.WriteLine("Which enemy would you like to view the stats of?");
                        Console.WriteLine("1. Goblin");
                        Console.WriteLine("2. Orc");
                        Console.WriteLine("3. Brute");
                        Console.WriteLine("4. Dragon");
                        Console.WriteLine("\nEnter number:");
                        int enemyChoice = Convert.ToInt32(Console.ReadLine());


                        Console.Clear();
                        Console.WriteLine(EnemyList[enemyChoice - 1].Name + "\n|Health| " + EnemyList[enemyChoice - 1].Health + "\n|Damage| " + EnemyList[enemyChoice - 1].Damage + "\n|XP Reward| " + EnemyList[enemyChoice - 1].GivesExperience + "\n|Gold Reward| " + EnemyList[enemyChoice - 1].GivesGold + "\n");

                        Console.WriteLine("\nWould you like to view another enemy? (y/n)");
                        viewAnotherEnemy = Console.ReadLine();
                    }
                }
                if (choice == 4) //open shop
                {
                    ShowShop = true;

                    while (ShowShop)
                    {
                        Console.Clear();

                        Console.WriteLine("Welcome to the shop!");
                        for (int i = 0; i < ShopList.Count; i++)
                        {
                            Console.WriteLine((i + 1) + ". " + ShopList[i].Name);
                        }
                        Console.WriteLine("5. Exit Shop");

                        Console.WriteLine("\nSelect an item to inspect: ");
                        shopChoice = Convert.ToInt32(Console.ReadLine());

                        Console.Clear();

                        if (shopChoice < 1 || shopChoice > (ShopList.Count + 1))
                        {
                            Console.WriteLine("Invalid choice. Returning to main menu.\n");
                            Thread.Sleep(1500);
                        }
                        else if (shopChoice == 5)
                        {
                            ShowShop = false;
                        }
                        else
                        {
                            Console.WriteLine(ShopList[shopChoice - 1].Name + "\n" + ShopList[shopChoice - 1].Description + "\nCost: " + ShopList[shopChoice - 1].Price + " Gold");
                            Console.WriteLine("\nWould you like to buy this item? (y/n)");
                            buyChoice = Console.ReadLine();

                            if (buyChoice.ToLower() != "y" && buyChoice.ToLower() != "n")
                            {
                                Console.Clear();
                                Console.WriteLine("Invalid choice returning to shop");
                                Thread.Sleep(1500);
                            }
                            else
                            {
                                Console.Clear();

                                if (buyChoice.ToLower() == "y")
                                {
                                    Console.WriteLine("How many would you like to buy?:");
                                    buyQuantity = Convert.ToInt32(Console.ReadLine());

                                    if (buyQuantity > 0 && player.Gold >= (ShopList[shopChoice - 1].Price * buyQuantity))
                                    {
                                        Console.Clear();
                                        BuyItem(player, ShopList, InventoryList, shopChoice, buyQuantity);
                                        Console.WriteLine(ShopList[shopChoice - 1].Name + " purchased!");
                                        Thread.Sleep(2000);
                                    }
                                    else if (buyQuantity <= 0)
                                    {
                                        Console.Clear();
                                        Console.WriteLine("You must buy at least 1 item. Returning to shop.");
                                        Thread.Sleep(1500);
                                    }
                                    else
                                    {
                                        Console.Clear();
                                        Console.WriteLine("You don't have enough gold to buy this item. Come back when you have grinded some more!");
                                        Thread.Sleep(3000);
                                    }
                                }
                                else if (buyChoice.ToLower() == "n")
                                {
                                    Console.WriteLine("Returning to main menu.\n");
                                    Thread.Sleep(500);
                                }
                                else
                                {
                                    Console.WriteLine("Invalid choice. Returning to main menu.\n");
                                    Thread.Sleep(1000);
                                }
                            }
                        }
                    }


                }
                if (choice == 5) //view inventory
                {
                    Console.Clear();
                    Console.WriteLine("Inventory:");
                    for (int i = 0; i < InventoryList.Count; i++)
                    {
                        Console.WriteLine((i + 1) + ". " + InventoryList[i].Name);
                    }

                    Console.WriteLine("\nPress Enter to return...");
                    Console.ReadLine();
                }
                if (choice == 6) //view credits
                {
                    Console.Clear();

                    Console.WriteLine("Game created by: [Primans]\nGithub: [https://github.com/Primans]\n\nSpecial thanks to: [Me, Myself and I]\n\nPress Enter to return...");
                    Console.ReadLine();
                }
                if (choice == 7) //view guide
                {
                    Console.Clear();
                    Console.WriteLine("Dungeon Game - Guide");
                    Console.WriteLine("Welcome to Dungeon Game! Your goal is to become stronger by fighting enemies, earning experience (XP), collecting gold and upgrading your character.");

                    Console.WriteLine("\nHow to play:");

                    Console.WriteLine("* Fight enemies: Choose option 1 in the main menu to encounter a random enemy. You can accept or decline the fight.");
                    Console.WriteLine("* Attack: Deal damage to your opponent using your sword.");
                    Console.WriteLine("* Heal: Use Health Potions or Super Health Potions to restore health during combat.");
                    Console.WriteLine("* Earn rewards: Defeating enemies grants you XP and gold.");
                    Console.WriteLine("* Level up: Every 100 XP increases your level. Leveling up also improves your character's stats.");
                    Console.WriteLine("* Visit the shop: Spend your gold on potions, armor upgrades and sword upgrades.");
                    Console.WriteLine("* Manage inventory: View your collected potions and use them during battles.");

                    Console.WriteLine("\nYour goal:");

                    Console.WriteLine("Defeat increasingly powerful enemies, improve your equipment and become the strongest adventurer in the dungeon!");

                    Console.WriteLine("\nGood luck, adventurer!\n");

                    Console.WriteLine("\n\nPress Enter to return to the menu...");
                    Console.ReadLine();
                }
                if (choice == 8) //save game
                {
                    Console.Clear();
                    Console.WriteLine("Saving game...");
                    
                    SaveGame(player, InventoryList);
                    Console.ReadLine();
                }
                if (choice == 9) //quit the game
                    {
                        Console.Clear();
                        Console.WriteLine("IMPORTANT NOTE: YOU WILL LOSE ALL UNSAVED PROGRESS IF YOU QUIT!");
                        Console.WriteLine("Are you 10000000000% sure you want to quit? (y/n)");
                        quitChoice = Console.ReadLine();

                        if (quitChoice.ToLower() == "y")
                        {
                            Console.Clear();
                            Console.WriteLine("Quitting game...");
                            Console.WriteLine("Thanks for playing!");
                            ShowMenu = false;
                        }
                        else if (quitChoice.ToLower() == "n")
                        {
                            //returns to lobby
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("Invalid choice. Returning to main menu.\n");
                            Thread.Sleep(1000);
                        }
                    }
            }
        }
    }