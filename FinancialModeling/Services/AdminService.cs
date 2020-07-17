using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using FinancialModeling.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialModeling.Services
{
    public class AdminService : IAdminService
    {
        private IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        //Add/Edit Client API
        public async Task<ParkingClient> SetupClient(AddClientModel addClientModel)
        {
            return await _adminRepository.SetupClient(addClientModel);
        }

        public async Task<List<ParkingClientModel>> GetClientList(int clientId = 0)
        {
            return await _adminRepository.GetClientList(clientId);
        }

        public async Task<ParkingClientModel> GetClientById(int clientId)
        {
            return await _adminRepository.GetClientById(clientId);
        }

        public async Task<FinanclialDashboardModel> GetClientInfo(int clientId)
        {
            return await _adminRepository.GetClientInfo(clientId);
        }

        public SetupHourlyModelDto UpdateHourlyModel(SetupHourlyModelDto hourlyModelDto)
        {
            return _adminRepository.UpdateHourlyModel(hourlyModelDto);
        }

        public SetupTimeOfDayModelDto UpdateTimeOfDayModel(SetupTimeOfDayModelDto setupTimeOfDayModel)
        {
            return _adminRepository.UpdateTimeOfDayModel(setupTimeOfDayModel);
        }
        public SetupEscalatingModelDto UpdateEscalatingModel(SetupEscalatingModelDto setupEscalatingModel)
        {
            return _adminRepository.UpdateEscalatingModel(setupEscalatingModel);
        }
        public LocationEquipmentCostModel UpdateEquipmentCost(LocationEquipmentCostModel equipmentCostModel)
        {
            return _adminRepository.UpdateEquipmentCost(equipmentCostModel);
        }
        public async Task<ParkingClient> UpdateClient(AddClientModel updateClientModel)
        {
            return await _adminRepository.UpdateClient(updateClientModel);
        }

        public async Task<bool> UpdateModelAvailability(ClientModelDto clientModelDto)
        {
            return await _adminRepository.UpdateModelAvailability(clientModelDto);
        }

        public async Task UpdateZoneOperatingDays(AddZoneModel zone)
        {
            await _adminRepository.UpdateZoneOperatingDays(zone);
        }
    }
}