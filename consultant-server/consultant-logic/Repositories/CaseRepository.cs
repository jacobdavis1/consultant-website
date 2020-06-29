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

        public CaseRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCaseAsync(Case targetCase)
        {
            try
            {
                Cases c = _context.Cases.Add(CaseMapper.Map(targetCase)).Entity;

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

                foreach (Appointment a in targetCase.UpcomingAppointments)
                {
                    _context.Appointments.Add(AppointmentMapper.Map(a));
                }

                foreach (CaseNote cn in targetCase.Notes)
                {
                    _context.Casenotes.Add(CaseNoteMapper.Map(cn));
                }

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
                Case aCase = CaseMapper.Map(await _context.Cases
                    .Include(c => c.Appointments)
                    .Include(c => c.Casenotes)
                    .FirstOrDefaultAsync(c => c.Caseid == caseId.ToString()));

                List<Caseclient> caseClients = _context.Caseclient.Where(cc => cc.Caseid == caseId.ToString()).ToList();
                foreach (Caseclient cc in caseClients)
                {
                    aCase.Clients.Add(UserMapper.Map(_context.Users.FirstOrDefault(u => u.Userid == cc.Clientid)));
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

        public async Task<bool> UpdateCaseAsync(Case targetCase)
        {
            try
            {
                _context.Cases.Update(_context.Cases.FirstOrDefault(c => c.Caseid == targetCase.ToString()));

                foreach (Appointment appointment in targetCase.UpcomingAppointments)
                {
                    _context.Appointments.Update(_context.Appointments.FirstOrDefault(a => a.Appointmentid == appointment.Id.ToString()));
                }

                foreach (CaseNote note in targetCase.Notes)
                {
                    _context.Casenotes.Update(_context.Casenotes.FirstOrDefault(a => a.Noteid == note.Id.ToString()));
                }

                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCaseAsync(Case targetCase)
        {
            try
            {
                // Delete all entries in appointment
                foreach (Appointment appointment in targetCase.UpcomingAppointments)
                {
                    _context.Appointments.Remove(_context.Appointments.FirstOrDefault(a => a.Appointmentid == appointment.Id.ToString()));
                }

                // Next, delete all entries in casenote
                foreach (CaseNote note in targetCase.Notes)
                {
                    _context.Casenotes.Remove(_context.Casenotes.FirstOrDefault(a => a.Noteid == note.Id.ToString()));
                }

                // Next, delete all entries in caseclient
                foreach (User client in targetCase.Clients)
                {
                    Caseclient cc = _context.Caseclient.FirstOrDefault(cc => cc.Caseid == targetCase.Id.ToString());
                    if (cc != null)
                        _context.Caseclient.Remove(cc);
                }

                // Finally, remove the case
                _context.Cases.Remove(_context.Cases.FirstOrDefault(c => c.Caseid == targetCase.Id.ToString()));

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
