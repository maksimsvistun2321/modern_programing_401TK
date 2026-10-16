namespace InventoryManagement.Domain.Interfaces
{
    public interface IStockOperation
    {
        StockMovement Execute(StockItem item, int quantity);
    }
}