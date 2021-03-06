﻿using System;
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
                Id = appointment.Appointmentid,
                CaseId = appointment.Caseid ?? -1,
                Title = appointment.Appointmenttitle,
                AppointmentDateTime = appointment.Appointmentdatetime
            };
        }

        public static Database.Appointments Map(Appointment appointment)
        {
            return new Database.Appointments
            {
                Appointmentid = appointment.Id,
                Caseid = appointment.CaseId,
                Appointmenttitle = appointment.Title,
                Appointmentdatetime = appointment.AppointmentDateTime
            };
        }
    }
}
