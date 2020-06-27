using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Client
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public List<Case> Cases { get; set; }
    }
}
