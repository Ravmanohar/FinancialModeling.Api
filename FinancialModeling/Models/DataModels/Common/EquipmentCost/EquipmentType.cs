using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class EquipmentType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
    }
}