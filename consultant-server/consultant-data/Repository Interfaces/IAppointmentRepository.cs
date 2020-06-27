using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using consultant_data.Models;

namespace consultant_data.RepositoryInterfaces
{
    public interface IAppointmentRepository
    {
        Task<bool> AddAppointmentAsync(Appointment appointment);

        Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId);

        Task<List<Appointment>> GetAllAppointmentsForConsultantAsync(Consultant consultant);

        Task<List<Appointment>> GetAllAppointmentsForDateAsync(DateTime dateTime);

        Task<bool> UpdateAppointmentAsync(Appointment appointment);

        Task<bool> DeleteAppointmentAsync(Appointment appointment);
    }
}
