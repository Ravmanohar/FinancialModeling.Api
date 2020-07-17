using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FinancialModeling.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancialModeling.Repository
{
    public class AccountRepository : IAccountRepository
    {
        FinancialModelingDbContext _context;
        static string dbConnection = ConfigurationManager.ConnectionStrings["FinancialModelingConnection"].ConnectionString;
        public AccountRepository(FinancialModelingDbContext context)
        {
            _context = context;
        }
        public async Task<List<UserModel>> GetUsersByClientId(int clientId)
        {
            return await (from u in _context.Users.AsNoTracking().Where(x => x.ClientId == clientId)
                    select new UserModel
                    {
                        UserId = u.Id,
                        Username = u.UserName,
                        Email = u.Email,
                        ClientId = u.ClientId,
                        CreatedBy = u.CreatedBy,
                        CreatedDate = u.CreatedDate,
                        ModifiedBy = u.ModifiedBy,
                        ModifiedDate = u.ModifiedDate,
                        IsActive = u.IsActive
                    }).ToListAsync();
        }

        public UserModel GetUser(string userId)
        {
            ApplicationUser appUser = _context.Users.Find(userId);
            UserModel userModel = new UserModel();
            userModel.UserId = appUser.Id;
            userModel.Email = appUser.Email;
            userModel.Username = appUser.UserName;
            userModel.ClientId = appUser.ClientId;
            userModel.CreatedBy = appUser.CreatedBy;
            userModel.CreatedDate = appUser.CreatedDate;
            userModel.ModifiedBy = appUser.ModifiedBy;
            userModel.ModifiedDate = appUser.ModifiedDate;
            userModel.IsActive = appUser.IsActive;
            return userModel;
        }

        public bool DeleteUser(string userId, string modifiedBy, bool isActive)
        {
            ApplicationUser appUser = _context.Users.Find(userId);
            if (appUser != null)
            {
                appUser.IsActive = isActive;
                appUser.ModifiedBy = modifiedBy;
                appUser.ModifiedDate = DateTime.UtcNow;
                _context.Entry(appUser).State = EntityState.Modified;
                _context.SaveChangesAsync();
            }
            return true;
        }
    }
}