using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_data.RepositoryInterfaces
{
    public interface ICaseRepository
    {
        #region Cases
        Task<bool> AddCaseAsync(Case targetCase);

        Task<Case> GetCaseByIdAsync(Guid caseId);

        Task<List<Case>> GetAllCasesForConsultantAsync(User consultant);

        Task<bool> UpdateCaseAsync(Case targetCase);

        Task<bool> DeleteCaseAsync(Case targetCase);
        #endregion


        #region Appointments
        Task<bool> AddAppointmentToCaseAsync(Case targetCase, Appointment appointment);

        Task<List<Appointment>> GetAllAppointmentsForCaseAsync(Case targetCase);

        Task<List<Appointment>> GetAllAppointmentsForConsultantAsync(User consultant);

        Task<List<Appointment>> GetAllAppointmentsForDateAsyncAsync(DateTime dateTime);

        Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId);

        Task<Appointment> GetNextAppointmentForCaseAsync(Case targetCase);

        Task<bool> UpdateAppointmentAsync(Appointment appointment);

        Task<bool> DeleteAppointmentFromCaseAsync(Case targetCase, Appointment appointment);
        #endregion


        #region Notes
        Task<bool> AddNoteToCaseAsync(Case targetCase, CaseNote note);

        Task<List<CaseNote>> GetAllNotesForCaseAsync(Case targetCase);

        Task<CaseNote> GetNoteByIdAsync(Guid noteId);

        Task<bool> UpdateNoteAsync(CaseNote note);

        Task<bool> DeleteNoteFromCaseAsync(Case targetCase, CaseNote note);
        #endregion
    }
}
