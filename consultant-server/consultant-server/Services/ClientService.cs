using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_logic.Services
{
    class ClientService
    {
        IUserRepository _user;
        ICaseRepository _case;
        ICaseStatusRepository _status;
        ICaseNoteRepository _note;

        public Task AddClient(User client)
        {

        }



        public Task CreateClientAppointment(User client, Appointment appointment)
        {

        }
    }
}
