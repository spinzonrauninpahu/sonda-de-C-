using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Application.Interfaces;
using Application.UseCases;
using Domain.Entities;
using Xunit;

namespace ARQ_lim.Tests.UseCases;

public class CreateOrderTests
{
    [Fact]
    public void Execute_ValidData_CreatesAndReturnsOrder()
    {
        var repository = new FakeOrderRepository();
        var useCase = new CreateOrder(repository);

        var result = useCase.Execute("Carlos", "Teclado", 2, 50m);

        Assert.NotNull(result);
        Assert.Equal("Carlos", result.CustomerName);
        Assert.Equal("Teclado", result.ProductName);
        Assert.Equal(2, result.Quantity);
        Assert.Equal(50m, result.UnitPrice);
    }

    [Fact]
    public void Execute_ValidData_SavesOrderInRepository()
    {
        var repository = new FakeOrderRepository();
        var useCase = new CreateOrder(repository);

        var result = useCase.Execute("Carlos", "Teclado", 2, 50m);

        Assert.Same(result, repository.SavedOrder);
    }

    private class FakeOrderRepository : IOrderRepository
    {
        public Order? SavedOrder { get; private set; }

        public void Create(Order order)
        {
            SavedOrder = order;
        }
    }
}