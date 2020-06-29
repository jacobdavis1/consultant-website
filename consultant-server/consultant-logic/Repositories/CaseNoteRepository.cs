using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using consultant_data.Database;
using consultant_data.Mappers;
using consultant_data.Models;
using consultant_data.RepositoryInterfaces;

namespace consultant_logic.Repositories
{
    public class CaseNoteRepository : ICaseNoteRepository
    {
        private readonly khbatlzvContext _context;

        public CaseNoteRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddNoteAsync(CaseNote note)
        {
            try
            {
                _context.Casenotes.Add(CaseNoteMapper.Map(note));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<CaseNote> GetNoteByIdAsync(Guid noteId)
        {
            try
            {
                return CaseNoteMapper.Map(await _context.Casenotes.FindAsync(noteId.ToString()));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<CaseNote>> GetAllNotesForCaseAsync(Case targetCase)
        {
            try
            {
                return _context.Casenotes.Where(cn => cn.Caseid == targetCase.Id.ToString())
                    .Select(CaseNoteMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<CaseNote>();
            }
        }

        public async Task<bool> UpdateNoteAsync(CaseNote note)
        {
            try
            {
                _context.Casenotes.Update(_context.Casenotes.FirstOrDefault(n => n.Noteid == note.Id.ToString()));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteNoteAsync(CaseNote note)
        {
            try
            {
                _context.Casenotes.Remove(_context.Casenotes.FirstOrDefault(n => n.Noteid == note.Id.ToString()));
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
