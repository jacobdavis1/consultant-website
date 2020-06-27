using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using consultant_data.Database;
using consultant_data.Mappers;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;

namespace consultant_data.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly khbatlzvContext _context;

        public AppointmentRepository(khbatlzvContext context)
        {
            context = _context;
        }

        public async Task<bool> AddAppointmentAsync(Appointment appointment)
        {
            try
            {
                await _context.Appointments.AddAsync(AppointmentMapper.Map(appointment));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

        public async Task<Appointment> GetAppointmentByIdAsync(Guid appointmentId)
        {
            try
            {
                return AppointmentMapper.Map(await _context.Appointments.FindAsync(appointmentId.ToString()));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Appointment>> GetAllAppointmentsForConsultantAsync(Consultant consultant)
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

        public async Task<List<Appointment>> GetAllAppointmentsForDateAsync(DateTime dateTime)
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

        public async Task<bool> UpdateAppointmentAsync(Appointment appointment)
        {
            try
            {
                _context.Appointments.Update(AppointmentMapper.Map(appointment));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAppointmentAsync(Appointment appointment)
        {
            try
            {
                _context.Appointments.Remove(AppointmentMapper.Map(appointment));
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
