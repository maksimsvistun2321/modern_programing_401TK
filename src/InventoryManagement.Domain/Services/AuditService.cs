namespace InventoryManagement.Domain.Services
{
    public class AuditService
    {
        public void OnStockLevelChanged(StockItem stockItem)
        {
            Console.WriteLine($"{stockItem.Product.Name} stock changed, current quantity: {stockItem.Quantity}");
        }
    }
}