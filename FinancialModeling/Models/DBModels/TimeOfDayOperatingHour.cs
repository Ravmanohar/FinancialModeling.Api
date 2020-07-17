using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class TimeOfDayOperatingHour
    {
        [Key]
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
        public bool IsPeak { get; set; }
        public bool IsActive { get; set; }
    }
}