using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using consultant_server.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace consultant_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
        JwtBearerDefaults.AuthenticationScheme)]
    public class AppointmentController : ControllerBase
    {
        private IUserRepository _user;
        private ICaseRepository _case;
        private IAppointmentRepository _appointment;
        private IAuthProvider _auth;

        public AppointmentController(IUserRepository user, ICaseRepository caseRepo, IAppointmentRepository appointment, IAuthProvider auth)
        {
            _user = user;
            _case = caseRepo;
            _appointment = appointment;
            _auth = auth;
        }

        //i.GET - /appointment/{caseId}/all - Get all the appointments for this case
        [HttpGet("{caseId}/all")]
        public async Task<ActionResult<IEnumerable<Appointment>>> GetAllAppointmentsForCase(int caseId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);

                if (user == null)
                    return Forbid();

                if (aCase == null)
                    return NotFound();

                if (aCase.Clients.Contains(user) || user.Role.Id == Role.Consultant.Id)
                {
                    return aCase.Appointments.ToList();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //ii.GET - /appointment/{caseId}/{appointmentId} - Get the appointment with this ID.
        [HttpGet("{caseId}/{appointmentId}")]
        public async Task<ActionResult<Appointment>> GetAppointmentById(int caseId, int appointmentId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);
                Appointment appointment = await _appointment.GetAppointmentByIdAsync(appointmentId);

                if (user == null)
                    return Forbid();

                if (aCase == null || appointment == null)
                    return NotFound();

                if (aCase.Clients.Contains(user) || user.Role.Id == Role.Consultant.Id)
                {
                    return appointment;
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }
        
        //iii.POST - /appointment/{caseId}/new - Create a new appointment for a given case. (auth)
        [HttpPost("{caseId}/new")]
        public async Task<ActionResult> PostAppointmentToCase(int caseId, [FromBody] Appointment appointment)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);

                if (user == null)
                    return Forbid();

                if (aCase == null)
                    return NotFound();

                if (aCase.Clients.Contains(user) || user.Role.Id == Role.Consultant.Id)
                {
                    await _appointment.AddAppointmentToCaseAsync(aCase, appointment);
                    return NoContent();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //iv.PUT - /appointment/{caseId}/{appointmentId} - Update this appointment with given datetime or name (auth, either consultant or related party)
        [HttpPut("{caseId}/{appointmentId}")]
        public async Task<ActionResult> PutAppointmentToCase(int caseId, int appointmentId, [FromBody] Appointment appointment)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);
                Appointment existingAppointment = await _appointment.GetAppointmentByIdAsync(appointmentId);

                if (user == null)
                    return Forbid();

                if (aCase == null || existingAppointment == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    await _appointment.UpdateAppointmentAsync(appointment);
                    return NoContent();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //v. DELETE - /appointment/{caseId}/{appointmentId}/cancel - Cancels an appointment, deleting it from the server (auth)
        [HttpDelete("{caseId}/{appointmentId}")]
        public async Task<ActionResult> DeleteAppointmentFromCase(int caseId, int appointmentId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);
                Appointment appointment = await _appointment.GetAppointmentByIdAsync(appointmentId);

                if (user == null)
                    return Forbid();

                if (aCase == null || appointment == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    await _appointment.DeleteAppointmentFromCaseAsync(aCase, appointment);
                    return NoContent();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

    }
}
