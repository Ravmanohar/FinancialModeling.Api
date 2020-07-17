using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class Permit
    {
        [Key]
        public int PermitCode { get; set; }
        public string PermitName { get; set; }

        public int ClientId { get; set; }
        public int ZoneId { get; set; }
        public int ParkingTypeId { get; set; }
        public bool IsActive { get; set; }
    }
}

//public class ClientPermitType
//{
//    [Key]
//    public int PermitId { get; set; }
//    public string PermitName { get; set; }
//    public int AnnualCost { get; set; }
//    public int QuantitySold { get; set; }

//    public int ClientId { get; set; }
//    public int ClientModelId { get; set; }
//    public int ZoneId { get; set; }
//}