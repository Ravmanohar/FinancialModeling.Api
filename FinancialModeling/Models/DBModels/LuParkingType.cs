using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class LuParkingType
    {
        [Key]
        public int ParkingTypeId { get; set; }
        public string ParkingTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}