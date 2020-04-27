using consultant_logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_logic.RepositoryInterfaces
{
    interface ICaseRepository
    {
        Task<bool> AddCase(Case targetCase);

        Task<Case> GetCaseById(Guid caseId);

        Task<List<Case>> GetAllCasesForConsultant(Consultant consultant);

        Task<bool> UpdateCase(Case targetCase);

        Task<bool> DeleteCase(Case targetCase);
    }
}
