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
    }
}
