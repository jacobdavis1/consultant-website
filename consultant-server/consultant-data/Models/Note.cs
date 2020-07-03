using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Note
    {
        public int Id { get; set; }
        public int CaseId { get; set; }

        public string Content { get; set; }

        public override bool Equals(Object obj)
        {
            Note other = obj as Note;

            if (other == null) return false;

            return (Id == other.Id && CaseId == other.CaseId
                        && Content == other.Content);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
