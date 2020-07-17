using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FinancialModeling.Services
{
    public interface IAdminService
    {
        Task<ParkingClient> SetupClient(AddClientModel addClientModel);

        Task<List<ParkingClientModel>> GetClientList(int clientId = 0);
        Task<ParkingClientModel> GetClientById(int clientId);
        Task<FinanclialDashboardModel> GetClientInfo(int clientId);
        SetupHourlyModelDto UpdateHourlyModel(SetupHourlyModelDto hourlyModelDto);
        SetupTimeOfDayModelDto UpdateTimeOfDayModel(SetupTimeOfDayModelDto setupTimeOfDayModel);
        SetupEscalatingModelDto UpdateEscalatingModel(SetupEscalatingModelDto setupEscalatingModel);
        LocationEquipmentCostModel UpdateEquipmentCost(LocationEquipmentCostModel equipmentCostModel);
        Task<ParkingClient> UpdateClient(AddClientModel updateClientModel);
        Task<bool> UpdateModelAvailability(ClientModelDto clientModelDto);
        Task UpdateZoneOperatingDays(AddZoneModel zone);
    }
}
