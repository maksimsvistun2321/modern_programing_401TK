using InventoryManagement.Domain;

namespace InventoryManagment.Tests;

public class InventoryManagmentTests
{
    [Fact]
    public void IncreaseQuantity_ReturnCorectQuantity()
    {
        var product = new Product() { Name = "Laptop", SKU = "LAP-BLK-512GB" };

        var stockItem = new StockItem(product, 10, 500m);

        stockItem.IncreaseQuantity(5);

        Assert.Equal(15, stockItem.Quantity);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-3)]
    public void IncreaseQuantity_InvalidAmount_ThrowsException(int amount)
    {
        var product = new Product() { Name = "Laptop", SKU = "LAP-BLK-512GB" };
        var item = new StockItem(product, 10, 500m);

        Assert.Throws<ArgumentOutOfRangeException>(
            () => item.IncreaseQuantity(amount)
        );

        Assert.Equal(10, item.Quantity);
    }

    [Fact]
    public void DecreaseQuantity_AmountExceedsStock_ThrowsException()
    {
        var product = new Product() { Name = "Laptop", SKU = "LAP-BLK-512GB" };
        var item = new StockItem(product, 10, 500m);

        Assert.Throws<InvalidOperationException>(
            () => item.DecreaseQuantity(15)
        );

        Assert.Equal(10, item.Quantity);
    }

    [Fact]
    public void DecreaseQuantity_InsufficientStock_DoesNotRaiseEvent()
    {
        var product = new Product() { Name = "Laptop", SKU = "LAP-BLK-512GB" };
        var item = new StockItem(product, 10, 500m);

        bool eventRaised = false;

        item.StockLevelChanged += (sender) =>
        {
            eventRaised = true;
        };

        Assert.Throws<InvalidOperationException>(
            () => item.DecreaseQuantity(15)
        );

        Assert.False(eventRaised);
    }

    [Fact]
    public void IncreaseQuantity_TwoSubscribers_BothReceiveNotification()
    {
        var product = new Product() { Name = "Laptop", SKU = "LAP-BLK-512GB" };
        var item = new StockItem(product, 10, 500m);

        int firstSubscriber = 0;
        int secondSubscriber = 0;

        item.StockLevelChanged += (sender) =>
        {
            firstSubscriber++;
        };

        item.StockLevelChanged += (sender) =>
        {
            secondSubscriber++;
        };

        item.IncreaseQuantity(5);

        Assert.Equal(1, firstSubscriber);
        Assert.Equal(1, secondSubscriber);
    }
}
