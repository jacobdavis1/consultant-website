using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Cases
    {
        public Cases()
        {
            Appointments = new HashSet<Appointments>();
            Caseclient = new HashSet<Caseclient>();
            Casenotes = new HashSet<Casenotes>();
        }

        public int Caseid { get; set; }
        public string Casetitle { get; set; }
        public int? Activeconsultantid { get; set; }
        public int? Currentstatusid { get; set; }

        public virtual Users Activeconsultant { get; set; }
        public virtual Casestatuses Currentstatus { get; set; }
        public virtual ICollection<Appointments> Appointments { get; set; }
        public virtual ICollection<Caseclient> Caseclient { get; set; }
        public virtual ICollection<Casenotes> Casenotes { get; set; }
    }
}
