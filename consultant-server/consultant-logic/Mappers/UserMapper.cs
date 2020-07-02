using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consultant_data.Database;
using consultant_data.Models;
using consultant_logic.Mappers;

namespace consultant_data.Mappers
{
    public class UserMapper
    {
        public static User Map(Database.Users user)
        {
            User newUser = new User
            {
                Id = user.Rowid,
                UserId = user.Userid,
                Role = RoleMapper.Map(user.UserroleNavigation),
                Cases = user.Cases.Select(CaseMapper.Map).ToList()
            };

            return newUser;
        }

        public static Database.Users Map(User user)
        {
            return new Database.Users
            {
                Rowid = user.Id,
                Userid = user.UserId,
                Userrole = user.Role.Id
            };
        }
    }
}
