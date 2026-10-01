namespace InventoryManagement.Domain
{
    public interface IStockOperation
    {
        StockMovement Execute(StockItem item, int quantity);
    }
}
