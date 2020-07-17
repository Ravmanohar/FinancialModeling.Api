using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class EscalatingZone
    {
        [Key]
        public int ZoneId { get; set; }
        //public int ZoneId { get; set; }
        //public string ZoneCode { get; set; }
        //public string ZoneName { get; set; }
        public int ClientModelId { get; set; }
        public int ClientId { get; set; }

        public decimal NonPeakHourlyRate { get; set; }
        public decimal NonPeakEscalatingRate { get; set; }
        public decimal NonPeakHourEscalatingRateBegins { get; set; }
        public decimal NonPeakDailyMaxOrAllDayRate { get; set; }
        public decimal NonPeakEveningFlatRate { get; set; }

        public decimal PeakHourlyRate { get; set; }
        public decimal PeakEscalatingRate { get; set; }
        public decimal PeakHourEscalatingRateBegins { get; set; }
        public decimal PeakDailyMaxOrAllDayRate { get; set; }
        public decimal PeakEveningFlatRate { get; set; }


        public int NumberOfSpacesPerZone { get; set; }
        public int PercentOfSpaceOccupied { get; set; }
        public int NumberOfSpacesRemaining { get; set; }

        public int CompliancePercentage { get; set; }

        public int NonPeakOccupancyPercentage { get; set; }
        public int PeakOccupancyPercentage { get; set; }
        public int ZoneCode { get; set; }//References Zone
        public bool IsActive { get; set; }
        public string DailyHourlyPercentValuesJson { get; set; }
    }
}