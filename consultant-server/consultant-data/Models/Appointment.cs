using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Appointment
    {
        public int Id { get; set; }
        public int CaseId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Title { get; set; }
    }
}
