using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class ClientMapper
    {
        public static Client Map(Database.Clients client)
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

        public static Database.Clients Map(Client client)
        {
            return new Database.Clients
            {
                Clientid = client.Id.ToString(),
                Firstname = client.FirstName,
                Middlename = client.MiddleName,
                Lastname = client.LastName,
                Email = client.Email
            };
        }
    }
}
