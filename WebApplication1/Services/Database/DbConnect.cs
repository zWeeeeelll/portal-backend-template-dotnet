using System.Data.SqlClient;
using System.Data;

namespace portal.Database
{
    public class DbConnect : IDisposable
    {
        public IDbConnection Connection { get; set; }
        public DbConnect(IConfiguration configuration)
        {
            Connection = new SqlConnection(configuration.GetConnectionString("DefaultConnection"));

            Connection.Open();
        }

        public void Dispose() => Connection?.Dispose();
    }
}
