using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public Role Role { get; set; }

        public List<Case> Cases { get; set; } = new List<Case>();
    }
}
