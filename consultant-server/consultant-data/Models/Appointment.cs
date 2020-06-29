using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public string Title { get; set; }
    }
}
