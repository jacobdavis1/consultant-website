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

        public int Rowid { get; set; }
        public string Userid { get; set; }
        public int Userrole { get; set; }

        public virtual Roles UserroleNavigation { get; set; }
        public virtual ICollection<Caseclient> Caseclient { get; set; }
        public virtual ICollection<Cases> Cases { get; set; }
    }
}
