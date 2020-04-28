using System;
using System.Collections.Generic;
using System.Text;

using consultant_logic.Models;

namespace consultant_data.Mappers
{
    public class ConsultantMapper
    {
        public static Consultant MapConsultant(Database.Consultants consultant)
        {
            return new Consultant
            {
                Id = Guid.Parse(consultant.Consultantid),
                FirstName = consultant.Firstname,
                MiddleName = consultant.Middlename,
                LastName = consultant.Lastname
            };
        }

        public static Database.Consultants MapConsultant(Consultant consultant)
        {
            return new Database.Consultants
            {
                Consultantid = consultant.Id.ToString(),
                Firstname = consultant.FirstName,
                Middlename = consultant.MiddleName,
                Lastname = consultant.LastName
            }
        }
    }
}
