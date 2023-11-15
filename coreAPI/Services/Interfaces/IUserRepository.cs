using System.Collections.Generic;
using System.Threading.Tasks;
using coreAPI.Data;
using coreAPI.Models;


namespace coreAPI.Services
{
    /// <summary>
    /// User Endpoints 
    /// </summary>
    public interface IUserRepository
    {

        bool UserExists(int id);

        Task<IEnumerable<User>> GetUsersAsync();

        Task<User> GetUserAsync(int id);

        Task<IEnumerable<User>> GetUsersByAppAsync(int appID);

        Task UpdateUserAsync(User oUser);

        Task SaveUserAsync(User repo);

        Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest model);

    }
}
