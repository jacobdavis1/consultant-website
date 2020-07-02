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

        public int Statusid { get; set; }
        public string Statustext { get; set; }

        public virtual ICollection<Cases> Cases { get; set; }
    }
}
