using InventoryManagement.Domain;

Warehouse warehouse = new Warehouse();

Product laptop = new() { Name = "Laptop", SKU = "LAP-ACER-GREY-16" };

warehouse.AddProduct(laptop);
warehouse.ReceiveStock(laptop, 10);

var laptopItem = warehouse.StockItems.First(x => x.Product.Equals(laptop));

laptopItem.AddStorageLocation("PICK", 2, "A", 2, 34);

Console.WriteLine($"{laptopItem.Product.Name} is stored in {laptopItem.Location}");

Console.WriteLine($"Laptop stock status: {laptopItem.Status}. Quantity: {laptopItem.Quantity}");
Console.WriteLine(laptopItem.GetStockStatusDescription());

warehouse.ReceiveStock(laptop, 100);
Console.WriteLine($"Laptop stock status: {laptopItem.Status}. Quantity: {laptopItem.Quantity}");
Console.WriteLine(laptopItem.GetStockStatusDescription());

warehouse.IssueStock(laptop, 110);
Console.WriteLine($"Laptop stock status: {laptopItem.Status}. Quantity: {laptopItem.Quantity}");
Console.WriteLine(laptopItem.GetStockStatusDescription());