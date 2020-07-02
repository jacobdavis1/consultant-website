using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using consultant_data.Database;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace consultant_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
    JwtBearerDefaults.AuthenticationScheme)]
    public class CaseController : ControllerBase
    {
        private ICaseRepository _case;
        private IAppointmentRepository _appointment;
        private INoteRepository _note;

        public CaseController(ICaseRepository caseRepo, IAppointmentRepository appointment, INoteRepository note)
        {
            _case = caseRepo;
            _appointment = appointment;
            _note = note;
        }

        // GET: api/Case
        [HttpGet("all", Name = "GetAllCases")]
        public ActionResult<IEnumerable<Case>> GetAllCases()
        {
            return new List<Case> { new Case() };
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
