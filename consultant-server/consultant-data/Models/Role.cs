using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public static Role Consultant { get; } = new Role { Id = 1, Text = "Consultant" };
        public static Role Client { get; } = new Role { Id = 2, Text = "Client" };

        public override bool Equals(Object obj)
        {
            Role other = obj as Role;

            if (other == null) return false;

            return (Id == other.Id && Text == other.Text);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
