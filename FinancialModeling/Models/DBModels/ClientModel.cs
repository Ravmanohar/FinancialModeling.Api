using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class ClientModel
    {
        [Key]
        public int ClientModelId { get; set; }
        public int ClientId { get; set; }
        public int ParkingTypeId { get; set; }
        public int ModelTypeId { get; set; }
        public bool IsSetupDone { get; set; }
        public bool IsAvailable { get; set; }
        public string CreatedById { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}