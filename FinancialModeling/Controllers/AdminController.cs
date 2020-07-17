using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using FinancialModeling.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FinancialModeling.Controllers
{
    //[Authorize]
    public class AdminController : ApiController
    {
        private IAdminService _adminService;
        private IAccountService _accountService;

        public AdminController(IAdminService adminService, IAccountService accountService)
        {
            _adminService = adminService;
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<ParkingClient> SetupClient(AddClientModel addClientModel)
        {
            return await _adminService.SetupClient(addClientModel);
        }

        [HttpGet]
        public async Task<List<ParkingClientModel>> GetClientList()
        {
            string userId = User.Identity.GetUserId();
            bool isAdmin = User.IsInRole("Admin");
            if (isAdmin)
            {
                return await _adminService.GetClientList();
            }
            else
            {
                int clientId = _accountService.GetUser(userId).ClientId;
                return await _adminService.GetClientList(clientId);
            }
        }

        [HttpGet]
        public async Task<ParkingClientModel> GetClientById(int clientId)
        {
            return await _adminService.GetClientById(clientId);
        }

        [HttpGet]
        public async Task<FinanclialDashboardModel> GetClientInfo(int clientId)
        {
            return await _adminService.GetClientInfo(clientId);
        }

        [HttpPost]
        public SetupHourlyModelDto UpdateHourlyModel(SetupHourlyModelDto hourlyModelDto)
        {
            return _adminService.UpdateHourlyModel(hourlyModelDto);
        }

        [HttpPost]
        public SetupTimeOfDayModelDto UpdateTimeOfDayModel(SetupTimeOfDayModelDto setupTimeOfDayModel)
        {
            return _adminService.UpdateTimeOfDayModel(setupTimeOfDayModel);
        }

        [HttpPost]
        public SetupEscalatingModelDto UpdateEscalatingModel(SetupEscalatingModelDto setupEscalatingModel)
        {
            return _adminService.UpdateEscalatingModel(setupEscalatingModel);
        }

        [HttpPost]
        public LocationEquipmentCostModel UpdateEquipmentCost(LocationEquipmentCostModel equipmentCostModel)
        {
            return _adminService.UpdateEquipmentCost(equipmentCostModel);
        }
        public async Task<List<UserModel>> GetUsersByClientId(int clientId)
        {
            return await _accountService.GetUsersByClientId(clientId);
        }

        public UserModel GetUser()
        {
            string userId = User.Identity.GetUserId();
            UserModel userModel = _accountService.GetUser(userId);
            bool isAdmin = User.IsInRole("Admin");
            userModel.Role = isAdmin ? "Admin" : "User";
            return userModel;
        }

        [HttpPost]
        public async Task<ParkingClient> UpdateClient(AddClientModel updateClientModel)
        {
            return await _adminService.UpdateClient(updateClientModel);
        }

        [HttpPost]
        public async Task<bool> UpdateModelAvailability(ClientModelDto clientModelDto)
        {
            return await _adminService.UpdateModelAvailability(clientModelDto);
        }

        [HttpPost]
        public bool DeleteUser(string userId, bool isActive)
        {
            string modifiedBy = User.Identity.GetUserId();
            return _accountService.DeleteUser(userId, modifiedBy, isActive);
        }

        [HttpPost]
        public async Task UpdateZoneOperatingDays(AddZoneModel zone)
        {
            await _adminService.UpdateZoneOperatingDays(zone);
        }
    }
}