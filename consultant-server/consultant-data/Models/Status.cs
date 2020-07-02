using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_data.Models
{
    public class Status
    {
        public int Id { get; set; }
        public string Text { get; set; }

        public static Status Unassigned { get; } = new Status { Id = 1, Text = "Unassigned" };
    }
}
