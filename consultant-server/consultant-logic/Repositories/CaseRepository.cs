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
        private IAppointmentRepository _appointment;
        private INoteRepository _note;

        public CaseRepository(khbatlzvContext context, IAppointmentRepository appointment, INoteRepository note)
        {
            _context = context;
            _appointment = appointment;
            _note = note;
        }

        // NOTE: AddCaseAsync isnt able to delay saving due to needing to query the db in order to get the id,
        //  which is accomplished by saving
        public async Task<Case> AddCaseAsync(Case targetCase, bool save = true)
        {
            try
            {
                // Add the case
                Cases dbCase = _context.Cases.Add(CaseMapper.Map(targetCase)).Entity;
                await _context.SaveChangesAsync();

                // Next, add the caseclient entry for every client
                foreach (User user in targetCase.Clients)
                {
                    _context.Caseclient.Add(new Caseclient
                    {
                        Caseid = dbCase.Caseid,
                        Clientid = user.Id
                    });

                    Users contextUser = await _context.Users.FirstOrDefaultAsync(u => u.Rowid == user.Id);
                    contextUser.Cases.Add(dbCase);
                    _context.Users.Update(contextUser);
                }

                // Next, add all of the case's appointments, if any
                foreach (Appointment a in targetCase.Appointments)
                {
                    await _appointment.AddAppointmentToCaseAsync(targetCase, a, false);
                }

                // Finally, add all of the case's notes, if any
                foreach (Note cn in targetCase.Notes)
                {
                    await _note.AddNoteToCaseAsync(targetCase, cn, false);
                }

                if (save)
                    await _context.SaveChangesAsync();

                return CaseMapper.Map(dbCase);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<Case> GetCaseByIdAsync(int caseId)
        {
            try
            {
                Cases dbCase = await _context.Cases
                    .Include(c => c.Appointments)
                    .Include(c => c.Casenotes)
                    .FirstOrDefaultAsync(c => c.Caseid == caseId);

                if (dbCase == null)
                    return null;

                Case aCase = CaseMapper.Map(dbCase);

                List<Caseclient> caseClients = _context.Caseclient.Where(cc => cc.Caseid == caseId).ToList();
                foreach (Caseclient cc in caseClients)
                {
                    aCase.Clients.Add(UserMapper.Map(await _context.Users.FirstOrDefaultAsync(u => u.Rowid == cc.Clientid)));
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
                    .Where(c => c.Activeconsultant.Rowid == consultant.Id)
                    .Select(CaseMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Case>();
            }
        }

        public async Task<Case> UpdateCaseAsync(Case targetCase, bool save = true)
        {
            try
            {
                foreach (Appointment appointment in targetCase.Appointments)
                {
                    await _appointment.UpdateAppointmentAsync(appointment);
                }

                foreach (Note note in targetCase.Notes)
                {
                    await _note.UpdateNoteAsync(note);
                }

                Cases dbCase = await _context.Cases.FirstOrDefaultAsync(c => c.Caseid == targetCase.Id);
                dbCase.Activeconsultantid = targetCase.ActiveConsultant.Id;
                dbCase.Currentstatusid = targetCase.Status.Id;
                dbCase.Casetitle = targetCase.Title;

                dbCase = _context.Cases.Update(dbCase).Entity;

                if (save)
                    await _context.SaveChangesAsync();
                
                return CaseMapper.Map(dbCase);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> DeleteCaseAsync(Case targetCase, bool save = true)
        {
            try
            {
                // Delete all entries in appointment
                List<Appointment> appointmentCopy = targetCase.Appointments.ToList();
                foreach (Appointment appointment in appointmentCopy)
                {
                    await _appointment.DeleteAppointmentFromCaseAsync(targetCase, appointment, false);
                }

                // Next, delete all entries in casenote
                List<Note> notesCopy = targetCase.Notes.ToList();
                foreach (Note note in notesCopy)
                {
                    await _note.DeleteNoteFromCaseAsync(targetCase, note, false);
                }

                // Next, delete all entries in caseclient
                foreach (User client in targetCase.Clients)
                {
                    Caseclient cc = await _context.Caseclient.FirstOrDefaultAsync(cc => cc.Caseid == targetCase.Id);
                    if (cc != null)
                        _context.Caseclient.Remove(cc);
                }

                // Finally, remove the case
                _context.Cases.Remove(await _context.Cases.FirstOrDefaultAsync(c => c.Caseid == targetCase.Id));

                if (save)
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
