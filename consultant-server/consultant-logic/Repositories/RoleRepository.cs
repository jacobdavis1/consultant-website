using consultant_data.Database;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using consultant_logic.Mappers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_logic.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly khbatlzvContext _context;

        public RoleRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddRole(Role role, bool save = true)
        {
            try
            {
                _context.Roles.Add(RoleMapper.Map(role));

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteRole(Role role, bool save = true)
        {
            try
            {
                Roles dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == role.Id);
                _context.Roles.Remove(dbRole);

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Role> GetRoleById(int id)
        {
            try
            {
                Roles dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == id);

                if (dbRole != null)
                    return RoleMapper.Map(dbRole);

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Role> GetRoleByText(string roleText)
        {
            try
            {
                Roles dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Roletext == roleText);

                if (dbRole != null)
                    return RoleMapper.Map(dbRole);

                return null;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateRole(Role role, bool save = true)
        {
            try
            {
                Roles dbRole = await _context.Roles.FirstOrDefaultAsync(r => r.Roleid == role.Id);
                dbRole.Roletext = role.Text;

                _context.Roles.Update(dbRole);

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
