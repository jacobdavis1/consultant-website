using consultant_data.Database;
using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_logic.Mappers
{
    class RoleMapper
    {
        public static Role Map(Roles role)
        {
            return new Role
            {
                Id = role.Roleid,
                Text = role.Roletext
            };
        }

        public static Roles Map(Role role)
        {
            return new Roles
            {
                Roleid = role.Id,
                Roletext = role.Text
            };
        }
    }
}
