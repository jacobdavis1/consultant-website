using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using consultant_data.Database;
using consultant_data.Mappers;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;

namespace consultant_logic.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly khbatlzvContext _context;

        public UserRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(UserMapper.Map(user));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<User> GetUserByIdAsync(Guid userId)
        {
            try
            {
                Users dbUser = await _context.Users
                    .Include(u => u.Cases).ThenInclude(c => c.Appointments)
                    .Include(u => u.Cases).ThenInclude(c => c.Casenotes)
                    .FirstOrDefaultAsync(u => u.Userid == userId.ToString());

                if (dbUser == null)
                    return null;

                return UserMapper.Map(dbUser);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<User>> SearchUsersByNameAsync(string firstName, string middleName, string lastName)
        {
            try
            {
                return _context.Users
                    .Include(u => u.Cases)
                    .Where(c => (c.Firstname == firstName || firstName == "")
                        && (c.Middlename == middleName || middleName == "")
                        && (c.Lastname == lastName || lastName == ""))
                    .Select(UserMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<User>();
            }
        }

        public async Task<List<User>> GetAllUsersByCaseAsync(Case targetCase)
        {
            throw new NotImplementedException();
        }

        /* public async Task<List<User>> GetAllUsersByCaseAsync(Case targetCase)
        {
            try
            {
                return await _context.Cases.FindAsync(targetCase.Id.ToString())
                    .Result
                    .Users
                    .Select(UserMapper.MapUser)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<User>();
            }
        } */

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                // Update the User
                _context.Users.Update(await _context.Users.FirstOrDefaultAsync(c => c.Userid == user.Id.ToString()));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(User user)
        {
            try
            {
                // Remove all caseclient entries
                foreach (Case c in user.Cases)
                {
                    // Select the caseclient with the user's id and the case id pertaining to this case
                    Caseclient cc = await _context.Caseclient.FirstOrDefaultAsync(cc => cc.Clientid == user.Id.ToString() && cc.Caseid == c.Id.ToString());
                    if (cc != null)
                        _context.Caseclient.Remove(cc);
                }

                // Finally, remove the user
                _context.Users.Remove(await _context.Users.FirstOrDefaultAsync(c => c.Userid == user.Id.ToString()));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
