using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class CaseNote
    {
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }

        public string Content { get; set; }
    }
}
