using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class OperatingDaysDto
    {
        public int Id { get; set; }
        public int DaysPerYear { get; set; }
        public int PeakDays { get; set; }
        public int OffDays { get; set; }
        public int NonPeakDays { get; set; }

        public int ClientId { get; set; }
        public int ClientModelId { get; set; }
        public int ZoneId { get; set; }
    }
}