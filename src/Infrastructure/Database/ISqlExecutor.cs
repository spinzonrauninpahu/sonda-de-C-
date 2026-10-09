using Microsoft.Data.SqlClient;

namespace Infrastructure.Database;

public interface ISqlExecutor
{
    void Execute(string connectionString, string sql, SqlParameter[] parameters);
}
