namespace InventoryManagement.Domain.Services
{
    public class LowStockNotificationService
    {
        public void OnStockLevelChanged(StockItem stockItem, Predicate<StockItem> isLowStock)
        {
            if (isLowStock(stockItem))
            {
                Console.WriteLine($"{stockItem.Product.Name} is low stock, current quantity: {stockItem.Quantity}");
            }
        }
    }
}