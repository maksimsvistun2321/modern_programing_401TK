namespace InventoryManagement.Domain
{
    public sealed record StockReport(string SKU, string ProductName, string Zone, int Quantity, decimal StockValue);
}
