using FinancialModeling.Models;
using FinancialModeling.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace FinancialModeling.Services
{
    public class AccountService : IAccountService
    {
        private IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public async Task<List<UserModel>> GetUsersByClientId(int clientId)
        {
            return await _accountRepository.GetUsersByClientId(clientId);
        }

        public UserModel GetUser(string userId)
        {
            return _accountRepository.GetUser(userId);
        }

        public bool DeleteUser(string userId, string modifiedBy, bool isActive)
        {
            return _accountRepository.DeleteUser(userId, modifiedBy, isActive);
        }
    }
}