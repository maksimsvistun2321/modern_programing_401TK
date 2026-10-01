namespace InventoryManagement.Domain
{
    public class IssueOperation : IStockOperation
    {
        public StockMovement Execute(StockItem item, int quantity)
        {
            item.DecreaseQuantity(quantity);

            return new StockMovement(item.Product, quantity, MovementType.Issue);
        }
    }
}
