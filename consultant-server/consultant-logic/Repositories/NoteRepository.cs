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
    public class NoteRepository : INoteRepository
    {
        private readonly khbatlzvContext _context;

        public NoteRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNoteToCaseAsync(Case targetCase, Note note, bool save = true)
        {
            try
            {
                // First, add the note
                note.CaseId = targetCase.Id;
                _context.Casenotes.Add(CaseNoteMapper.Map(note));

                // Finally, update the case model and db
                targetCase.Notes.Add(note);

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<List<Note>> GetAllNotesForCaseAsync(Case targetCase)
        {
            try
            {
                return _context.Casenotes.Where(cn => cn.Caseid == targetCase.Id)
                    .Select(CaseNoteMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Note>();
            }
        }

        public async Task<Note> GetNoteByIdAsync(int noteId)
        {
            try
            {
                Casenotes dbNote = await _context.Casenotes.FirstOrDefaultAsync(n => n.Noteid == noteId);

                if (dbNote == null)
                    return null;

                return CaseNoteMapper.Map(dbNote);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateNoteAsync(Note note, bool save = true)
        {
            try
            {
                Casenotes dbNote = await _context.Casenotes.FirstOrDefaultAsync(n => n.Noteid == note.Id);
                dbNote.Content = note.Content;

                _context.Casenotes.Update(dbNote);

                if (save)
                    await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNoteFromCaseAsync(Case targetCase, Note note, bool save = true)
        {
            try
            {
                // First, remove the note from the case model
                targetCase.Notes.Remove(note);

                // Finally, remove the note
                _context.Casenotes.Remove(await _context.Casenotes.FirstOrDefaultAsync(n => n.Noteid == note.Id));

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
