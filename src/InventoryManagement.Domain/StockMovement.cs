namespace InventoryManagement.Domain
{
    public class StockMovement
    {
        public Guid Id { get; }
        public DateTime Timestamp { get; }
        public Product Product { get; }
        public int Quantity { get; }
        public MovementType Type { get; }

        public StockMovement(Product product, int quantity, MovementType type)
        {
            if (quantity < 0) throw new ArgumentException("Quantity can't be negative!");

            Id = Guid.NewGuid();
            Timestamp = DateTime.UtcNow;
            Product = product;
            Quantity = quantity;
            Type = type;
        }

        public override string ToString()
        {
            return $"[{Timestamp:yyyy-MM-dd HH:mm:ss}] {Type}: {Product.Name} x{Quantity}";
        }
    }

    public enum MovementType
    {
        Receipt,
        Issue
    }
}
