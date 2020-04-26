using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Caseclient
    {
        public string Caseid { get; set; }
        public string Clientid { get; set; }

        public virtual Cases Case { get; set; }
        public virtual Clients Client { get; set; }
    }
}
