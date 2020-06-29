using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Users
    {
        public Users()
        {
            Caseclient = new HashSet<Caseclient>();
            Cases = new HashSet<Cases>();
        }

        public string Userid { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Caseclient> Caseclient { get; set; }
        public virtual ICollection<Cases> Cases { get; set; }
    }
}
