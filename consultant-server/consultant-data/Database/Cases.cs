using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Cases
    {
        public Cases()
        {
            Appointments = new HashSet<Appointments>();
        }

        public string Caseid { get; set; }
        public string Casetitle { get; set; }
        public string Activeconsultantid { get; set; }
        public string Currentstatusid { get; set; }

        public virtual Consultants Activeconsultant { get; set; }
        public virtual Casestatuses Currentstatus { get; set; }
        public virtual ICollection<Appointments> Appointments { get; set; }
    }
}
