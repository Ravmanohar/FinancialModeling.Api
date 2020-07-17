using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using FinancialModeling.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Services
{
    public class LookupService : ILookupService
    {
        private ILookupRepository _lookupRepository;
        public LookupService(ILookupRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }
        public List<LuModelType> GetLuModelTypes()
        {
            return _lookupRepository.GetLuModelTypes();
        }
        public List<LuParkingType> GetLuParkingTypes()
        {
            return _lookupRepository.GetLuParkingTypes();
        }
    }
}