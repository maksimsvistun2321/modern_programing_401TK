namespace InventoryManagement.Domain
{
    public class Product
    {
        public Guid Id { get; }
        public required string Name { get; init; }
        public required string SKU { get; init; } //unique alphanumeric code, e.g. NKE-AM-BLK-10
        public string? Description { get; private set; }

        public Product()
        {
            Id = Guid.NewGuid();
            Name = "Unknown name";
            SKU = "Unknown SKU";
            Description = "Unknown description";
        }
        public Product(string name, string sku)
        {
            Id = Guid.NewGuid();
            Name = name;
            SKU = sku;
        }
        public Product(string name, string sku, string description)
        {
            Id = Guid.NewGuid();
            Name = name;
            SKU = sku;
            Description = description;
        }

        public void UpdateDescription(string newDescription)
        {
            Description = newDescription;
        }
    }
}
