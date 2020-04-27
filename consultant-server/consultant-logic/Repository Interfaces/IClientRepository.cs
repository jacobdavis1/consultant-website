using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    interface IClientRepository
    {
        Task<bool> AddClient(Client client);

        Task<Client> GetClientById(Guid clientId);

        Task<List<Client>> SearchClientsByName(string firstName, string middleName, string lastName);

        Task<List<Client>> GetAllClientsByCase(Case targetCase);

        Task<bool> UpdateClient(Client client);

        Task<bool> DeleteClient(Client client);
    }
}
