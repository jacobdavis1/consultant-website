﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_data.Models;

namespace consultant_data.RepositoryInterfaces
{
    public interface ICaseStatusRepository
    {
        Task<bool> AddCaseStatusAsync(CaseStatus caseStatus);

        Task<CaseStatus> GetCaseStatusByIdAsync(Guid statusId);

        Task<CaseStatus> GetCaseStatusByTextAsync(string text);

        Task<bool> UpdateCaseStatusAsync(CaseStatus caseStatus);

        Task<bool> DeleteCaseStatusAsync(CaseStatus caseStatus);
    }
}