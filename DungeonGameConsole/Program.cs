// basklass som alla typer av karaktärer i spelet ärver från
using System.Security.Cryptography.X509Certificates;

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
    static void BuyItem(Player player, List<ShopItem> ShopList, List<ShopItem> InventoryList, int shopChoice, int buyQuantity)
    {
        if (ShopList[shopChoice - 1].Name == "Health Potion" || ShopList[shopChoice - 1].Name == "Super Health Potion")
        {
            for (int i = 0; i < buyQuantity; i++)
            {
                InventoryList.Add(ShopList[shopChoice - 1]);
                player.Gold -= (ShopList[shopChoice - 1].Price * buyQuantity);
            }

        }
        else if (ShopList[shopChoice - 1].Name == "Armor Upgrade")
        {
            for (int i = 0; i < buyQuantity; i++)
            {
                player.Defence += 5;
                player.Gold -= (ShopList[shopChoice - 1].Price * buyQuantity);
            }

            Console.WriteLine("Your defence has increased by " + buyQuantity * 5 + "!");
        }
        else if (ShopList[shopChoice - 1].Name == "Sword Upgrade")
        {
            for (int i = 0; i < buyQuantity; i++)
            {
                player.Defence += 5;
                player.Gold -= (ShopList[shopChoice - 1].Price * buyQuantity);
            }

            Console.WriteLine("Your damage has increased by " + buyQuantity * 5 + "!");
        }
    }
    static void Main(string[] args)
    {
        bool ShowMenu = true;
        bool ShowShop = true;
        string viewAnotherEnemy = "y";
        int shopChoice = 0;
        string buyChoice = "n";
        string quitChoice = "n";
        int buyQuantity;

        Console.Clear();
        Console.WriteLine("Choose player name: ");
        string playerName = Console.ReadLine();


        // format playerName, health, damage, level, experience, gold, defence
        Player player = new Player(playerName, 100, 15, 1, 0, 0, 0);

        List<Enemy> EnemyList = new List<Enemy>();
        // format name, health, damage, enemyLevel, givesExperience, givesGold
        EnemyList.Add(new Enemy("Goblin", 50, 10, 10, 20, 5));
        EnemyList.Add(new Enemy("Orc", 80, 20, 25, 20, 10));
        EnemyList.Add(new Enemy("Brute", 250, 20, 50, 60, 35));
        EnemyList.Add(new Enemy("Dragon", 500, 100, 200, 100, 100));

        List<ShopItem> ShopList = new List<ShopItem>();
        //format 
        ShopList.Add(new ShopItem("Health Potion", "Brewed from herbs found deep within the Whispering Forest. Adventurers swear it tastes worse than it smells.\nUsed in battles to restore 25 health", 10));
        ShopList.Add(new ShopItem("Super Health Potion", "A rare crimson brew said to contain the blood of ancient beasts. One sip can bring even a dying warrior back to their feet.\nUsed in battles to restore 75 health ", 40));
        ShopList.Add(new ShopItem("Armor Upgrade", "Reinforced with fragments of old knight armor, each piece carries the scars of battles long forgotten.\nIncreases defence permanently by 5", 50));
        ShopList.Add(new ShopItem("Sword Upgrade", "Forged with a shard of enchanted steel, its edge grows sharper with every battle.\nIncreases damage dealt by the player permanently by 5", 50));

        List<ShopItem> InventoryList = new List<ShopItem>();


        if (playerName == "dev")
        {
            player.Gold = 100000;
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
            Console.WriteLine("6. Quit game");
            Console.WriteLine("\nChoose an option: ");
            int choice = Convert.ToInt32(Console.ReadLine());


            if (choice == 1)
            {
                // fighting
            }
            if (choice == 2)
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
            if (choice == 3)
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
            if (choice == 4)
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
                            Console.WriteLine("How many would you like to buy?:");
                            buyQuantity = Convert.ToInt32(Console.ReadLine());

                            if (buyChoice.ToLower() == "y")
                            {
                                if (player.Gold >= (ShopList[shopChoice - 1].Price * buyQuantity))
                                {
                                    Console.Clear();
                                    BuyItem(player, ShopList, InventoryList, shopChoice, buyQuantity);
                                    Console.WriteLine(ShopList[shopChoice - 1].Name + " purchased!");
                                    Thread.Sleep(2000);
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
                                //returns to lobby
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
            if (choice == 5)
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
            if (choice == 6)
            {
                Console.Clear();
                Console.WriteLine("IMPORTANT NOTE: YOU WILL LOSE ALL PROGRESS IF YOU QUIT!");
                Console.WriteLine("Are you 10000000000% sure you want to quit? (y/n)");
                quitChoice = Console.ReadLine();

                if (quitChoice.ToLower() == "y")
                {
                    Console.Clear();
                    Console.WriteLine("Thanks for playing!");
                    ShowMenu = false;
                }
                else if (quitChoice.ToLower() == "n")
                {
                    //returns to lobby
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