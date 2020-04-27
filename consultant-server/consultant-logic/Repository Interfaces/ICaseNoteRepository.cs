using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    interface ICaseNoteRepository
    {
        Task<bool> AddNote(CaseNote note);

        Task<CaseNote> GetNoteById(Guid noteId);

        Task<List<CaseNote>> GetAllNotesForCase(Case targetCase);

        Task<bool> UpdateNote(CaseNote note);

        Task<bool> DeleteNote(CaseNote note);
    }
}
