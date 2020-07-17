using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModeling.Services
{
    public interface ILookupService
    {
        List<LuModelType> GetLuModelTypes();
        List<LuParkingType> GetLuParkingTypes();
    }
}
