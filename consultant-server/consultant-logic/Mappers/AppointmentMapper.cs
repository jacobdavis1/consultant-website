using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class AppointmentMapper
    {
        public static Appointment Map(Database.Appointments appointment)
        {
            return new Appointment
            {
                Id = Guid.Parse(appointment.Appointmentid),
                CaseId = Guid.Parse(appointment.Caseid),
                Title = appointment.Appointmenttitle,
                AppointmentDateTime = appointment.Appointmentdatetime.GetValueOrDefault()
            };
        }

        public static Database.Appointments Map(Appointment appointment)
        {
            return new Database.Appointments
            {
                Appointmentid = appointment.Id.ToString(),
                Caseid = appointment.CaseId.ToString(),
                Appointmenttitle = appointment.Title,
                Appointmentdatetime = appointment.AppointmentDateTime
            };
        }
    }
}
