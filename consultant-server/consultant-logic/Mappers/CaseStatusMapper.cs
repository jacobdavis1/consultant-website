using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class CaseStatusMapper
    {
        public static CaseStatus Map(Database.Casestatuses caseStatus)
        {
            return new CaseStatus
            {
                Id = Guid.Parse(caseStatus.Statusid),
                Text = caseStatus.Statustext
            };
        }

        public static Database.Casestatuses Map(CaseStatus caseStatus)
        {
            return new Database.Casestatuses
            {
                Statusid = caseStatus.Id.ToString(),
                Statustext = caseStatus.Text
            };
        }
    }
}
