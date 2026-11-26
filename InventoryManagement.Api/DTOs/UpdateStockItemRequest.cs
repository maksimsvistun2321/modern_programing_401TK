namespace InventoryManagement.Api.DTOs
{
    public class UpdateStockItemRequest
    {
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}