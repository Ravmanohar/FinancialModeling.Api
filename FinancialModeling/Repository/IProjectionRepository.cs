using FinancialModeling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModeling.Repository
{
    public interface IProjectionRepository
    {
        ProjectionModel CreateProjection(ProjectionModel projectionModel);
        bool UpdateProjection(ProjectionModel projectionModel);
        bool DeleteProjection(int projectionId);
        List<ProjectionModel> GetProjectionList(int clientId);
        ProjectionModel GetProjectionById(int projectionId);
    }
}
