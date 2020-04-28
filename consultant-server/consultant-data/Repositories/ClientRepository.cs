using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using consultant_data.Database;
using consultant_data.Mappers;
using consultant_logic.Models;
using consultant_logic.RepositoryInterfaces;

namespace consultant_data.Repositories
{
    class ClientRepository : IClientRepository
    {
        private readonly khbatlzvContext _context;

        public ClientRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddClientAsync(Client client)
        {
            try
            {
                await _context.Clients.AddAsync(ClientMapper.MapClient(client));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Client> GetClientByIdAsync(Guid clientId)
        {
            try
            {
                return ClientMapper.MapClient(await _context.Clients.FindAsync(clientId.ToString()));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Client>> SearchClientsByNameAsync(string firstName, string middleName, string lastName)
        {
            try
            {
                return _context.Clients.Where(c => (c.Firstname == firstName || firstName == "")
                                                            && (c.Middlename == middleName || middleName == "")
                                                            && (c.Lastname == lastName || lastName == ""))
                    .Select(ClientMapper.MapClient)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Client>();
            }
        }

        public async Task<List<Client>> GetAllClientsByCaseAsync(Case targetCase)
        {
            throw new NotImplementedException();
        }

        /* public async Task<List<Client>> GetAllClientsByCaseAsync(Case targetCase)
        {
            try
            {
                return await _context.Cases.FindAsync(targetCase.Id.ToString())
                    .Result
                    .Clients
                    .Select(ClientMapper.MapClient)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Client>();
            }
        } */

        public async Task<bool> UpdateClientAsync(Client client)
        {
            try
            {
                _context.Clients.Update(ClientMapper.MapClient(client));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteClientAsync(Client client)
        {
            try
            {
                _context.Clients.Remove(ClientMapper.MapClient(client));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
