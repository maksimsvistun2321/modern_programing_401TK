using InventoryManagement.Domain.Interfaces;

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
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            if (string.IsNullOrWhiteSpace(product.SKU))
                throw new ArgumentException("SKU cannot be empty");

            var existingItem = _stockItems.FirstOrDefault(item => item.Product.Equals(product));

            if (existingItem != null)
                throw new InvalidOperationException("Item already exists");

            _stockItems.Add(new StockItem(product, 0, 0));
        }

        public void ReceiveStock(Product product, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException("Quantity can't be negative");

            var item = _stockItems.First(item => item.Product.Equals(product));
            
            if (item == null)
                throw new InvalidOperationException("Product does not exist");

            IStockOperation operation = new ReceiptOperation();

            _movements.Add(operation.Execute(item, quantity));
        }

        public void IssueStock(Product product, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentOutOfRangeException("Quantity can't be negative");

            var item = _stockItems.First(item => item.Product.Equals(product));

            if (item == null)
                throw new InvalidOperationException("Product does not exist");

            IStockOperation operation = new IssueOperation();

            _movements.Add(operation.Execute(item, quantity));
        }
    }
}
