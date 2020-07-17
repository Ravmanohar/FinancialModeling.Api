using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using FinancialModeling.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModeling.Services
{
    public class ProjectionService : IProjectionService
    {
        private IProjectionRepository _projectionRepository;

        public ProjectionService(IProjectionRepository projectionRepository)
        {
            _projectionRepository = projectionRepository;
        }

        public ProjectionModel CreateProjection(ProjectionModel projectionModel)
        {
            return _projectionRepository.CreateProjection(projectionModel);
        }

        public bool DeleteProjection(int projectionId)
        {
            return _projectionRepository.DeleteProjection(projectionId);
        }

        public ProjectionModel GetProjectionById(int projectionId)
        {
            return _projectionRepository.GetProjectionById(projectionId);
        }

        public List<ProjectionModel> GetProjectionList(int clientId)
        {
            return _projectionRepository.GetProjectionList(clientId);
        }

        public bool UpdateProjection(ProjectionModel projectionModel)
        {
            return _projectionRepository.UpdateProjection(projectionModel);
        }
    }
}