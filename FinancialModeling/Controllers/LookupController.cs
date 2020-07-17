using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using FinancialModeling.Services;
using System.Collections.Generic;
using System.Web.Http;

namespace FinancialModeling.Controllers
{
    public class LookupController : ApiController
    {
        private ILookupService _lookupService;

        public LookupController(ILookupService lookupService)
        {
            _lookupService = lookupService;
        }

        [HttpGet]
        public List<LuModelType> GetLuModelTypes()
        {
            return _lookupService.GetLuModelTypes();
        }

        [HttpGet]
        public List<LuParkingType> GetLuParkingTypes()
        {
            return _lookupService.GetLuParkingTypes();
        }
    }
}
