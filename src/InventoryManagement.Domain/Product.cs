namespace InventoryManagement.Domain
{
    public class Product
    {
        public Guid Id { get; }
        public string Name { get; }

        public Product(string name)
        {
            Id = Guid.NewGuid();
            Name = name;
        }
    }
}
