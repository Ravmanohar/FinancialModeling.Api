using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class ClientPermitTypeDto
    {
        public int PermitId { get; set; }
        public int PermitCode { get; set; }

        public string PermitName { get; set; }
        public int AnnualCost { get; set; }
        public int QuantitySold { get; set; }

        public int ClientId { get; set; }
        public int ClientModelId { get; set; }
        public int ZoneId { get; set; }
    }
}