namespace InventoryManagement.Domain
{
    public class ReceiptOperation: IStockOperation
    {
        public StockMovement Execute(StockItem item, int quantity)
        {
            item.IncreaseQuantity(quantity);

            return new StockMovement(item.Product, quantity, MovementType.Receipt);
        }
    }
}
