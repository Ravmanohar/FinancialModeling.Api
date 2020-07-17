using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModeling.Repository
{
    public interface ILookupRepository
    {
        List<LuModelType> GetLuModelTypes();
        List<LuParkingType> GetLuParkingTypes();
    }
}
