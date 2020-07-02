using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_data.RepositoryInterfaces
{
    public interface IAppointmentRepository
    {
        Task<bool> AddAppointmentToCaseAsync(Case targetCase, Appointment appointment, bool save = true);

        Task<List<Appointment>> GetAllAppointmentsForCaseAsync(Case targetCase);

        Task<List<Appointment>> GetAllAppointmentsForConsultantAsync(User consultant);

        Task<List<Appointment>> GetAllAppointmentsForDateAsyncAsync(DateTime dateTime);

        Task<Appointment> GetAppointmentByIdAsync(int appointmentId);

        Task<Appointment> GetNextAppointmentForCaseAsync(Case targetCase);

        Task<bool> UpdateAppointmentAsync(Appointment appointment, bool save = true);

        Task<bool> DeleteAppointmentFromCaseAsync(Case targetCase, Appointment appointment, bool save = true);
    }
}
