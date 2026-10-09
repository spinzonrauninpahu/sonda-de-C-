using Microsoft.Data.SqlClient;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;

namespace Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;
        private readonly ISqlExecutor _sqlExecutor;

        public OrderRepository(string connectionString) : this(connectionString, new SqlExecutor()) { }

        public OrderRepository(string connectionString, ISqlExecutor sqlExecutor)
        {
            _connectionString = connectionString;
            _sqlExecutor = sqlExecutor;
        }

        public void Create(Order order)
        {
            const string sql = """
            INSERT INTO Orders (Id, Customer, Product, Qty, Price)
            VALUES (@Id, @Customer, @Product, @Qty, @Price)
            """;

            SqlParameter[] parameters =
            [
                new("@Id", order.Id),
                new("@Customer", order.CustomerName),
                new("@Product", order.ProductName),
                new("@Qty", order.Quantity),
                new("@Price", order.UnitPrice)
            ];

            _sqlExecutor.Execute(_connectionString, sql, parameters);
        }
    }
}
