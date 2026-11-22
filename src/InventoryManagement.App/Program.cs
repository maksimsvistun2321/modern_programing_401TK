using InventoryManagement.Domain;
using InventoryManagement.Domain.Interfaces;
using InventoryManagement.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddSingleton<IStockNotifier, ConsoleStockNotifier>();
//builder.Services.AddSingleton<InMemoryStockNotifier>();
//builder.Services.AddSingleton<IStockNotifier>(sp => sp.GetRequiredService<InMemoryStockNotifier>());
builder.Services.AddTransient<StockApplicationService>();

using IHost host = builder.Build();
var service = host.Services.GetRequiredService<StockApplicationService>();
var inMemoryService = host.Services.GetRequiredService<InMemoryStockNotifier>(); 

Warehouse warehouse = new Warehouse();
Product laptop = new Product() { Name = "Laptop", SKU = "LAP-ASUS-BLK-512GB" };

warehouse.AddProduct(laptop);
warehouse.ReceiveStock(laptop, 10);

service.IssueStock(warehouse, laptop, 4);

service.IssueStock(warehouse, laptop, 25);

//foreach(var item in inMemoryService.Notifications)
//{
//    Console.WriteLine(item);
//}