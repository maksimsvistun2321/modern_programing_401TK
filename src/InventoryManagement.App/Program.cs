using InventoryManagement.Domain;
using InventoryManagement.Domain.Repository;
using InventoryManagement.Domain.Services;

Warehouse warehouse = new();

Product laptop = new() { Name = "Laptop", SKU = "LAP-ACER-GREY-16" };

warehouse.AddProduct(laptop);
warehouse.ReceiveStock(laptop, 5);

var laptopItem = warehouse.StockItems.First(x => x.Product.Equals(laptop));

Predicate<StockItem> isLowStock = StockItem => StockItem.Quantity > 0 && StockItem.Quantity <= 5;

LowStockNotificationService notificationService = new();
AuditService auditService = new();

laptopItem.StockLevelChanged += stockItem => notificationService.OnStockLevelChanged(stockItem, isLowStock);
laptopItem.StockLevelChanged += auditService.OnStockLevelChanged;

warehouse.IssueStock(laptop, 2);

InMemoryRepository<string> journal = new();
Action<StockItem, string> logger = (stockItem, message) => journal.Add($"[{DateTime.Now:HH:mm::ss}] [{stockItem.Product.Name}] {message}");

laptopItem.AddStorageLocation("PICK", 2, "A", 2, 34);
logger(laptopItem, "Add storage location");

laptopItem.UpdatePrice(59.99m);
logger(laptopItem, "Update price");

Func<StockItem, decimal> StockValue = StockItem => StockItem.Quantity * StockItem.Price;

Console.WriteLine($"\nTotal price of all {laptopItem.Product.Name}s in warehouse: {StockValue(laptopItem)}");
logger(laptopItem, "Calculate total price using Func delegate");

Console.WriteLine($"\nIs {laptopItem.Product.Name} low stock: {laptopItem.IsLowStock()}");
logger(laptopItem, "Use extension method IsLowStock");

Console.WriteLine($"\nSummary for {laptopItem.Product.Name}: \n{laptopItem.GetSummary()}");
logger(laptopItem, "Use extension method GetSummary");

Console.WriteLine();
var logs = journal.GetAll();
foreach(string log in logs)
{
    Console.WriteLine(log);
}