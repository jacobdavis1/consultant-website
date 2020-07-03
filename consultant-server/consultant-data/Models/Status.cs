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

        public override bool Equals(Object obj)
        {
            Status other = obj as Status;

            if (other == null) return false;

            return (Id == other.Id && Text == other.Text);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
