﻿using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class CaseNoteMapper
    {
        public static CaseNote Map(Database.Casenotes caseNote)
        {
            return new CaseNote
            {
                Id = Guid.Parse(caseNote.Noteid),
                Content = caseNote.Content
            };
        }

        public static Database.Casenotes Map(CaseNote caseNote)
        {
            return new Database.Casenotes
            {
                Noteid = caseNote.Id.ToString(),
                Content = caseNote.Content
            };
        }
    }
}