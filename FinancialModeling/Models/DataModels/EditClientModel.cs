using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class EditClientModel
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

        public EditClientModel()
        {
            OnStreetZones = new List<AddZoneModel>();
            OffStreetZones = new List<AddZoneModel>();
            GaragesZones = new List<AddZoneModel>();

            OnStreetPermits = new List<AddPermitModel>();
            OffStreetPermits = new List<AddPermitModel>();
            GaragesPermits = new List<AddPermitModel>();
        }
    }
}