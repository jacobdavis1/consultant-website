using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Caseclient
    {
        public int Rowid { get; set; }
        public int? Caseid { get; set; }
        public int? Clientid { get; set; }

        public virtual Cases Case { get; set; }
        public virtual Users Client { get; set; }
    }
}
