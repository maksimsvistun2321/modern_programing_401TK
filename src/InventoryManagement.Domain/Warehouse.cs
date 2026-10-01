namespace InventoryManagement.Domain
{
    public class Warehouse
    {
        private readonly List<StockItem> _stockItems = new();
        private readonly List<StockMovement> _movements = new();

        public IReadOnlyCollection<StockItem> StockItems => _stockItems;
        public IReadOnlyCollection<StockMovement> Movements => _movements;

        public void AddProduct(Product product)
        {
            var existingItem = _stockItems.FirstOrDefault(item => item.Product.Equals(product));
            if (existingItem == null)
            {
                _stockItems.Add(new StockItem(product, 0));
            }
        }
    }
}
