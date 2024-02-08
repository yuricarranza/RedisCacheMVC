using Oracle.ManagedDataAccess.Client;
using RedisTestWebMVC.Models;

namespace RedisTestWebMVC.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<List<User>> GetUsersAsync()
        {
            List<User> users = new();
            using (OracleConnection connection = new OracleConnection("User Id=tiendas_adm;Password=TDATPSA;Data Source=10.20.11.167:1525/PMM; POOLING=FALSE;PERSIST SECURITY INFO=TRUE"))
            {
                await connection.OpenAsync();
                OracleCommand command = new OracleCommand("select firstname, lastname from users_prueba", connection);
                OracleDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    users.Add(new User
                    {
                        FirstName = reader.GetString(0),
                        LastName = reader.GetString(1)
                    });
                }
            }

            return users;
        }
    }
}
