using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain
{
    public static class StockItemExtensions
    {
        public static bool IsLowStock(this StockItem item) 
        {
            return item.Status == StockStatus.LowStock;
        }

        public static string GetSummary(this StockItem item)
        {
            return $"Name: {item.Product.Name}\nSKU: {item.Product.SKU}\nDescription: {item.Product.Description}\nPrice: {item.Price}\nQuantity: {item.Quantity}\nStatus: {item.GetStockStatusDescription()}\nLocation: {item.Location}";
        }
    }
}
