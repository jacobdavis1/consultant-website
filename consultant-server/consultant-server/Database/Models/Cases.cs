using System;
using System.Collections.Generic;

namespace consultant_server.Database
{
    public partial class Cases
    {
        public Cases()
        {
            Appointments = new HashSet<Appointments>();
        }

        public string Caseid { get; set; }
        public string Casetitle { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }
    }
}
