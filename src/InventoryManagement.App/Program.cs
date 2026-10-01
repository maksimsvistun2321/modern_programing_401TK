using InventoryManagement.Domain;

Warehouse warehouse = new Warehouse();

Product laptop = new Product("Laptop");

warehouse.AddProduct(laptop);
Console.WriteLine("Laptop added.");

warehouse.ReceiveStock(laptop, 10);
Console.WriteLine("Receive 10 units.");

warehouse.IssueStock(laptop, 3);
Console.WriteLine("Issue 3 units.");

var laptopItem = warehouse.StockItems.First(x => x.Product.Equals(laptop));

Console.WriteLine("Stock: " + laptopItem.Product.Name);
Console.WriteLine("Quantity: " + laptopItem.Quantity);

Console.WriteLine("\nMovement history:");

foreach (var movement in warehouse.Movements)
{
    Console.WriteLine(movement);
}

Console.WriteLine("\nBoundary test cases");
try
{
    Console.WriteLine("Trying to issue 100 laptops");
    warehouse.IssueStock(laptop, 100);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
try
{
    Console.WriteLine("Trying to receive negative amount of laptops");
    warehouse.ReceiveStock(laptop, -100);
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

Product smartphone = new Product("Smartphone");
warehouse.AddProduct(smartphone);
warehouse.AddProduct(smartphone);

Console.WriteLine("\nStocks in warehouse:");

foreach (var item in warehouse.StockItems)
{
    Console.WriteLine(item.Product.Name);
}
Console.WriteLine("No duplicates are created");


Console.WriteLine("\nInterface variables usage");

Product pc = new Product("PC");

warehouse.AddProduct(pc);
Console.WriteLine("PC added");

var pcItem = warehouse.StockItems.First(x => x.Product.Equals(pc));

IStockOperation operation = new ReceiptOperation();
var receipt = operation.Execute(pcItem, 5);
Console.WriteLine(receipt);

operation = new IssueOperation();
var issue = operation.Execute(pcItem, 2);
Console.WriteLine(issue);

Console.WriteLine("Stock: " + pcItem.Product.Name);
Console.WriteLine("Quantity: " + pcItem.Quantity);