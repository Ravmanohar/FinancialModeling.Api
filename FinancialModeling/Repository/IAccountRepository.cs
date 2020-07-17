using FinancialModeling.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialModeling.Repository
{
    public interface IAccountRepository
    {
        Task<List<UserModel>> GetUsersByClientId(int clientId);
        UserModel GetUser(string userId);
        bool DeleteUser(string userId, string modifiedBy, bool isActive);
    }
}
