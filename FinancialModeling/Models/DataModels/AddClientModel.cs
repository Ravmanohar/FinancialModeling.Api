using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FinancialModeling.Enums.FinancialModelingEnums;

namespace FinancialModeling.Models
{
    public class AddClientModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public bool IsPeakSeasonPricing { get; set; }
        public bool HavePermits { get; set; }
        public bool IsActive { get; set; }

        public List<AddZoneModel> OnStreetZones { get; set; }
        public List<AddZoneModel> OffStreetZones { get; set; }
        public List<AddZoneModel> GaragesZones { get; set; }

        public List<AddPermitModel> OnStreetPermits { get; set; }
        public List<AddPermitModel> OffStreetPermits { get; set; }
        public List<AddPermitModel> GaragesPermits { get; set; }
    }

    public class AddZoneModel
    {
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public int? OperatingDays { get; set; }
        public ActionType ActionType { get; set; }
    }

    public class AddPermitModel
    {
        public int PermitCode { get; set; }
        public string PermitName { get; set; }
        public ActionType ActionType { get; set; }
    }
}