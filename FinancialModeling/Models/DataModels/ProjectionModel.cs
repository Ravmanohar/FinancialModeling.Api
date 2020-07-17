using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class ProjectionModel
    {
        public int ProjectionId { get; set; }
        public string ProjectionName { get; set; }
        public int ClientId { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedById { get; set; }
        public string ModifiedById { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public FinanclialDashboardModel FinancialDashboard { get; set; }
    }
}