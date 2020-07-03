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

        public override bool Equals(Object obj)
        {
            Case other = obj as Case;

            if (other == null) return false;

            return (Id == other.Id && ActiveConsultant.Equals(other.ActiveConsultant)
                        && Status.Equals(other.Status) && Title == other.Title);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
