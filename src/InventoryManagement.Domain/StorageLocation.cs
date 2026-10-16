namespace InventoryManagement.Domain
{
    public record StorageLocation(string Zone, int Aisle, string Shelf, int Level, int Cell)
    {
        public override string ToString()
        {
            return $"{Zone}-{Aisle}-{Shelf}-{Level}-{Cell}";
        }
    };
}
