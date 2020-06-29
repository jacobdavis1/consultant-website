using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using consultant_data.Database;
using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class UserMapper
    {
        public static User Map(Database.Users user)
        {
            User newUser = new User
            {
                Id = Guid.Parse(user.Userid),
                FirstName = user.Firstname,
                MiddleName = user.Middlename,
                LastName = user.Lastname,
                Email = user.Email,
                Cases = user.Cases.Select(CaseMapper.Map).ToList()
            };

            return newUser;
        }

        public static Database.Users Map(User User)
        {
            return new Database.Users
            {
                Userid = User.Id.ToString(),
                Firstname = User.FirstName,
                Middlename = User.MiddleName,
                Lastname = User.LastName,
                Email = User.Email
            };
        }
    }
}
