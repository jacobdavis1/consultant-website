using System;
using System.Collections.Generic;
using System.Text;

using consultant_logic.Models;

namespace consultant_data.Mappers
{
    public class AppointmentMapper
    {
        public static Appointment MapAppointment(Database.Appointments appointment)
        {
            return new Appointment
            {
                Id = Guid.Parse(appointment.Appointmentid),
                Title = appointment.Appointmenttitle,
                AppointmentDateTime = appointment.Appointmentdatetime.GetValueOrDefault()
            };
        }

        public static Database.Appointments MapAppointment(Appointment appointment)
        {
            return new Database.Appointments
            {
                Appointmentid = appointment.Id.ToString(),
                Appointmenttitle = appointment.Title,
                Appointmentdatetime = appointment.AppointmentDateTime
            };
        }
    }
}
