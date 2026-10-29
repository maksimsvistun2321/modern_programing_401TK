using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain
{
    public class StockItem
    {
        public Guid Id { get; }
        public Product Product { get; }
        public int Quantity { get; private set; }
        public int Price { get; private set; }
        public StockStatus Status { get; private set; }
        public StorageLocation? Location { get; private set; }

        public StockItem(Product product, int quantity, int price)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity can't be negative");
            Id = Guid.NewGuid();
            Product = product;
            Quantity = quantity;
            Price = price;

            SetStockStatus();
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount can't be negative");

            Quantity += amount;

            SetStockStatus();
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount can't be negative");

            if (amount > Quantity)
                throw new InvalidOperationException($"We are out of stock of '{Product.Name}'");
            Quantity -= amount;

            SetStockStatus();
        }

        public void AddStorageLocation(string zone, int aisle, string shelf, int level, int cell)
        {
            Location = new StorageLocation(zone, aisle, shelf, level, cell);
        }

        public void UpdatePrice(int price)
        {
            if (price < 0)
                throw new ArgumentException("Price can't be negative");

            Price = price;
        }

        private StockStatus SetStockStatus() =>
            Quantity switch
            {
                0 => Status = StockStatus.OutOfStock,
                >= 1 and <= 5 => Status = StockStatus.LowStock,
                >= 6 and <= 100 => Status =  StockStatus.Normal,
                > 100 => Status = StockStatus.Overstock,
                _ => throw new ArgumentOutOfRangeException($"Not expected negative quantity: {Quantity}"),
            };

        public string GetStockStatusDescription() =>
            Status switch 
            { 
                StockStatus.OutOfStock => $"Item {Product.Name} is out of stock.",
                StockStatus.LowStock => $"Item {Product.Name} is low stock.",
                StockStatus.Normal => $"Item {Product.Name} is normal stock.",
                StockStatus.Overstock => $"Item {Product.Name} is overstock.",
                _ => throw new ArgumentOutOfRangeException($"Not expected stock status: {Status}"),
            };
    }
}
