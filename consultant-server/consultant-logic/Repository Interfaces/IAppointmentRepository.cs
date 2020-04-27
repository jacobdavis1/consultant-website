using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    interface IAppointmentRepository
    {
        Task<bool> AddAppointment(Appointment appointment);

        Task<Appointment> GetAppointmentById(Guid appointmentId);

        Task<List<Appointment>> GetAllAppointmentsForConsultant(Consultant consultant);

        Task<List<Appointment>> GetAllAppointmentsForDate(DateTime dateTime);

        Task<bool> UpdateAppointment(Appointment appointment);

        Task<bool> DeleteAppointment(Appointment appointment);
    }
}
