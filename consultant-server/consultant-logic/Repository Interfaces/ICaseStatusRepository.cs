using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    interface ICaseStatusRepository
    {
        Task<bool> AddCaseStatus(CaseStatus caseStatus);

        Task<CaseStatus> GetCaseStatusById(Guid statusId);

        Task<CaseStatus> GetCaseStatusByText(string text);

        Task<bool> UpdateCaseStatus(CaseStatus caseStatus);

        Task<bool> DeleteCaseStatus(CaseStatus caseStatus);
    }
}
