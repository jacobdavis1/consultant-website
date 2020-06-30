using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using consultant_data.Database;
using consultant_data.Mappers;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;

namespace consultant_logic.Repositories
{
    public class CaseRepository : ICaseRepository
    {
        private readonly khbatlzvContext _context;

        #region Cases
        public CaseRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCaseAsync(Case targetCase, bool save = true)
        {
            try
            {
                // Add the case
                Cases c = _context.Cases.Add(CaseMapper.Map(targetCase)).Entity;

                // Next, add the caseclient entry for every client
                foreach (User user in targetCase.Clients)
                {
                    _context.Caseclient.Add(new Caseclient
                    {
                        Caseid = targetCase.Id.ToString(),
                        Clientid = user.Id.ToString()
                    });

                    Users contextUser = await _context.Users.FirstOrDefaultAsync(u => u.Userid == user.Id.ToString());
                    contextUser.Cases.Add(c);
                    _context.Users.Update(contextUser);
                }

                // Next, add all of the case's appointments, if any
                foreach (Appointment a in targetCase.UpcomingAppointments)
                {
                    await AddAppointmentToCaseAsync(targetCase, a, false);
                }

                // Finally, add all of the case's notes, if any
                foreach (CaseNote cn in targetCase.Notes)
                {
                    await AddNoteToCaseAsync(targetCase, cn, false);
                }

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Case> GetCaseByIdAsync(Guid caseId)
        {
            try
            {
                Cases dbCase = await _context.Cases
                    .Include(c => c.Appointments)
                    .Include(c => c.Casenotes)
                    .FirstOrDefaultAsync(c => c.Caseid == caseId.ToString());

                if (dbCase == null)
                    return null;

                Case aCase = CaseMapper.Map(dbCase);

                List<Caseclient> caseClients = _context.Caseclient.Where(cc => cc.Caseid == caseId.ToString()).ToList();
                foreach (Caseclient cc in caseClients)
                {
                    aCase.Clients.Add(UserMapper.Map(await _context.Users.FirstOrDefaultAsync(u => u.Userid == cc.Clientid)));
                }

                return aCase;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Case>> GetAllCasesForConsultantAsync(User consultant)
        {
            try
            {
                return _context.Cases
                    .Include(c => c.Appointments)
                    .Include(c => c.Casenotes)
                    .Where(c => c.Activeconsultant.Userid == consultant.Id.ToString())
                    .Select(CaseMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Case>();
            }
        }

        public async Task<bool> UpdateCaseAsync(Case targetCase, bool save = true)
        {
            try
            {
                foreach (Appointment appointment in targetCase.UpcomingAppointments)
                {
                    await UpdateAppointmentAsync(appointment);
                }

                foreach (CaseNote note in targetCase.Notes)
                {
                    await UpdateNoteAsync(note);
                }

                Cases dbCase = await _context.Cases.FirstOrDefaultAsync(c => c.Caseid == targetCase.ToString());
                dbCase.Activeconsultantid = targetCase.ActiveConsultant.Id.ToString();
                dbCase.Currentstatusid = targetCase.Status.Id.ToString();
                dbCase.Casetitle = targetCase.Title;

                _context.Cases.Update(dbCase);

                if (save)
                    await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCaseAsync(Case targetCase, bool save = true)
        {
            try
            {
                // Delete all entries in appointment
                List<Appointment> appointmentCopy = targetCase.UpcomingAppointments.ToList();
                foreach (Appointment appointment in appointmentCopy)
                {
                    await DeleteAppointmentFromCaseAsync(targetCase, appointment);
                }

                // Next, delete all entries in casenote
                List<CaseNote> notesCopy = targetCase.Notes.ToList();
                foreach (CaseNote note in notesCopy)
                {
                    await DeleteNoteFromCaseAsync(targetCase, note);
                }

                // Next, delete all entries in caseclient
                foreach (User client in targetCase.Clients)
                {
                    Caseclient cc = await _context.Caseclient.FirstOrDefaultAsync(cc => cc.Caseid == targetCase.Id.ToString());
                    if (cc != null)
                        _context.Caseclient.Remove(cc);
                }

                // Finally, remove the case
                _context.Cases.Remove(await _context.Cases.FirstOrDefaultAsync(c => c.Caseid == targetCase.Id.ToString()));

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion;

        #region Appointments
        public async Task<bool> AddAppointmentToCaseAsync(Case targetCase, Appointment appointment, bool save = true)
        {
            try
            {
                // First, add the appointment to appointments
                appointment.CaseId = targetCase.Id;
                _context.Appointments.Add(AppointmentMapper.Map(appointment));

                // Finally, add the appointment to the case model's appointments
                targetCase.UpcomingAppointments.Add(appointment);

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<Appointment>> GetAllAppointmentsForCaseAsync(Case targetCase)
        {
            try
            {
                return _context.Appointments
                    .Where(a => a.Case.Caseid == targetCase.Id.ToString())
                    .Select(AppointmentMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Appointment>();
            }
        }

        public async Task<List<Appointment>> GetAllAppointmentsForConsultantAsync(User consultant)
        {
            try
            {
                return _context.Appointments
                    .Where(a => a.Case.Activeconsultantid == consultant.Id.ToString())
                    .Select(AppointmentMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Appointment>();
            }
        }

        public async Task<List<Appointment>> GetAllAppointmentsForDateAsyncAsync(DateTime dateTime)
        {
            try
            {
                return _context.Appointments
                    .Where(a => a.Appointmentdatetime.GetValueOrDefault().Date.CompareTo(dateTime.Date) == 0)
                    .Select(AppointmentMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Appointment>();
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId)
        {
            try
            {
                Appointments dbAppointment = await _context.Appointments.FindAsync(appointmentId.ToString());

                if (dbAppointment == null)
                    return null;

                return AppointmentMapper.Map(dbAppointment);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Appointment> GetNextAppointmentForCaseAsync(Case targetCase)
        {
            return null;
        }

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment, bool save = true)
        {
            try
            {
                Appointments dbAppointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Appointmentid == appointment.Id.ToString());
                dbAppointment.Appointmentdatetime = appointment.AppointmentDateTime;
                dbAppointment.Appointmenttitle = appointment.Title;

                _context.Appointments.Update(dbAppointment);

                if (save)
                    await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAppointmentFromCaseAsync(Case targetCase, Appointment appointment, bool save = true)
        {
            try
            {
                // First, remove the appointment from the case model
                targetCase.UpcomingAppointments.Remove(appointment);

                // Finally, remove the appointment
                _context.Appointments.Remove(await _context.Appointments.FirstOrDefaultAsync(a => a.Appointmentid == appointment.Id.ToString()));
                
                if (save)
                    await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion


        #region Notes
        public async Task<bool> AddNoteToCaseAsync(Case targetCase, CaseNote note, bool save = true)
        {
            try
            {
                // First, add the note
                note.CaseId = targetCase.Id;
                _context.Casenotes.Add(CaseNoteMapper.Map(note));

                // Finally, update the case model and db
                targetCase.Notes.Add(note);

                if (save)
                    await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<CaseNote>> GetAllNotesForCaseAsync(Case targetCase)
        {
            try
            {
                return _context.Casenotes.Where(cn => cn.Caseid == targetCase.Id.ToString())
                    .Select(CaseNoteMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<CaseNote>();
            }
        }

        public async Task<CaseNote> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                Casenotes dbNote = await _context.Casenotes.FirstOrDefaultAsync(n => n.Noteid == noteId.ToString());

                if (dbNote == null)
                    return null;

                return CaseNoteMapper.Map(dbNote);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateNoteAsync(CaseNote note, bool save = true)
        {
            try
            {
                Casenotes dbNote = await _context.Casenotes.FirstOrDefaultAsync(n => n.Noteid == note.Id.ToString());
                dbNote.Content = note.Content;

                _context.Casenotes.Update(dbNote);

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNoteFromCaseAsync(Case targetCase, CaseNote note, bool save = true)
        {
            try
            {
                // First, remove the note from the case model
                targetCase.Notes.Remove(note);

                // Finally, remove the note
                _context.Casenotes.Remove(await _context.Casenotes.FirstOrDefaultAsync(n => n.Noteid == note.Id.ToString()));

                if (save)
                    await _context.SaveChangesAsync();
                
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}
