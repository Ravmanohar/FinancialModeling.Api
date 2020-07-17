using FinancialModeling.Models.DataModels.Report;
using FinancialModeling.Services;
using System.IO;
using System.Web;
using System.Web.Http;

namespace FinancialModeling.Controllers
{
    public class ReportController : ApiController
    {
        private IExportService _exportService;

        public ReportController(IExportService exportService)
        {
            _exportService = exportService;
        }

        [HttpPost]
        [AllowAnonymous]
        public void DownloadFiveYearExcelReport([FromBody] ReportModel reportModel)
        {
            byte[] bytes = _exportService.DownloadFiveYearExcelReport(reportModel);
            string downloadFileName = reportModel.FinancialDashboard.EditClientModel.ClientName + ".xlsx";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.ContentType = "application/force-download";
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" + downloadFileName);
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.Close();
            HttpContext.Current.Response.End();
        }
    }
}
