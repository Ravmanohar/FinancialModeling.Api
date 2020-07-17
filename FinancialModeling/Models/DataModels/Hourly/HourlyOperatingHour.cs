using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class HourlyOperatingHourDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ZoneId { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TotalHours { get; set; }
    }
}