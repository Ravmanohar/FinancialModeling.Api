using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class ParkingClientModel
    {
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public string CreatedById { get; set; }
        public int GaragesPermitCount { get; set; }
        public int GaragesZoneCount { get; set; }
        public int NumberOfUsers { get; set; }
        public int OffStreetPermitCount { get; set; }
        public int OffStreetZoneCount { get; set; }
        public int OnStreetPermitCount { get; set; }
        public int OnStreetZoneCount { get; set; }
        public bool IsActive { get; set; }
    }
}