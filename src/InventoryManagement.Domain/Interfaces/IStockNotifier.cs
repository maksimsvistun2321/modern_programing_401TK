namespace InventoryManagement.Domain.Interfaces
{
    public interface IStockNotifier
    {
        public void Notify(string productName, int issuedQuantity, int remaindedQuantity);
    }
}
