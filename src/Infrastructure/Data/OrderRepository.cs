using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Application.Interfaces;
using Domain.Entities;


namespace Infrastructure.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Create(Order order)
        {
            const string sql = """
            INSERT INTO Orders (Id, Customer, Product, Qty, Price)
            VALUES (@Id, @Customer, @Product, @Qty, @Price)
            """;

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Id", order.Id);
            cmd.Parameters.AddWithValue("@Customer", order.CustomerName);
            cmd.Parameters.AddWithValue("@Product", order.ProductName);
            cmd.Parameters.AddWithValue("@Qty", order.Quantity);
            cmd.Parameters.AddWithValue("@Price", order.UnitPrice);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
