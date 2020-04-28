using System;
using System.Collections.Generic;
using System.Text;

using consultant_logic.Models;

namespace consultant_data.Mappers
{
    public class CaseStatusMapper
    {
        public static CaseStatus MapCaseStatus(Database.Casestatuses caseStatus)
        {
            return new CaseStatus
            {
                Id = Guid.Parse(caseStatus.Statusid),
                Text = caseStatus.Statustext
            };
        }

        public static Database.Casestatuses MapCaseStatus(CaseStatus caseStatus)
        {
            return new Database.Casestatuses
            {
                Statusid = caseStatus.Id.ToString(),
                Statustext = caseStatus.Text
            };
        }
    }
}
