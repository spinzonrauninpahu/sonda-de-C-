using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Xunit;

namespace ARQ_lim.Tests.Infrastructure;

public class OrderRepositoryTests
{
    [Fact]
    public void Create_ValidOrder_ExecutesInsertWithExpectedParameters()
    {
        var executor = new FakeSqlExecutor();
        var repository = new OrderRepository("fake-connection-string", executor);

        var order = new Order
        {
            Id = 123,
            CustomerName = "Carlos",
            ProductName = "Teclado",
            Quantity = 2,
            UnitPrice = 50m
        };

        repository.Create(order);

        Assert.Contains("INSERT INTO Orders", executor.Sql);
        Assert.Equal("fake-connection-string", executor.ConnectionString);
        Assert.Equal(5, executor.Parameters.Length);
        Assert.Equal(123, executor.Parameters[0].Value);
        Assert.Equal("Carlos", executor.Parameters[1].Value);
        Assert.Equal("Teclado", executor.Parameters[2].Value);
        Assert.Equal(2, executor.Parameters[3].Value);
        Assert.Equal(50m, executor.Parameters[4].Value);
    }

    private sealed class FakeSqlExecutor : ISqlExecutor
    {
        public string? ConnectionString { get; private set; }
        public string? Sql { get; private set; }
        public SqlParameter[] Parameters { get; private set; } = [];

        public void Execute(
            string connectionString,
            string sql,
            SqlParameter[] parameters)
        {
            ConnectionString = connectionString;
            Sql = sql;
            Parameters = parameters;
        }
    }

}