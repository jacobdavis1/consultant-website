﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using consultant_logic.Models;

namespace consultant_logic.RepositoryInterfaces
{
    public interface ICaseNoteRepository
    {
        Task<bool> AddNoteAsync(CaseNote note);

        Task<CaseNote> GetNoteByIdAsync(Guid noteId);

        Task<List<CaseNote>> GetAllNotesForCaseAsync(Case targetCase);

        Task<bool> UpdateNoteAsync(CaseNote note);

        Task<bool> DeleteNoteAsync(CaseNote note);
    }
}
