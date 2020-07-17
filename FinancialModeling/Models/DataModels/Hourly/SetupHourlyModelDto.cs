using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class SetupHourlyModelDto
    {
        public int ClientModelId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int ParkingTypeId { get; set; }
        public ParkingTypeDto SelectedParkingType { get; set; }
        public int ModelTypeId { get; set; }
        public ModelTypeDto SelectedModelType { get; set; }
        public List<HourlyZoneDto> HourlyZones { get; set; }
        public bool IsSetupDone { get; set; }
        public bool IsAvailable { get; set; }
    }
}