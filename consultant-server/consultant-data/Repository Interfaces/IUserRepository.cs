using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_data.Models;

namespace consultant_data.RepositoryInterfaces
{
    public interface IUserRepository
    {
        Task<bool> AddUserAsync(User User);

        Task<User> GetUserByIdAsync(Guid UserId);

        Task<List<User>> SearchUsersByNameAsync(string firstName, string middleName, string lastName);

        Task<List<User>> GetAllUsersByCaseAsync(Case targetCase);

        Task<bool> UpdateUserAsync(User User);

        Task<bool> DeleteUserAsync(User User);
    }
}
