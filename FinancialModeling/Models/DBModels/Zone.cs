using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class Zone
    {
        [Key]
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public int? OperatingDays { get; set; }
        public int ClientId { get; set; }
        public int ParkingTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}