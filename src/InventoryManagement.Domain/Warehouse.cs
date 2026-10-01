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

        public void ReceiveStock(Product product, int quantity)
        {
            var item = _stockItems.First(item => item.Product.Equals(product));

            IStockOperation operation = new ReceiptOperation();

            _movements.Add(operation.Execute(item, quantity));
        }

        public void IssueStock(Product product, int quantity)
        {
            var item = _stockItems.First(item => item.Product.Equals(product));

            IStockOperation operation = new IssueOperation();

            _movements.Add(operation.Execute(item, quantity));
        }
    }
}
