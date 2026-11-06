using InventoryManagement.Domain;
using System.Text.Json;
using Microsoft.Extensions.Configuration;

var rand = new Random();

while (true)
{
    Console.WriteLine("\n1. Run app");
    Console.WriteLine("2. Exit");
    Console.Write("Select option: ");

    string? choice = Console.ReadLine();

    if (choice == "2")
    {
        Console.WriteLine("Application closed.");
        break;
    }

    if (choice != "1")
    {
        Console.WriteLine("Invalid option. Try again.");
        continue;
    }

    Warehouse warehouse = new();

    int amount_of_products = 10;
    Product[] products = new Product[amount_of_products];
    StockItem[] stockItems = new StockItem[amount_of_products];

    #region simulation
    List<string> product_names = ["Laptop", "PC", "Smarphone", "Printer", "Monitor"];
    List<string> shorname = ["LAP", "PC", "SMART", "PRINT", "MON"];
    List<string> brand = ["Acer", "Asus", "HP", "Lenovo", "Huawei"];
    List<string> colors = ["GREY", "BLACK", "WHITE", "BLUE", "RED"];
    List<string> zones = ["LOAD", "UNLOAD", "STORE", "PICK", "PACK"];
    List<string> shelfs = ["A", "B", "C", "D", "E"];

    for (int i = 0; i < amount_of_products; i++)
    {
        int nameIndex = rand.Next(0, product_names.Count);
        int brandIndex = rand.Next(0, brand.Count);
        int colorIndex = rand.Next(0, colors.Count);

        products[i] = new Product()
        {
            Name = $"{product_names[nameIndex]} {brand[brandIndex]} {colors[colorIndex]}",
            SKU = $"{shorname[nameIndex]}-{brand[brandIndex]}-{colors[colorIndex]}-{rand.Next(1, 512)}",
        };

        warehouse.AddProduct(products[i]);
        warehouse.ReceiveStock(products[i], rand.Next(0, 100));

        int index = rand.Next(0, i);
        var itemToIssue = warehouse.StockItems.First(x => x.Product.Equals(products[index]));
        int itemQuantity = itemToIssue.Quantity;
        int issueQuantity = rand.Next(0, itemQuantity);
        warehouse.IssueStock(products[index], issueQuantity);

        var stockItem = warehouse.StockItems.First(x => x.Product.Equals(products[i]));
        stockItem.AddStorageLocation($"{zones[rand.Next(0, zones.Count)]}", rand.Next(1, 50), $"{shelfs[rand.Next(0, shelfs.Count)]}", rand.Next(1, 10), rand.Next(1, 200));
        stockItem.UpdatePrice((decimal)rand.Next(1, 1000));

        stockItems[i] = stockItem;
    }
    #endregion

    string filePath = "";
    try
    {
        string configFile = Path.Combine(AppContext.BaseDirectory, "appsettings.json");

        var config = new ConfigurationBuilder()
            .AddJsonFile(configFile, optional: false)
            .Build();

        string relativepath = config["Storage:FilePath"] ?? throw new InvalidOperationException("Storage:FilePath is not configured.");

        filePath = Path.Combine(AppContext.BaseDirectory, relativepath);

        Func<StockItem, decimal> StockValue = StockItem => StockItem.Quantity * StockItem.Price;

        List<StockReport> report = [];

        foreach (var item in stockItems)
        {
            report.Add(new StockReport(item.Product.SKU, item.Product.Name, item.Location.Zone, item.Quantity, StockValue(item)));
        }

        var options = new JsonSerializerOptions
        {
            WriteIndented = config.GetValue<bool?>("Storage:WriteIndented") ?? throw new InvalidOperationException("Storage:WriteIndented is not configured.")
        };

        string json = JsonSerializer.Serialize(report, options);

        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
        await File.WriteAllTextAsync(filePath, json);

        string savedJson = await File.ReadAllTextAsync(filePath);
        var deserealized = JsonSerializer.Deserialize<List<StockReport>>(savedJson);
        decimal totalPrice = 0;
        foreach (var item in deserealized)
        {
            totalPrice += item.StockValue;
            Console.WriteLine(item);
        }

        Console.WriteLine($"\nTotal price: {totalPrice}");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
    catch (FileNotFoundException)
    {
        Console.WriteLine($"File '{filePath}' not found.");
    }
    catch (JsonException ex)
    {
        Console.WriteLine($"Invalid JSON format: {ex.Message}");
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}
