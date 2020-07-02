using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Case
    {
        public int Id { get; set;}
        public User ActiveConsultant { get; set; }
        public Status Status { get; set; }
        public string Title { get; set; }
        public List<User> Clients { get; set; } = new List<User>();
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
        public List<Note> Notes { get; set; } = new List<Note>();
    }
}
