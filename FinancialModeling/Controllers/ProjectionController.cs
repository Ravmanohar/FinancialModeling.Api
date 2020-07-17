using FinancialModeling.Models;
using FinancialModeling.Models.DataModels.Report;
using FinancialModeling.Services;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FinancialModeling.Controllers
{
    public class ProjectionController : ApiController
    {
        private IProjectionService _projectionService;

        public ProjectionController(IProjectionService projectionService)
        {
            _projectionService = projectionService;
        }

        [HttpPost]
        public ProjectionModel CreateProjection(ProjectionModel projectionModel)
        {
            return _projectionService.CreateProjection(projectionModel);
        }

        [HttpPost]
        public bool DeleteProjection(int projectionId)
        {
            return _projectionService.DeleteProjection(projectionId);
        }

        [HttpGet]
        public ProjectionModel GetProjectionById(int projectionId)
        {
            return _projectionService.GetProjectionById(projectionId);
        }

        [HttpGet]
        public List<ProjectionModel> GetProjectionList(int clientId)
        {
            return _projectionService.GetProjectionList(clientId);
        }

        [HttpPost]
        public bool UpdateProjection(ProjectionModel projectionModel)
        {
            return _projectionService.UpdateProjection(projectionModel);
        }

    }

}
