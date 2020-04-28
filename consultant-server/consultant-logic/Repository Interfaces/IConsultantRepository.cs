using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    public interface IConsultantRepository
    {
        Task<bool> AddConsultantAsync(Consultant consultant);

        Task<Consultant> GetConsultantByIdAsync(Guid consultantId);

        Task<List<Consultant>> SearchConsultantsByNameAsync(string firstName, string middleName, string lastName);

        Task<bool> UpdateConsultantAsync(Consultant consultant);

        Task<bool> DeleteConsultantAsync(Consultant consultant);
    }
}
