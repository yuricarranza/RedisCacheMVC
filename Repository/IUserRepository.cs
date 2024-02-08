using RedisTestWebMVC.Models;

namespace RedisTestWebMVC.Repository
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsersAsync();
    }
}
