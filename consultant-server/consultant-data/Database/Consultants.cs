using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Consultants
    {
        public Consultants()
        {
            Cases = new HashSet<Cases>();
        }

        public string Consultantid { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }

        public virtual ICollection<Cases> Cases { get; set; }
    }
}
