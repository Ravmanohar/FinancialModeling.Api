using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class EquipmentZoneModel
    {
        public int ClientId { get; set; }
        public int ZoneCode { get; set; }
        public int? OperatingDays { get; set; }
        public string ZoneName { get; set; }
        public string LocationType { get; set; }
        public int ParkingTypeId { get; set; }
        public List<EquipmentCostModel> Equipments { get; set; }
    }
}