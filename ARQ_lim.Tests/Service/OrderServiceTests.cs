using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Services;
using Xunit;

namespace ARQ_lim.Tests.Services;

public class OrderServiceTests
{
    [Fact]
    public void CreateOrder_ValidData_ReturnsOrderWithExpectedData()
    {
        string customer = "Carlos";
        string product = "Teclado";
        int quantity = 2;
        decimal price = 50m;

        var result = OrderService.CreateOrder(
            customer, product, quantity, price);

        Assert.Equal(customer, result.CustomerName);
        Assert.Equal(product, result.ProductName);
        Assert.Equal(quantity, result.Quantity);
        Assert.Equal(price, result.UnitPrice);
        Assert.InRange(result.Id, 1, 9999998);
    }

    [Fact]
    public void CreateOrder_CreatesOrder_AddsItToLastOrders()
    {
        var result = OrderService.CreateOrder(
            "Carlos", "Teclado", 2, 50m);

        Assert.Contains(result, OrderService.LastOrders);
    }
}