using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class TimeOfDayZone
    {
        [Key]
        public int ZoneId { get; set; }
        public int ClientModelId { get; set; }
        public int ClientId { get; set; }

        public int NumberOfSpacesPerZone { get; set; }
        public int PercentOfSpaceOccupied { get; set; }
        public int NumberOfSpacesRemaining { get; set; }

        public int CompliancePercentage { get; set; }

        public int ZoneCode { get; set; }//References Zone
        public bool IsActive { get; set; }
    }
}