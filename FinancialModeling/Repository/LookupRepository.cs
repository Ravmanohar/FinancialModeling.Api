using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace FinancialModeling.Repository
{
    public class LookupRepository : ILookupRepository
    {
        FinancialModelingDbContext _context;
        static string dbConnection = ConfigurationManager.ConnectionStrings["FinancialModelingConnection"].ConnectionString;

        public LookupRepository(FinancialModelingDbContext context)
        {
            _context = context;
        }

        public List<LuModelType> GetLuModelTypes()
        {
            return _context.LuModelTypes.AsNoTracking().ToList();
        }

        public List<LuParkingType> GetLuParkingTypes()
        {
            return _context.LuParkingTypes.AsNoTracking().ToList();
        }
    }
}