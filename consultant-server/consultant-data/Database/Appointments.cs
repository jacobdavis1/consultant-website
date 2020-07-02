using System;
using System.Collections.Generic;

namespace consultant_data.Database
{
    public partial class Appointments
    {
        public int Appointmentid { get; set; }
        public int? Caseid { get; set; }
        public DateTime Appointmentdatetime { get; set; }
        public string Appointmenttitle { get; set; }

        public virtual Cases Case { get; set; }
    }
}
