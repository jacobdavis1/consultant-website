using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    interface IConsultantRepository
    {
        Task<bool> AddConsultant(Consultant consultant);

        Task<Consultant> GetConsultantById(Guid consultantId);

        Task<List<Consultant>> SearchConsultantsByName(string firstName, string middleName, string lastName);

        Task<bool> UpdateConsultant(Consultant consultant);

        Task<bool> DeleteConsultant(Consultant consultant);
    }
}
