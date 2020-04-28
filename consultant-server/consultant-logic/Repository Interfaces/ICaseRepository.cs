using consultant_logic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_logic.RepositoryInterfaces
{
    public interface ICaseRepository
    {
        Task<bool> AddCaseAsync(Case targetCase);

        Task<Case> GetCaseByIdAsync(Guid caseId);

        Task<List<Case>> GetAllCasesForConsultantAsync(Consultant consultant);

        Task<bool> UpdateCaseAsync(Case targetCase);

        Task<bool> DeleteCaseAsync(Case targetCase);
    }
}
