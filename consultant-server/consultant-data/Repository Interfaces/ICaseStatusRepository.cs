﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using consultant_data.Models;

namespace consultant_data.RepositoryInterfaces
{
    public interface ICaseStatusRepository
    {
        Task<Status> AddCaseStatusAsync(Status caseStatus, bool save = true);

        Task<Status> GetCaseStatusByIdAsync(int statusId);

        Task<Status> GetCaseStatusByTextAsync(string text);

        Task<Status> UpdateCaseStatusAsync(Status caseStatus, bool save = true);

        Task<bool> DeleteCaseStatusAsync(Status caseStatus, bool save = true);
    }
}
