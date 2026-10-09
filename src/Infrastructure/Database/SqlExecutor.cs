using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace Infrastructure.Database
{
    public class SqlExecutor : ISqlExecutor
    {
        public void Execute(string connectionString, string sql, SqlParameter[] parameters)
        {
            using var conn = new SqlConnection(connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddRange(parameters);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}