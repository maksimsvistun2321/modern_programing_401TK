using InventoryManagement.Domain.Enums;

namespace InventoryManagement.Domain
{
    public class StockItem
    {
        public Guid Id { get; }
        public Product Product { get; }
        public int Quantity { get; private set; }
        public StorageLocation? Location { get; private set; }

        public StockItem(Product product, int quantity)
        {
            if (quantity < 0)
                throw new ArgumentException("Quantity can't be negative");
            Id = Guid.NewGuid();
            Product = product;
            Quantity = quantity;
        }

        public void IncreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount can't be negative");

            Quantity += amount;
        }

        public void DecreaseQuantity(int amount)
        {
            if (amount < 0)
                throw new ArgumentException("Amount can't be negative");

            if (amount > Quantity)
                throw new InvalidOperationException($"We are out of stock of '{Product.Name}'");
            Quantity -= amount;
        }

        public void AddStorageLocation(StorageLocation location)
        {
            if (location == null)
                throw new ArgumentNullException($"Location can't be null");
            Location = location;
        }
    }
}
