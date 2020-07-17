using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using Newtonsoft.Json;

namespace FinancialModeling.Repository
{
    public class ProjectionRepository : IProjectionRepository
    {
        FinancialModelingDbContext _context;
        static string dbConnection = ConfigurationManager.ConnectionStrings["FinancialModelingConnection"].ConnectionString;
        public ProjectionRepository(FinancialModelingDbContext context)
        {
            _context = context;
        }

        public ProjectionModel CreateProjection(ProjectionModel projectionModel)
        {
            Projection projection = new Projection();
            projection.ProjectionId = projectionModel.ProjectionId;
            projection.ProjectionName = projectionModel.ProjectionName;
            projection.ClientId = projectionModel.ClientId;
            projection.UserId = projectionModel.UserId;
            projection.CreatedDate = DateTime.UtcNow;
            projection.CreatedById = projectionModel.CreatedById;
            projection.ModifiedById = projectionModel.ModifiedById;
            projection.ModifiedDate = null;
            projection.IsActive = true;
            projection.IsDeleted = false;
            projection.FinancialDashboardJson = JsonConvert.SerializeObject(projectionModel.FinancialDashboard);

            _context.Projections.Add(projection);
            _context.SaveChanges();

            projectionModel.ProjectionId = projection.ProjectionId;
            return projectionModel;
        }

        public bool DeleteProjection(int projectionId)
        {
            Projection projection = _context.Projections.Where(x => x.ProjectionId == projectionId).FirstOrDefault();
            projection.IsDeleted = true;
            _context.Entry(projection).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }

        public ProjectionModel GetProjectionById(int projectionId)
        {
            Projection projection = _context.Projections.AsNoTracking().Where(x => x.ProjectionId == projectionId).FirstOrDefault();
            ProjectionModel projectionModel = new ProjectionModel();
            if (projection != null)
            {
                projectionModel.ProjectionId = projection.ProjectionId;
                projectionModel.ProjectionName = projection.ProjectionName;
                projectionModel.ClientId = projection.ClientId;
                projectionModel.UserId = projection.UserId;
                projectionModel.CreatedDate = projection.CreatedDate;
                projectionModel.CreatedById = projection.CreatedById;
                projectionModel.ModifiedById = projection.ModifiedById;
                projectionModel.ModifiedDate = projection.ModifiedDate;
                projectionModel.IsActive = projection.IsActive;
                projectionModel.FinancialDashboard = JsonConvert.DeserializeObject<FinanclialDashboardModel>(projection.FinancialDashboardJson);
            }
            return projectionModel;
        }

        public List<ProjectionModel> GetProjectionList(int clientId)
        {
            return (from p in _context.Projections.AsNoTracking().Where(x => x.ClientId == clientId && x.IsDeleted == false).AsEnumerable()
                    select new ProjectionModel
                    {
                        ProjectionId = p.ProjectionId,
                        ProjectionName = p.ProjectionName,
                        ClientId = p.ClientId,
                        UserId = p.UserId,
                        CreatedDate = p.CreatedDate,
                        CreatedById = p.CreatedById,
                        ModifiedById = p.ModifiedById,
                        ModifiedDate = p.ModifiedDate,
                        IsActive = p.IsActive,
                        FinancialDashboard = null
                    }).ToList();
        }

        public bool UpdateProjection(ProjectionModel projectionModel)
        {
            Projection projection = _context.Projections.Where(x => x.ProjectionId == projectionModel.ProjectionId).FirstOrDefault();
            if (projectionModel.FinancialDashboard != null)
                projection.FinancialDashboardJson = JsonConvert.SerializeObject(projectionModel.FinancialDashboard);
            projection.ModifiedById = projectionModel.ModifiedById;
            projection.ModifiedDate = DateTime.UtcNow;
            projection.ProjectionName = projectionModel.ProjectionName;
            _context.Entry(projection).State = EntityState.Modified;
            _context.SaveChanges();
            return true;
        }
    }
}