using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.CustomHandler
{
    public class SqlErrorLogging
    {
        readonly FinancialModelingDbContext _context;

        public SqlErrorLogging()
        {
        }
        public SqlErrorLogging(FinancialModelingDbContext context)
        {
            _context = context;
        }

        public void InsertErrorLog(ApiError apiError)
        {
            try
            {
                using (var dbContext = new FinancialModelingDbContext())
                {
                    //ApiError apiErrorObj = new ApiError();
                    //apiErrorObj.ApiErrorId = apiError.ApiErrorId;
                    //apiErrorObj.Message = apiError.Message;
                    //apiErrorObj.RequestMethod = apiError.RequestMethod;
                    //apiErrorObj.RequestUri = apiError.RequestUri;
                    //apiErrorObj.TimeUtc = apiError.TimeUtc;
                    dbContext.ApiErrors.Add(apiError);
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
