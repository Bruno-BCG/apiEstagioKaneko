using System.Data;
using Microsoft.Data.SqlClient;

namespace restauranteApi
{

    public class SqlConnectionFactory
    {
        private readonly string _cs;
        public SqlConnectionFactory(IConfiguration cfg)
            => _cs = cfg.GetConnectionString("DefaultConnection")!;

        public SqlConnection CreateConnection() => new SqlConnection(_cs);

        public async Task<SqlConnection> OpenConnectionAsync()
        {
            var conn = new SqlConnection(_cs);
            await conn.OpenAsync();
            return conn;
        }
    }

}
