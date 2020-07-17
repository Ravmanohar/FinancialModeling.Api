using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DBModels
{
    public class ApiError
    {
        [Key]
        public int ApiErrorId { get; set; }
        public string Message { get; set; }
        public string RequestMethod { get; set; }
        public string RequestUri { get; set; }
        public DateTime TimeUtc { get; set; }
    }
}