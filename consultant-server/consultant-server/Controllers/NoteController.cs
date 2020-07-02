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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace consultant_server.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes =
        JwtBearerDefaults.AuthenticationScheme)]
    public class NoteController : ControllerBase
    {
        private IUserRepository _user;
        private ICaseRepository _case;
        private INoteRepository _note;
        private IAuthProvider _auth;

        public NoteController(IUserRepository user, ICaseRepository caseRepo, INoteRepository note, IAuthProvider auth)
        {
            _user = user;
            _case = caseRepo;
            _note = note;
            _auth = auth;
        }

        //i.GET - /note/{caseId}/all - Get all notes for this case
        [HttpGet("{caseId}/all")]
        public async Task<ActionResult<IEnumerable<Note>>> GetAllNotes(int caseId)
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
                    return aCase.Notes.ToList();
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //ii.GET - /note/{caseId}/{noteId} - Get the note with this ID.
        [HttpGet("{caseId}/{noteId}")]
        public async Task<ActionResult<Note>> GetNoteById(int caseId, int noteId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);
                Note note = await _note.GetNoteByIdAsync(noteId);

                if (user == null)
                    return Forbid();

                if (aCase == null || note == null)
                    return NotFound();

                if (aCase.Clients.Contains(user) || user.Role.Id == Role.Consultant.Id)
                {
                    return note;
                }
                else
                    return Forbid();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e);
            }
        }

        //iii.POST - /note/{caseId}/new - Create a new note for this case with the enclosed data
        [HttpPost("{caseId}/new")]
        public async Task<ActionResult> PostNoteToCase(int caseId, [FromBody] Note note)
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
                    await _note.AddNoteToCaseAsync(aCase, note);
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

        //iv.PUT - /note/{caseId}/{noteId} - Update a note with the given noteId with the enclosed data. (consultant only)
        [HttpPut("{caseId}/{noteId}")]
        public async Task<ActionResult> PutNoteToCase(int caseId, int noteId, [FromBody] Note note)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);
                Note existingNote = await _note.GetNoteByIdAsync(noteId);

                if (user == null)
                    return Forbid();

                if (aCase == null || existingNote == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    await _note.UpdateNoteAsync(note);
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

        //v.DELETE - /note/{caseId}/{noteId} - Delete the note with this ID. (consultant only)
        [HttpDelete("{caseId}/{noteId}")]
        public async Task<ActionResult> DeleteNoteFromCase(int caseId, int noteId)
        {
            try
            {
                string userId = _auth.GetUserIdFromToken(HttpContext);
                User user = await _user.GetUserByIdAsync(userId);
                Case aCase = await _case.GetCaseByIdAsync(caseId);
                Note note = await _note.GetNoteByIdAsync(noteId);

                if (user == null)
                    return Forbid();

                if (aCase == null || note == null)
                    return NotFound();

                if (user.Role.Id == Role.Consultant.Id)
                {
                    await _note.DeleteNoteFromCaseAsync(aCase, note);
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
