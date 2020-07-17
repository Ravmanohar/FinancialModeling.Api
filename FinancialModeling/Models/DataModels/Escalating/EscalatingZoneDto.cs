using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class EscalatingZoneDto
    {
        public int ZoneId { get; set; }
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }
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

        public OperatingDaysDto OperatingDays { get; set; }

        public EscalatingOperatingHourDto EscalatingOperatingHourDaily { get; set; }
        public EscalatingOperatingHourDto EscalatingOperatingHourEvening { get; set; }

        public List<ClientPermitTypeDto> ClientPermitTypes { get; set; }
        public bool IsModified { get; set; }
        public string DailyHourlyPercentValuesJson { get; set; }
        public List<HourlyPercentValue> DailyHourlyPercentValuesList { get; set; }
    }

    public class HourlyPercentValue
    {
        public int Hour { get; set; }
        public decimal Percent { get; set; }
    }
}