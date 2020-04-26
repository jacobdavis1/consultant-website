using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Casenotes
    {
        public string Noteid { get; set; }
        public string Caseid { get; set; }
        public string Content { get; set; }

        public virtual Cases Case { get; set; }
    }
}
