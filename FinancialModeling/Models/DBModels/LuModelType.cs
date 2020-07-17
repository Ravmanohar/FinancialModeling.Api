using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class LuModelType
    {
        [Key]
        public int ModelTypeId { get; set; }
        public string ModelTypeName { get; set; }
        public bool IsActive { get; set; }
    }
}