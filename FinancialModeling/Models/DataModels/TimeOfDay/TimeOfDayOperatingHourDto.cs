using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FinancialModeling.Enums.FinancialModelingEnums;

namespace FinancialModeling.Models
{
    public class TimeOfDayOperatingHourDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ZoneId { get; set; }
        public int ClientModelId { get; set; }

        public decimal PeakSeasonHourlyRate { get; set; }
        public decimal NonPeakSeasonHourlyRate { get; set; }
        public string OperatingHoursStart { get; set; }
        public string OperatingHoursEnd { get; set; }
        public int TotalHours { get; set; }

        public int NonPeakOccupancyPercentage { get; set; }
        public int PeakOccupancyPercentage { get; set; }

        public ActionType ActionType { get; set; }
    }
}