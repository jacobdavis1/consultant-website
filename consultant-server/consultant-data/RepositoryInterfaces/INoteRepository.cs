using consultant_data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace consultant_data.RepositoryInterfaces
{
    public interface INoteRepository
    {
        Task<Note> AddNoteToCaseAsync(Case targetCase, Note note, bool save = true);

        Task<List<Note>> GetAllNotesForCaseAsync(Case targetCase);

        Task<Note> GetNoteByIdAsync(int noteId);

        Task<Note> UpdateNoteAsync(Note note, bool save = true);

        Task<bool> DeleteNoteFromCaseAsync(Case targetCase, Note note, bool save = true);
    }
}
