using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_data.Models;

namespace consultant_data.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<User> AddUserAsync(User user, bool save = true);

        Task<User> GetUserByIdAsync(string userId);

        Task<User> GetUserByRowIdAsync(int rowId);

        Task<List<User>> GetAllUsersByCaseAsync(Case targetCase);

        Task<User> UpdateUserAsync(User user, bool save = true);

        Task<bool> DeleteUserAsync(User user, bool save = true);
    }
}
