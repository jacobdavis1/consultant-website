using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_data.RepositoryInterfaces
{
    public interface ICaseRepository
    {
        Task<Case> AddCaseAsync(Case targetCase, bool save = true);

        Task<Case> GetCaseByIdAsync(int caseId);

        Task<List<Case>> GetAllCasesForConsultantAsync(User consultant);

        Task<List<Case>> GetAllCasesForClientAsync(User client);

        Task<Case> UpdateCaseAsync(Case targetCase, bool save = true);

        Task<bool> DeleteCaseAsync(Case targetCase, bool save = true);
    }
}
