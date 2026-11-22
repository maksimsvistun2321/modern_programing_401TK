
using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Domain.Services
{
    public class ConsoleStockNotifier : IStockNotifier
    {
        public void Notify(string productName, int issuedQuantity, int remaindedQuantity)
        {
            Console.WriteLine($"Issued {issuedQuantity} units of product {productName}. Remainded quantity: {remaindedQuantity}");
        }
    }
}
