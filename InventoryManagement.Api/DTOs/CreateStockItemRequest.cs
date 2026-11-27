namespace InventoryManagement.Api.DTOs
{
    public class CreateStockItemRequest
    {
        public string Name { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public string? Description {  get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
