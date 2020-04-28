﻿using System;
using System.Collections.Generic;
using System.Text;

namespace consultant_logic.Models
{
    public class Case
    {
        public Guid Id { get; set;}
        public Consultant ActiveConsultant { get; set; }
        public CaseStatus Status { get; set; }
        public string Title { get; set; }
        public List<Client> Clients { get; set; }
        public List<Appointment> UpcomingApointments { get; set; }
        public List<CaseNote> Notes { get; set; }
    }
}
