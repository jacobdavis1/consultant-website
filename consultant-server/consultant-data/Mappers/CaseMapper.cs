using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using consultant_logic.Models;

namespace consultant_data.Mappers
{
    public class CaseMapper
    {
        public static Case MapCase(Database.Cases targetCase)
        {
            return new Case
            {
                Id = Guid.Parse(targetCase.Caseid),
                Title = targetCase.Casetitle,
                Status = CaseStatusMapper.MapCaseStatus(targetCase.Currentstatus),
                UpcomingApointments = targetCase.Appointments.Select(AppointmentMapper.MapAppointment).ToList()
            };
        }

        public static Database.Cases MapCase(Case targetCase)
        {
            return new Database.Cases
            {
                Caseid = targetCase.Id.ToString(),
                Activeconsultantid = targetCase.ActiveConsultant.Id.ToString(),
                Currentstatusid = targetCase.Status.Id.ToString()
            };
        }
    }
}
