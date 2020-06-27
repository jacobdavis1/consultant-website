﻿using System;
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
    public class ConsultantRepository : IConsultantRepository
    {
        private readonly khbatlzvContext _context;

        public ConsultantRepository(khbatlzvContext context)
        {
            _context = context;
        }

        public async Task<bool> AddConsultantAsync(Consultant consultant)
        {
            try
            {
                await _context.Consultants.AddAsync(ConsultantMapper.Map(consultant));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<Consultant> GetConsultantByIdAsync(Guid consultantId)
        {
            try
            {
                return ConsultantMapper.Map(await _context.Consultants.FindAsync(consultantId.ToString()));
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public async Task<List<Consultant>> SearchConsultantsByNameAsync(string firstName = "", string middleName = "", string lastName = "")
        {
            try
            {
                return _context.Consultants.Where(c => (c.Firstname == firstName || firstName == "") 
                                                            && (c.Middlename == middleName || middleName == "") 
                                                            && (c.Lastname == lastName || lastName == "") )
                    .Select(ConsultantMapper.Map)
                    .ToList();
            }
            catch (Exception e)
            {
                return new List<Consultant>();
            }
        }

        public async Task<bool> UpdateConsultantAsync(Consultant consultant)
        {
            try
            {
                _context.Consultants.Update(ConsultantMapper.Map(consultant));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                return true;
            }
        }

        public async Task<bool> DeleteConsultantAsync(Consultant consultant)
        {
            try
            {
                _context.Consultants.Remove(ConsultantMapper.Map(consultant));
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