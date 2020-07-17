using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class PermitDetail
    {
        [Key]
        public int PermitId { get; set; }
        public int AnnualCost { get; set; }
        public int QuantitySold { get; set; }

        public int PermitCode { get; set; }//Refereces Permit

        public int ClientId { get; set; }
        public int ClientModelId { get; set; }
        //public int ZoneInfoId { get; set; }
        public int ZoneId { get; set; }
        public bool IsActive { get; set; }
    }
}