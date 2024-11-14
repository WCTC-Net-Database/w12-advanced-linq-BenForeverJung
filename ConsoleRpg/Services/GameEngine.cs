using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Equipments;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ConsoleRpg.Services;

public class GameEngine
{
    private readonly GameContext _context;
    private readonly MenuManager _menuManager;
    private readonly OutputManager _outputManager;

    private IPlayer _player;
    private IMonster _goblin;

    public GameEngine(GameContext context, MenuManager menuManager, OutputManager outputManager)
    {
        _menuManager = menuManager;
        _outputManager = outputManager;
        _context = context;
    }

    public void Run()
    {
        if (_menuManager.ShowMainMenu())
        {
            SetupGame();
        }
    }
    //  Main menu for Game Loop
    private void GameLoop()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Attack");
            _outputManager.WriteLine("2. Manage Character");
            _outputManager.WriteLine("3. Display Items");
            _outputManager.WriteLine("4. Quit");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    AttackCharacter();
                    break;
                case "2":
                    ManageCharacter();
                    ;
                    break;
                case "3":
                    DisplayItems();
                    break;
                case "4":
                    _outputManager.WriteLine("Exiting game...", ConsoleColor.Red);
                    _outputManager.Display();
                    Environment.Exit(0);
                    break;
                default:
                    _outputManager.WriteLine("Invalid selection. Please choose 1.", ConsoleColor.Red);
                    break;
            }
        }
    }


    public void ManageCharacter()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. View Your Inventory");
            _outputManager.WriteLine("2. Use Or Equip An Item From Your Inventory");
            _outputManager.WriteLine("3. Add An Item To your Inventory");
            _outputManager.WriteLine("4. Remove An Item From Your Inventory");
            _outputManager.WriteLine("5. Back To Main Menu");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    _player.ListInventory();
                    break;
                case "2":
                    _player.ListInventory();
                    Console.WriteLine("Enter Item Number To Use or Equip");
                    int itemNumber = Convert.ToInt32(Console.ReadLine());
                    var item = _context.Items.FirstOrDefault(i => i.Id == itemNumber);
                    var equipment = _context.Equipments.FirstOrDefault(e => e.Id == _player.EquipmentId);
                    _player.UseItem(item, equipment);
                    break;
                case "3":
                    ListAllItems();
                    Console.WriteLine("Enter Item Number To Add To Your Inventory");
                    itemNumber = Convert.ToInt32(Console.ReadLine());
                    item = _context.Items.FirstOrDefault(i => i.Id == itemNumber);
                    _player.AddItem(item);
                    break;
                case "4":
                    _player.ListInventory();
                    Console.WriteLine("Enter Item Number To Remove From Your Inventory");
                    itemNumber = Convert.ToInt32(Console.ReadLine());
                    item = _context.Items.FirstOrDefault(i => i.Id == itemNumber);
                    _player.RemoveItem(item);
                    break;
                case "5":
                    GameLoop();
                    break;
                default:
                    GameLoop();
                    break;
            }
        }

    }


    public void DisplayItems()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. Search Items by Name");
            _outputManager.WriteLine("2. Display Items by Type");
            _outputManager.WriteLine("3. Display Sorted Items");
            _outputManager.WriteLine("4. Back to Main Menu");

            _outputManager.Display();

            var input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    SearchItemsByName();
                    break;
                case "2":
                    ListItemsByType();
                    break;
                case "3":
                    SortItems();
                    break;
                case "4":
                    GameLoop();
                    break;
                default:
                    GameLoop();
                    break;
            }
        }
    }

    public void SearchItemsByName()
    {
        Console.WriteLine($"What item name would you like to search?");
        string? searchFor = Console.ReadLine();
        searchFor = searchFor.ToLower();

        var searchedItems = _context.Items.Where(item => item.Name.ToLower().Contains(searchFor));

        foreach (var item in searchedItems)
        {
            Console.WriteLine($"Items matching your search:");
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Type: {item.Type}, Attack Level: {item.Attack}, Defense Level: {item.Defense}");
        }
    }


    public void ListAllItems()
    {
        foreach (var item in _context.Items)
        {
            Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Type: {item.Type}, Attack Level: {item.Attack}, Defense Level: {item.Defense}");
        }
    }



    public void ListItemsByType()
    {
        var itemsByType = _context.Items.OrderBy(i => i.Type); //.OrderBy(i => i.Name);
        //.Select(item => item.Key);

        foreach (var item in itemsByType)
        {
            Console.WriteLine($"Type: {item.Type} #{item.Id},\t Name: {item.Name}, \t Attack Level: {item.Attack},\t Defense Level: {item.Defense}");
        }
    }

    public void SortItems()
    {
        _outputManager.Clear();

        while (true)
        {
            _outputManager.WriteLine("Choose an action:", ConsoleColor.Cyan);
            _outputManager.WriteLine("1. list Items Sorted by Name");
            _outputManager.WriteLine("2. list Items Sorted by Attack Value");
            _outputManager.WriteLine("3. list Items Sorted by Defense value");
            _outputManager.WriteLine("4. Back to Display Items Menu");

            _outputManager.Display();

            var input = Console.ReadLine();


            switch (input)
            {
                case "1":
                    var sortedItems = _context.Items.OrderBy(i => i.Name);
                    Console.WriteLine("Items Sorted by Name");
                    foreach (var item in sortedItems)
                    {
                        Console.WriteLine($"#{item.Id} {item.Name}\tType: {item.Type}\t Attack Level: {item.Attack}\tDefense Level: {item.Defense}");
                    }
                    break;
                case "2":
                    sortedItems = _context.Items.OrderBy(i => i.Attack);
                    Console.WriteLine("Items Sorted by Attack Value");
                    foreach (var item in sortedItems)
                    {
                        Console.WriteLine($"#{item.Id} {item.Name}\tType: {item.Type}\t Attack Level: {item.Attack}\tDefense Level: {item.Defense}");
                    }
                    break;
                case "3":
                    sortedItems = _context.Items.OrderBy(i => i.Defense);
                    Console.WriteLine("Items Sorted by Defense Value");
                    foreach (var item in sortedItems)
                    {
                        Console.WriteLine($"#{item.Id} {item.Name}\tType: {item.Type}\t Attack Level: {item.Attack}\tDefense Level: {item.Defense}");
                    }
                    break;
                case "4":
                    DisplayItems();
                    break;
                default:
                    DisplayItems();
                    break;
            }
        }
    }

    private void AttackCharacter()
    {
        if (_goblin is ITargetable targetableGoblin)
        {
            _player.Attack(targetableGoblin);
            _player.UseAbility(_player.Abilities.First(), targetableGoblin);
        }
    }

    private void SetupGame()
    {
        _player = _context.Players.FirstOrDefault();
        _outputManager.WriteLine($"{_player.Name} has entered the game.", ConsoleColor.Green);

        // Load monsters into random rooms 
        LoadMonsters();

        // Pause before starting the game loop
        Thread.Sleep(200);
        GameLoop();
    }

    private void LoadMonsters()
    {
        _goblin = _context.Monsters.OfType<Goblin>().FirstOrDefault();
    }

}
