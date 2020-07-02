using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Casenotes
    {
        public int Noteid { get; set; }
        public int? Caseid { get; set; }
        public string Content { get; set; }

        public virtual Cases Case { get; set; }
    }
}
