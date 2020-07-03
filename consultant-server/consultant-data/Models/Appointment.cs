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

        public override bool Equals(Object obj)
        {
            Appointment other = obj as Appointment;

            if (other == null) return false;

            return (Id == other.Id && CaseId == other.CaseId
                        && AppointmentDateTime.Equals(other.AppointmentDateTime)
                        && Title == other.Title);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
