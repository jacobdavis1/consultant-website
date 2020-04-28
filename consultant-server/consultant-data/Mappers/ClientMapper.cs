using System;
using System.Collections.Generic;
using System.Text;

using consultant_logic.Models;

namespace consultant_data.Mappers
{
    public class ClientMapper
    {
        public static Client MapClient(Database.Clients client)
        {
            return new Client
            {
                Id = Guid.Parse(client.Clientid),
                FirstName = client.Firstname,
                MiddleName = client.Middlename,
                LastName = client.Lastname,
                Email = client.Email
            };
        }

        public static Database.Clients MapClient(Client client)
        {
            return new Database.Clients
            {
                Clientid = client.Id.ToString(),
                Firstname = client.FirstName,
                Middlename = client.MiddleName,
                Lastname = client.LastName,
                Email = client.Email
            }
        }
    }
}
