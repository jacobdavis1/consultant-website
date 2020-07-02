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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
    public class CaseController : ControllerBase
    {
        private IUserRepository _user;
        private ICaseRepository _case;
        private IAppointmentRepository _appointment;
        private INoteRepository _note;
        private IAuthProvider _auth;

        public CaseController(IUserRepository user, ICaseRepository caseRepo, IAppointmentRepository appointment, INoteRepository note, IAuthProvider auth)
        {
            _user = user;
            _case = caseRepo;
            _appointment = appointment;
            _note = note;
            _auth = auth;
        }

        // GET: api/Case
        [HttpGet("all", Name = "GetAllCases")]
        public async Task<ActionResult<IEnumerable<Case>>> GetAllCases()
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);

                if (user == null)
                {
                    user = new User
                    {
                        UserId = userId,
                        Role = Role.Client
                    };

                    user = await _user.AddUserAsync(user);
                }

                if (user.Role.Text == "Consultant")
                {
                    return await _case.GetAllCasesForConsultantAsync(user);
                }
                else if (user.Role.Text == "Client")
                {
                    return user.Cases.ToList();
                }
                else
                    return new List<Case> { new Case() };
            }
            catch (Exception e)
            {
                return null;
            }
        }

        // GET: api/Case/5
        [HttpGet("{id}", Name = "GetCase")]
        public string GetCase(int id)
        {
            return "value";
        }

        // POST: api/Case
        [HttpPost]
        public void PostCase([FromBody] string value)
        {
        }

        // PUT: api/Case/5
        [HttpPut("{id}")]
        public void PutCase(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void DeleteCase(int id)
        {
        }
    }
}
