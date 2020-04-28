using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

using consultant_data.Database;
using consultant_data.Mappers;
using consultant_logic.Models;
using consultant_logic.RepositoryInterfaces;

namespace consultant_data.Repositories
{
    class CaseRepository
    {
        private readonly khbatlzvContext _context;

        public CaseRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddCaseAsync(Case targetCase)
        {
            try
            {
                await _context.Cases.AddAsync(CaseMapper.MapCase(targetCase));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Case> GetCaseByIdAsync(Guid caseId)
        {
            try
            {
                return CaseMapper.MapCase(await _context.Cases.FindAsync(caseId.ToString()));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Case>> GetAllCasesForConsultantAsync(Consultant consultant)
        {
            try
            {
                return _context.Cases.Where(c => c.Activeconsultant.Consultantid == consultant.Id.ToString())
                    .Select(CaseMapper.MapCase)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Case>();
            }
        }

        public async Task<bool> UpdateCaseAsync(Case targetCase)
        {
            try
            {
                _context.Cases.Update(CaseMapper.MapCase(targetCase));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> DeleteCaseAsync(Case targetCase)
        {
            try
            {
                _context.Cases.Remove(CaseMapper.MapCase(targetCase));
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
