using consultant_data.Database;
using consultant_data.Mappers;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consultant_logic.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly khbatlzvContext _context;

        public AppointmentRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddAppointmentToCaseAsync(Case targetCase, Appointment appointment, bool save = true)
        {
            try
            {
                // First, add the appointment to appointments
                appointment.CaseId = targetCase.Id;
                _context.Appointments.Add(AppointmentMapper.Map(appointment));

                // Finally, add the appointment to the case model's appointments
                targetCase.Appointments.Add(appointment);

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
                    .Where(a => a.Case.Caseid == targetCase.Id)
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
                    .Where(a => a.Case.Activeconsultantid == consultant.Id)
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
                    .Where(a => a.Appointmentdatetime.Date.CompareTo(dateTime.Date) == 0)
                    .Select(AppointmentMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Appointment>();
            }
        }

        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId)
        {
            try
            {
                Appointments dbAppointment = await _context.Appointments.FindAsync(appointmentId);

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
                Appointments dbAppointment = await _context.Appointments.FirstOrDefaultAsync(a => a.Appointmentid == appointment.Id);
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
                targetCase.Appointments.Remove(appointment);

                // Finally, remove the appointment
                _context.Appointments.Remove(await _context.Appointments.FirstOrDefaultAsync(a => a.Appointmentid == appointment.Id));

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
