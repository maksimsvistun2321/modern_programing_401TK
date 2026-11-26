using InventoryManagement.Domain.Interfaces;

namespace InventoryManagement.Domain.Services
{
    public class InMemoryStockNotifier : IStockNotifier
    {
        private readonly List<string> _notifications = [];

        public IReadOnlyCollection<string> Notifications => _notifications;

        public void Notify(string productName, int issuedQuantity, int remaindedQuantity)
        {
            _notifications.Add($"Issued {issuedQuantity} units of product {productName}. Remainded quantity: {remaindedQuantity}");
        }
    }
}
