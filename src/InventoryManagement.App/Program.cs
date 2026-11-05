using InventoryManagement.Domain;
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

try
{
    warehouse.AddProduct(laptop);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}
try
{
    warehouse.ReceiveStock(laptop, -5);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
try
{
    warehouse.IssueStock(laptop, 10);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}
try
{
    Console.Write("Enter quantity: ");

    int.TryParse(Console.ReadLine(), out int quantity);

    warehouse.ReceiveStock(laptop, quantity);
}
catch (ArgumentOutOfRangeException ex)
{
    Console.WriteLine(ex.Message);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine(ex.Message);
}