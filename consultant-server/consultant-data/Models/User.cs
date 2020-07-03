using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Role Role { get; set; }

        public List<Case> Cases { get; set; } = new List<Case>();

        public override bool Equals(object obj)
        {
            User other = obj as User;

            if (other == null) return false;

            return (Id == other.Id && UserId == other.UserId
                        && Role.Equals(other.Role));
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
