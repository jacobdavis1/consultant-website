using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class CaseMapper
    {
        public static Case Map(Database.Cases targetCase)
        {
            return new Case
            {
                Id = Guid.Parse(targetCase.Caseid),
                Title = targetCase.Casetitle,
                Status = CaseStatusMapper.Map(targetCase.Currentstatus),
                UpcomingApointments = targetCase.Appointments.Select(AppointmentMapper.Map).ToList()
            };
        }

        public static Database.Cases Map(Case targetCase)
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
