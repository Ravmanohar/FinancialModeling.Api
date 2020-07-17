using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using FinancialModeling.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Services
{
    public interface IProjectionService
    {
        ProjectionModel CreateProjection(ProjectionModel projectionModel);
        bool UpdateProjection(ProjectionModel projectionModel);
        bool DeleteProjection(int projectionId);
        List<ProjectionModel> GetProjectionList(int clientId);
        ProjectionModel GetProjectionById(int projectionId);
    }
}