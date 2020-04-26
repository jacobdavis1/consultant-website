using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_logic.Models
{
    class Consultant
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
    }
}
