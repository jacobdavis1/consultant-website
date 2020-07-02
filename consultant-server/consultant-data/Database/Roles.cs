using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Roles
    {
        public Roles()
        {
            Users = new HashSet<Users>();
        }

        public int Roleid { get; set; }
        public string Roletext { get; set; }

        public virtual ICollection<Users> Users { get; set; }
    }
}
