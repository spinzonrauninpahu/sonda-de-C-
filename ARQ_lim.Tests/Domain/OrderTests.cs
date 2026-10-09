using Domain.Entities;
using Xunit;

namespace ARQ_lim.Tests.Domain.Entities;

public class OrderTests
{
    [Fact]
    public void CalculateTotal_MultipliesQuantityByUnitPrice()
    {
        var order = new Order
        {
            Id = 1,
            CustomerName = "Carlos",
            ProductName = "Teclado",
            Quantity = 2,
            UnitPrice = 50m
        };

        var total = order.CalculateTotal();

        Assert.Equal(100m, total);
    }

    [Fact]
    public void CalculateTotal_WhenQuantityIsZero_ReturnsZero()
    {
        var order = new Order
        {
            Quantity = 0,
            UnitPrice = 50m
        };

        Assert.Equal(0m, order.CalculateTotal());
    }

}