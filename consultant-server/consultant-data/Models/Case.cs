using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Case
    {
        public Guid Id { get; set;}
        public User ActiveConsultant { get; set; }
        public CaseStatus Status { get; set; }
        public string Title { get; set; }
        public List<User> Clients { get; set; } = new List<User>();
        public List<Appointment> UpcomingAppointments { get; set; } = new List<Appointment>();
        public List<CaseNote> Notes { get; set; } = new List<CaseNote>();
    }
}
