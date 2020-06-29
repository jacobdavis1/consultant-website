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
    public class CaseStatusRepository : ICaseStatusRepository
    {
        private readonly khbatlzvContext _context;

        public CaseStatusRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCaseStatusAsync(CaseStatus caseStatus)
        {
            try
            {
                _context.Casestatuses.Add(CaseStatusMapper.Map(caseStatus));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<CaseStatus> GetCaseStatusByIdAsync(Guid statusId)
        {
            try
            {
                return CaseStatusMapper.Map(await _context.Casestatuses.FindAsync(statusId.ToString()));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<CaseStatus> GetCaseStatusByTextAsync(string text)
        {
            try
            {
                return CaseStatusMapper.Map(_context.Casestatuses.FirstOrDefault(s => s.Statustext == text));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<bool> UpdateCaseStatusAsync(CaseStatus caseStatus)
        {
            try
            {
                _context.Casestatuses.Update(_context.Casestatuses.FirstOrDefault(C => C.Statusid == caseStatus.Id.ToString()));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCaseStatusAsync(CaseStatus caseStatus)
        {
            try
            {
                _context.Casestatuses.Remove(_context.Casestatuses.FirstOrDefault(C => C.Statusid == caseStatus.Id.ToString()));
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
