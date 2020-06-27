using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class ConsultantMapper
    {
        public static Consultant Map(Database.Consultants consultant)
        {
            return new Consultant
            {
                Id = Guid.Parse(consultant.Consultantid),
                FirstName = consultant.Firstname,
                MiddleName = consultant.Middlename,
                LastName = consultant.Lastname
            };
        }

        public static Database.Consultants Map(Consultant consultant)
        {
            return new Database.Consultants
            {
                Consultantid = consultant.Id.ToString(),
                Firstname = consultant.FirstName,
                Middlename = consultant.MiddleName,
                Lastname = consultant.LastName
            };
        }
    }
}
