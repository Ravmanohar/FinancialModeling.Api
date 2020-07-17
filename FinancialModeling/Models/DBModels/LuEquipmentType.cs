using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class LuEquipmentType
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public bool IsActive { get; set; }
    }
}