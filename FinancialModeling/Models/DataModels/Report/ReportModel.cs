using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models.DataModels.Report
{
    public class ReportModel
    {
        public FinanclialDashboardModel FinancialDashboard { get; set; }
        public FinancialDashboardRevenueModel FinancialDashboardRevenue { get; set; }
        public List<ProjectedRevenueSummary> ProjectedRevenueSummaries { get; set; }
        public List<ProjectedEquipmentCostSummary> ProjectedEquipmentCostSummaries { get; set; }
    }

    public class ProjectedRevenueSummary
    {
        public string ReportName { get; set; }
        public string ReportHeader { get; set; }
        public bool IsAvailable { get; set; }
        public List<ZoneSummary> ZoneSummaryList { get; set; }
    }
    public class ZoneSummary
    {
        public int ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public RevenueInfo NonPeak { get; set; }
        public RevenueInfo Peak { get; set; }
        public RevenueInfo Variance { get; set; }
    }

    public class RevenueInfo
    {
        public int Hourly { get; set; }
        public int Permit { get; set; }
        public int Total { get; set; }
    }

    public class ReportColumns
    {
        public int ZoneColumn { get; set; } = 1;
        public int HeaderColumn { get; set; } = 1;
        public int HourlyColumn { get; set; } = 1;
        public int PermitColumn { get; set; } = 1;
        public int TotalColumn { get; set; } = 1;
    }


    public class ProjectedEquipmentCostSummary
    {
        public string ReportName { get; set; }
        public string ReportHeader { get; set; }
        public bool IsAvailable { get; set; }
        public List<ZoneEquipmentSummary> ZoneEquipmentList { get; set; }
    }
    public class ZoneEquipmentSummary
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public int ZoneCode { get; set; }

        public decimal EquipmentCost { get; set; }
        public decimal EstimatedSoftwareFees { get; set; }
        public decimal EstimatedCreditCardTransactionFees { get; set; }
        public decimal EstimatedCostOfAdditionalSparesAndMisc { get; set; }
        public decimal SubtotalOperatingCost { get; set; }
        public decimal Total { get; set; }

        public decimal WarrantyCostYear2 { get; set; }
        public decimal WarrantyCostYear3 { get; set; }
        public decimal WarrantyCostYear4 { get; set; }
        public decimal WarrantyCostYear5 { get; set; }

        public decimal TotalEstimatedEquipmentAndOperatingCost1 { get; set; }
        public decimal TotalEstimatedEquipmentAndOperatingCost2 { get; set; }
        public decimal TotalEstimatedEquipmentAndOperatingCost3 { get; set; }
        public decimal TotalEstimatedEquipmentAndOperatingCost4 { get; set; }
        public decimal TotalEstimatedEquipmentAndOperatingCost5 { get; set; }
    }


    public class FinancialDashboardRevenueModel
    {
        public RevenueModel HourlyRevenueModel { get; set; }
        public RevenueModel TimeOfDayRevenueModel { get; set; }
        public RevenueModel EscalatingRevenueModel { get; set; }
    }
    public class RevenueModel
    {
        public Years OnStreet { get; set; }
        public Years OffStreet { get; set; }
        public Years Garages { get; set; }
    }

    public class Years
    {
        public bool IsAvailable { get; set; }
        public List<ZoneRevenueSummary> Year1 { get; set; }
        public List<ZoneRevenueSummary> Year2 { get; set; }
        public List<ZoneRevenueSummary> Year3 { get; set; }
        public List<ZoneRevenueSummary> Year4 { get; set; }
        public List<ZoneRevenueSummary> Year5 { get; set; }
        //public Dictionary<int, List<ZoneRevenueSummary>> YearsList { get; set; }
    }

    public class ZoneRevenueSummary
    {
        public int ZoneId { get; set; }
        public string ZoneName { get; set; }
        public ZoneRevenue NonPeak { get; set; }
        public ZoneRevenue Peak { get; set; }
    }

    public class ZoneRevenue
    {
        public decimal Revenue { get; set; }
        public decimal Cost { get; set; }
        public decimal Gain { get; set; }
    }
}