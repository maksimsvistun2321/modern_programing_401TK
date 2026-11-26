using InventoryManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace InventoryManagement.Domain.Services
{
    public sealed class StockApplicationService(
        IStockNotifier notifier,
        ILogger<StockApplicationService> logger
        )
    {
        public void IssueStock(Warehouse warehouse, Product product, int quantity)
        {
            logger.LogInformation($"Issue operation of {quantity} units from {product.Name} started");

            var stockItem = warehouse.StockItems.FirstOrDefault(item => item.Product.Equals(product));

            if (stockItem is null)
            {
                logger.LogWarning($"Product {product.Name} was not found in warehouse.");
                return;
            }

            try
            {
                warehouse.IssueStock(product, quantity);
                int remaindedQuantity = stockItem.Quantity;
                notifier.Notify(product.Name, quantity, remaindedQuantity);
                logger.LogInformation($"Issued {quantity} units of product {product.Name}. Remainded quantity: {remaindedQuantity}");
            }
            catch(Exception ex)
            {
                logger.LogWarning(ex.Message);
            }
        }
    }
}
