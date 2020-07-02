using System;
using System.Collections.Generic;
using System.Text;

using consultant_data.Models;

namespace consultant_data.Mappers
{
    public class CaseNoteMapper
    {
        public static Note Map(Database.Casenotes caseNote)
        {
            return new Note
            {
                Id = caseNote.Noteid,
                CaseId = caseNote.Caseid ?? -1,
                Content = caseNote.Content
            };
        }

        public static Database.Casenotes Map(Note caseNote)
        {
            return new Database.Casenotes
            {
                Noteid = caseNote.Id,
                Caseid = caseNote.CaseId,
                Content = caseNote.Content
            };
        }
    }
}
