using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class HourlyZone
    {
        [Key]
        public int ZoneId { get; set; }
        //public int ZoneId { get; set; }
        //public string ZoneCode { get; set; }
        //public string ZoneName { get; set; }
        public int ClientModelId { get; set; }
        public int ClientId { get; set; }

        public decimal NonPeakSeasonHourlyRate { get; set; }
        public decimal PeakSeasonHourlyRate { get; set; }

        public int NumberOfSpacesPerZone { get; set; }
        public int PercentOfSpaceOccupied { get; set; }
        public int NumberOfSpacesRemaining { get; set; }

        public int CompliancePercentage { get; set; }

        public int NonPeakOccupancyPercentage { get; set; }
        public int PeakOccupancyPercentage { get; set; }

        public int ZoneCode { get; set; }//References Zone
        public bool IsActive { get; set; }
    }
}