using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    public interface IClientRepository
    {
        Task<bool> AddClientAsync(Client client);

        Task<Client> GetClientByIdAsync(Guid clientId);

        Task<List<Client>> SearchClientsByNameAsync(string firstName, string middleName, string lastName);

        Task<List<Client>> GetAllClientsByCaseAsync(Case targetCase);

        Task<bool> UpdateClientAsync(Client client);

        Task<bool> DeleteClientAsync(Client client);
    }
}
