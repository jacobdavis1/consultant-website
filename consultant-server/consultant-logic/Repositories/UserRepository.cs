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

        public async Task<User> AddUserAsync(User user, bool save = true)
        {
            try
            {
                Users dbUser = _context.Users.Add(UserMapper.Map(user)).Entity;

                if (save)
                    await _context.SaveChangesAsync();

                return UserMapper.Map(dbUser);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            try
            {
                Users dbUser = await _context.Users
                    .Include(u => u.Cases).ThenInclude(c => c.Appointments)
                    .Include(u => u.Cases).ThenInclude(c => c.Casenotes)
                    .Include(u => u.Cases).ThenInclude(c => c.Activeconsultant)
                    .Include(U => U.Cases).ThenInclude(c => c.Currentstatus)
                    .Include(u => u.UserroleNavigation)
                    .FirstOrDefaultAsync(u => u.Userid == userId);

                if (dbUser == null)
                    return null;

                return UserMapper.Map(dbUser);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<User> GetUserByRowIdAsync(int rowId)
        {
            try
            {
                Users dbUser = await _context.Users
                    .Include(u => u.Cases).ThenInclude(c => c.Appointments)
                    .Include(u => u.Cases).ThenInclude(c => c.Casenotes)
                    .Include(u => u.Cases).ThenInclude(c => c.Activeconsultant)
                    .Include(U => U.Cases).ThenInclude(c => c.Currentstatus)
                    .Include(u => u.UserroleNavigation)
                    .FirstOrDefaultAsync(u => u.Rowid == rowId);

                if (dbUser == null)
                    return null;

                return UserMapper.Map(dbUser);
            }
            catch (Exception e)
            {
                return null;
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

        public async Task<User> UpdateUserAsync(User user, bool save = true)
        {
            try
            {
                // Update the User
                Users dbUser = _context.Users.Update(await _context.Users.FirstOrDefaultAsync(c => c.Rowid == user.Id)).Entity;

                if (save)
                    await _context.SaveChangesAsync();

                return UserMapper.Map(dbUser);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> DeleteUserAsync(User user, bool save = true)
        {
            try
            {
                // Remove all caseclient entries
                foreach (Case c in user.Cases)
                {
                    // Select the caseclient with the user's id and the case id pertaining to this case
                    Caseclient cc = await _context.Caseclient.FirstOrDefaultAsync(cc => cc.Clientid == user.Id && cc.Caseid == c.Id);
                    if (cc != null)
                        _context.Caseclient.Remove(cc);
                }

                // Finally, remove the user
                _context.Users.Remove(await _context.Users.FirstOrDefaultAsync(c => c.Rowid == user.Id));

                if (save)
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
