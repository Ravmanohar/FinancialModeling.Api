using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class TimeOfDayZoneDto
    {
        public int ZoneId { get; set; }
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }

        public int ClientModelId { get; set; }
        public int ClientId { get; set; }

        public int NumberOfSpacesPerZone { get; set; }
        public int PercentOfSpaceOccupied { get; set; }
        public int NumberOfSpacesRemaining { get; set; }

        public int CompliancePercentage { get; set; }

        public List<TimeOfDayOperatingHourDto> HoursOfOperations { get; set; }

        public OperatingDaysDto OperatingDays { get; set; }
        public List<ClientPermitTypeDto> ClientPermitTypes { get; set; }
        public bool IsModified { get; set; }
    }
}