using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class ParkingClient
    {
        [Key]
        public int ClientId { get; set; }
        public string ClientName { get; set; }

        public int OnStreetZoneCount { get; set; }
        public int OffStreetZoneCount { get; set; }
        public int GaragesZoneCount { get; set; }

        public int OnStreetPermitCount { get; set; }
        public int OffStreetPermitCount { get; set; }
        public int GaragesPermitCount { get; set; }


        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public int NumberOfUsers { get; set; }
        public bool IsPeakSeasonPricing { get; set; }
        public bool HavePermits { get; set; }
        public bool IsActive { get; set; }
    }
}