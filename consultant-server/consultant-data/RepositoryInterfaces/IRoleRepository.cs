using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_data.RepositoryInterfaces
{
    public interface IRoleRepository
    {
        public Task<bool> AddRole(Role role, bool save = true);

        public Task<Role> GetRoleById(int id);

        public Task<Role> GetRoleByText(string roleText);

        public Task<bool> UpdateRole(Role role, bool save = true);

        public Task<bool> DeleteRole(Role role, bool save = true);
    }
}
