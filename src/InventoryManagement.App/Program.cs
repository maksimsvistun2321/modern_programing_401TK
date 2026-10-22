using InventoryManagement.Domain;
using InventoryManagement.Domain.Enums;
using InventoryManagement.Domain.Repository;

var rand = new Random();

InMemoryRepository<Product> productRepository = new();
InMemoryRepository<StockItem> stockItemRepository = new();

Warehouse warehouse = new();

List<string> product_names = ["Laptop", "PC", "Smarphone", "Printer", "Monitor"];
List<string> shorname = ["LAP", "PC", "SMART", "PRINT", "MON"];
List<string> brand = ["Acer", "Asus", "HP", "Lenovo", "Huawei"];
List<string> colors = ["GREY", "BLACK", "WHITE", "BLUE", "RED"];
List<string> zones = ["LOAD", "UNLOAD", "STORE", "PICK", "PACK"];
List<string> shelfs = ["A", "B", "C", "D", "E"] ;

int amount_of_products = 30;
Product[] products = new Product[amount_of_products];

for (int i = 0; i < amount_of_products; i++)
{
    int nameIndex = rand.Next(0, product_names.Count);
    int brandIndex = rand.Next(0, brand.Count);
    int colorIndex = rand.Next(0, colors.Count);

    products[i] = new Product() { 
        Name = $"{product_names[nameIndex]} {brand[brandIndex]} {colors[colorIndex]}",
        SKU = $"{shorname[nameIndex]}-{brand[brandIndex]}-{colors[colorIndex]}-{rand.Next(1, 512)}",
    };

    warehouse.AddProduct(products[i]);
    warehouse.ReceiveStock(products[i], rand.Next(0, 100));

    //issue simulation on random stock item
    int index = rand.Next(0, i);
    var itemToIssue = warehouse.StockItems.First(x => x.Product.Equals(products[index]));
    int itemQuantity = itemToIssue.Quantity;
    int issueQuantity = rand.Next(0, itemQuantity);
    warehouse.IssueStock(products[index], issueQuantity);

    var stockItem = warehouse.StockItems.First(x => x.Product.Equals(products[i]));
    stockItem.AddStorageLocation($"{zones[rand.Next(0, zones.Count)]}", rand.Next(1, 50), $"{shelfs[rand.Next(0, shelfs.Count)]}", rand.Next(1, 10), rand.Next(1, 200));
    stockItem.UpdatePrice(rand.Next(1, 1000));


    productRepository.Add(products[i]);
    stockItemRepository.Add(stockItem);
}

var productsList = productRepository.GetAll();
var stockItemsList = stockItemRepository.GetAll();

Console.WriteLine("Task 1");
var task1 = stockItemsList
    .Where(x => x.Quantity < 5)
    .OrderByDescending(x => x.Quantity)
    .ToList();

foreach (var item in task1)
{
    Console.WriteLine($"{item.Product.Name}, Quantity: {item.Quantity}");
}

Console.WriteLine("\nTask 2");
var task2 = stockItemsList
    .OrderByDescending(x => x.Quantity)
    .Take(5)
    .ToList();

foreach (var item in task2)
{
    Console.WriteLine($"{item.Product.Name}, Quantity: {item.Quantity}");
}

Console.WriteLine("\nTask 3");
var task3 = stockItemsList
    .Sum(x => x.Quantity);

Console.WriteLine($"Total quantity: {task3}");

Console.WriteLine("\nTask 4");
var task4 = stockItemsList
    .Sum(x => x.Price);

Console.WriteLine($"Total price: {task4}");

Console.WriteLine("\nTask 5");
var task5 = stockItemsList
    .GroupBy(
    x => x.Location.Zone,
    (zone, quantity) => new
    {
        Zone = zone,
        Count = quantity.Count(),
    });

foreach (var item in task5)
{
    Console.WriteLine($"Zone: {item.Zone}, quantity per zone: {item.Count}");
}

Console.WriteLine("\nTask 6");
//var task6 = warehouse.Movements.SelectMany();
var task6 = warehouse.Movements;

foreach (var item in task6)
{
    Console.WriteLine(item);
}

Console.WriteLine("\nTask 7");
var task7 = warehouse.Movements
    .Where(x => x.Type == MovementType.Issue)
    .GroupBy(x => x.Product.Name)
    .Select(x => new
    {
        Name = x.Key,
        Count = x.Count()
    })
    .OrderByDescending(x => x.Count)
    .Take(3)
    .ToList();

foreach (var item in task7)
{
    Console.WriteLine(item);
}

Console.WriteLine("\nTask 8");
var issueList = warehouse.Movements
    .Where(x => x.Type == MovementType.Issue)
    .Select(x => x.Product.Name)
    .ToList();

var receiptList = warehouse.Movements
    .Where(x => x.Type == MovementType.Receipt)
    .Select(x => x.Product.Name)
    .ToList();

Console.WriteLine(string.Join("\n", receiptList.Except(issueList)));


Console.WriteLine("\nTask 9");
var task9 = stockItemsList
    .Any(x => x.Status == StockStatus.OutOfStock);

Console.WriteLine("There {0} out-of-stock product.", task9 ? "are" : "are not any");

Console.WriteLine("\nTask 10");
Dictionary<string, int> dictionary = new Dictionary<string, int>(); ;

foreach (var item in stockItemsList)
{
    dictionary.Add($"{item.Product.SKU}", item.Quantity);
}
foreach (var item in dictionary) 
{
    Console.WriteLine(item);
}