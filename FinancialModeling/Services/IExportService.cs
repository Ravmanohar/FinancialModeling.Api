using FinancialModeling.Models.DataModels.Report;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModeling.Services
{
    public interface IExportService
    {
        byte[] DownloadFiveYearExcelReport(ReportModel reportModel);
    }
}
