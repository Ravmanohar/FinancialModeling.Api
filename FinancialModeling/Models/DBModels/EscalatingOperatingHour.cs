using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class EscalatingOperatingHour
    {
        [Key]
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int ZoneId { get; set; }
        public int ClientModelId { get; set; }

        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public int TotalHours { get; set; }
        public int OperatingHourType { get; set; }//"Daily";//Evening
        public bool IsActive { get; set; }
    }
    public enum OperatingHourType
    {
        Daily = 1,
        Evening = 2,
    }
}