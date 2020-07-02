using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using consultant_data.Database;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using consultant_server.ServiceInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Npgsql.Logging;

namespace consultant_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
        JwtBearerDefaults.AuthenticationScheme)]
    public class CaseController : ControllerBase
    {
        private IUserRepository _user;
        private ICaseRepository _case;
        private IAuthProvider _auth;
        private ICaseStatusRepository _status;

        public CaseController(IUserRepository user, ICaseRepository caseRepo, IAuthProvider auth, ICaseStatusRepository status)
        {
            _user = user;
            _case = caseRepo;
            _auth = auth;
            _status = status;
        }

        //i.GET - /case/all - Get all cases for this client
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Case>>> GetAllCases()
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    return await _case.GetAllCasesForConsultantAsync(user);
                }
                else if (user.Role.Id == Role.Client.Id)
                {
                    return await _case.GetAllCasesForClientAsync(user);
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //ii. GET - /case/{caseId} - Get the case with this ID.
        [HttpGet("{caseId}")]
        public async Task<ActionResult<Case>> GetCase(int caseId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                Case c = await _case.GetCaseByIdAsync(caseId);

                if (c == null)
                    return NotFound();

                if (c.Clients.Contains(user) || user.Role.Id == Role.Consultant.Id)
                    return c;
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //iii.PUT - /case/{caseId}/assignTo/{consultantId} - Reassign this case to the given consultant (auth, consultant)
        [HttpPut("{caseId}/assignTo/{consultantId}")]
        public async Task<ActionResult> ReassignCase(int caseId, int consultantId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                Case c = await _case.GetCaseByIdAsync(caseId);
                User consultant = await _user.GetUserByRowIdAsync(consultantId);

                if (c == null || consultant == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    c.ActiveConsultant = consultant;
                    await _case.UpdateCaseAsync(c);
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

        //iv. PUT - /case/{caseId}/status?status={ newStatusText } - Update the status to a new status. A status that doesnt exist will get 404 (auth, consultant)
        [HttpPut("{caseId}/status")]
        public async Task<ActionResult> UpdateCaseStatus(int caseId, [FromQuery] string statusText)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                Case c = await _case.GetCaseByIdAsync(caseId);
                Status s = await _status.GetCaseStatusByTextAsync(statusText);

                if (c == null || s == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    c.Status = s;
                    await _case.UpdateCaseAsync(c);
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

        //v. PUT - /case/{caseId} - Update any aspect of a case with the given ID.
        [HttpPut("{caseId}")]
        public async Task<ActionResult> PutCase(int caseId, [FromBody] Case aCase)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                Case c = await _case.GetCaseByIdAsync(caseId);
                


                if (c == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    c = aCase;
                    await _case.UpdateCaseAsync(c);
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

        //vi.POST - /case/new - Create a new case for this client. Give it to the given consultant
        [HttpPost("new")]
        public async Task<ActionResult> PostCase([FromBody] Case aCase)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    await _case.AddCaseAsync(aCase);
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

        //vii. DELETE - /case/{caseId} - Delete the case with this ID.
        [HttpDelete("{caseId}")]
        public async Task<ActionResult> DeleteCase(int caseId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                    return Forbid();

                Case c = await _case.GetCaseByIdAsync(caseId);

                if (c == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    await _case.DeleteCaseAsync(c);
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
