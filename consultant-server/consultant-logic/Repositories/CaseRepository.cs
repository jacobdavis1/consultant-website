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
        private IUserRepository _user;

        public CaseRepository(khbatlzvContext context, IAppointmentRepository appointment, INoteRepository note, IUserRepository user)
        {
            _context = context;
            _appointment = appointment;
            _note = note;
            _user = user;
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
                _context.Entry(dbCase).Reference(c => c.Currentstatus).Load();
                _context.Entry(dbCase).Reference(c => c.Activeconsultant).Load();

                // Next, add the caseclient entry for every client
                foreach (User user in targetCase.Clients)
                {
                    Caseclient dbCaseclient = _context.Caseclient.Add(new Caseclient
                    {
                        Caseid = dbCase.Caseid,
                        Clientid = user.Id
                    }).Entity;

                    User u = await _user.GetUserByRowIdAsync(user.Id);
                    u.Cases.Add(targetCase);
                    await _user.UpdateUserAsync(u, false);

                    dbCase.Caseclient.Add(dbCaseclient);
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
                    .Include(c => c.Currentstatus)
                    .Include(c => c.Activeconsultant)
                        .ThenInclude(u => u.UserroleNavigation)
                    .FirstOrDefaultAsync(c => c.Caseid == caseId);

                if (dbCase == null)
                    return null;

                Case aCase = CaseMapper.Map(dbCase);

                List<Caseclient> caseClients = _context.Caseclient.Where(cc => cc.Caseid == caseId).ToList();
                foreach (Caseclient cc in caseClients)
                {
                    Users dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Rowid == cc.Clientid);
                    _context.Entry(dbUser).Reference(u => u.UserroleNavigation).Load();
                    aCase.Clients.Add(UserMapper.Map(dbUser));
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
                    .Include(c => c.Activeconsultant)
                        .ThenInclude(u => u.UserroleNavigation)
                    .Include(c => c.Appointments)
                    .Include(c => c.Casenotes)
                    .Include(c => c.Currentstatus)
                    .Where(c => c.Activeconsultant.Rowid == consultant.Id)
                    .Select(CaseMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Case>();
            }
        }

        public async Task<List<Case>> GetAllCasesForClientAsync(User client)
        {
            try
            {
                Users dbUser = await _context.Users.FirstOrDefaultAsync(u => u.Rowid == client.Id);

                return dbUser.Cases
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
                _context.Entry(dbCase).Reference(c => c.Currentstatus).Load();

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
