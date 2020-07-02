using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class CaseStatusMapper
    {
        public static Status Map(Database.Casestatuses caseStatus)
        {
            return new Status
            {
                Id = caseStatus.Statusid,
                Text = caseStatus.Statustext
            };
        }

        public static Database.Casestatuses Map(Status caseStatus)
        {
            return new Database.Casestatuses
            {
                Statusid = caseStatus.Id,
                Statustext = caseStatus.Text
            };
        }
    }
}
