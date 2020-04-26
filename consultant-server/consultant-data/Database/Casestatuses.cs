using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Casestatuses
    {
        public Casestatuses()
        {
            Cases = new HashSet<Cases>();
        }

        public string Statusid { get; set; }
        public string Statustext { get; set; }

        public virtual ICollection<Cases> Cases { get; set; }
    }
}
