using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_data.RepositoryInterfaces
{
    public interface ICaseRepository
    {
        Task<bool> AddCaseAsync(Case targetCase);

        Task<Case> GetCaseByIdAsync(Guid caseId);

        Task<List<Case>> GetAllCasesForConsultantAsync(User consultant);

        Task<bool> UpdateCaseAsync(Case targetCase);

        Task<bool> DeleteCaseAsync(Case targetCase);
    }
}
