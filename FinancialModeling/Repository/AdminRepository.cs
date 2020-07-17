using FinancialModeling.Models;
using FinancialModeling.Models.DBModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using static FinancialModeling.Enums.FinancialModelingEnums;

namespace FinancialModeling.Repository
{
    public class AdminRepository : IAdminRepository
    {
        FinancialModelingDbContext _context;
        static string dbConnection = ConfigurationManager.ConnectionStrings["FinancialModelingConnection"].ConnectionString;
        public AdminRepository(FinancialModelingDbContext context)
        {
            _context = context;
        }

        #region Static fields
        static List<int> modelTypeIds = new List<int>() { (int)ModelTypeEnum.Hourly, (int)ModelTypeEnum.TimeOfDay, (int)ModelTypeEnum.Escalating };
        static List<int> parkingTypeIds = new List<int>() { (int)ParkingTypeEnum.OnStreet, (int)ParkingTypeEnum.OffStreet, (int)ParkingTypeEnum.Garages };
        static DefaultModel defaultModel = new DefaultModel();
        #endregion Static fields

        #region Public Methods Exposed by service

        public async Task<ParkingClient> SetupClient(AddClientModel addClientModel)
        {
            string createdById = null;

            ParkingClient parkingClient = new ParkingClient();
            parkingClient.ClientId = 0;
            parkingClient.ClientName = addClientModel.ClientName;
            parkingClient.NumberOfUsers = 0;
            parkingClient.OnStreetZoneCount = addClientModel.OnStreetZones.Count;
            parkingClient.OffStreetZoneCount = addClientModel.OffStreetZones.Count;
            parkingClient.GaragesZoneCount = addClientModel.GaragesZones.Count;
            parkingClient.OnStreetPermitCount = addClientModel.OnStreetPermits.Count;
            parkingClient.OffStreetPermitCount = addClientModel.OffStreetPermits.Count;
            parkingClient.GaragesPermitCount = addClientModel.GaragesPermits.Count;
            parkingClient.HavePermits = addClientModel.HavePermits;
            parkingClient.IsPeakSeasonPricing = addClientModel.IsPeakSeasonPricing;
            parkingClient.CreatedDate = DateTime.UtcNow;
            parkingClient.CreatedById = createdById;
            parkingClient.IsActive = true;
            _context.ParkingClients.Add(parkingClient);
            _context.SaveChanges();

            List<ClientModel> clientModels = new List<ClientModel>();
            modelTypeIds.ForEach((modelTypeId) =>
            {
                parkingTypeIds.ForEach((parkingTypeId) =>
                {
                    ClientModel clientModel = new ClientModel();
                    clientModel.ClientModelId = 0;
                    clientModel.ClientId = parkingClient.ClientId;
                    clientModel.ParkingTypeId = parkingTypeId;
                    clientModel.ModelTypeId = modelTypeId;
                    clientModel.IsSetupDone = false;
                    clientModel.IsAvailable = true;
                    clientModel.CreatedById = createdById;
                    clientModel.CreatedDate = DateTime.UtcNow;
                    clientModel.IsActive = true;
                    clientModels.Add(clientModel);
                });
            });
            _context.ClientModels.AddRange(clientModels);
            await _context.SaveChangesAsync();

            List<Permit> onStreetPermits = new List<Permit>();
            foreach (AddPermitModel addPermitModel in addClientModel.OnStreetPermits)
            {
                Permit permit = new Permit();
                permit.PermitCode = 0;
                permit.ClientId = parkingClient.ClientId;
                permit.PermitName = addPermitModel.PermitName;
                permit.ParkingTypeId = (int)ParkingTypeEnum.OnStreet;
                permit.IsActive = true;
                onStreetPermits.Add(permit);
            }
            _context.Permits.AddRange(onStreetPermits);

            List<Permit> offStreetPermits = new List<Permit>();
            foreach (AddPermitModel addPermitModel in addClientModel.OffStreetPermits)
            {
                Permit permit = new Permit();
                permit.PermitCode = 0;
                permit.ClientId = parkingClient.ClientId;
                permit.PermitName = addPermitModel.PermitName;
                permit.ParkingTypeId = (int)ParkingTypeEnum.OffStreet;
                permit.IsActive = true;
                offStreetPermits.Add(permit);
            }
            _context.Permits.AddRange(offStreetPermits);

            List<Permit> garagesPermits = new List<Permit>();
            foreach (AddPermitModel addPermitModel in addClientModel.GaragesPermits)
            {
                Permit permit = new Permit();
                permit.PermitCode = 0;
                permit.ClientId = parkingClient.ClientId;
                permit.PermitName = addPermitModel.PermitName;
                permit.ParkingTypeId = (int)ParkingTypeEnum.Garages;
                permit.IsActive = true;
                garagesPermits.Add(permit);
            }
            _context.Permits.AddRange(garagesPermits);
            await _context.SaveChangesAsync();


            foreach (AddZoneModel onStreetZone in addClientModel.OnStreetZones)
            {
                List<ClientModel> onStreetModels = clientModels.FindAll(x => x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet);
                AddZonesToModels(onStreetModels, onStreetZone, onStreetPermits, parkingClient, (int)ParkingTypeEnum.OnStreet);
            }
            foreach (AddZoneModel offStreetZone in addClientModel.OffStreetZones)
            {
                List<ClientModel> offStreetModels = clientModels.FindAll(x => x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet);
                AddZonesToModels(offStreetModels, offStreetZone, offStreetPermits, parkingClient, (int)ParkingTypeEnum.OffStreet);
            }
            foreach (AddZoneModel garagesZone in addClientModel.GaragesZones)
            {
                List<ClientModel> garagesModels = clientModels.FindAll(x => x.ParkingTypeId == (int)ParkingTypeEnum.Garages);
                AddZonesToModels(garagesModels, garagesZone, garagesPermits, parkingClient, (int)ParkingTypeEnum.Garages);
            }
            await _context.SaveChangesAsync();

            return parkingClient;
        }

        public async Task<ParkingClient> UpdateClient(AddClientModel updateClientModel)
        {
            ParkingClient parkingClient = new ParkingClient();
            if (updateClientModel != null)
            {
                parkingClient = _context.ParkingClients.Where(x => x.ClientId == updateClientModel.ClientId).FirstOrDefault();
                if (parkingClient != null)
                {
                    parkingClient.HavePermits = updateClientModel.HavePermits;
                    parkingClient.IsPeakSeasonPricing = updateClientModel.IsPeakSeasonPricing;
                    parkingClient.IsActive = updateClientModel.IsActive;
                    parkingClient.ClientName = updateClientModel.ClientName;
                    parkingClient.OnStreetZoneCount = updateClientModel.OnStreetZones.Where(x => x.ActionType != ActionType.Deleted).Count();
                    parkingClient.OffStreetZoneCount = updateClientModel.OffStreetZones.Where(x => x.ActionType != ActionType.Deleted).Count();
                    parkingClient.GaragesZoneCount = updateClientModel.GaragesZones.Where(x => x.ActionType != ActionType.Deleted).Count();
                    parkingClient.OnStreetPermitCount = updateClientModel.OnStreetPermits.Where(x => x.ActionType != ActionType.Deleted).Count();
                    parkingClient.OffStreetPermitCount = updateClientModel.OffStreetPermits.Where(x => x.ActionType != ActionType.Deleted).Count();
                    parkingClient.GaragesPermitCount = updateClientModel.GaragesPermits.Where(x => x.ActionType != ActionType.Deleted).Count();
                    _context.Entry(parkingClient).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                List<ClientModel> clientModels = await _context.ClientModels.Where(x => x.ClientId == updateClientModel.ClientId).ToListAsync();
                List<ClientModel> onStreetModels = clientModels.FindAll(x => x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet && x.IsActive == true);
                List<Zone> onStreetZones = await _context.Zones.Where(x => x.ClientId == updateClientModel.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet && x.IsActive == true).ToListAsync();

                List<ClientModel> offStreetModels = clientModels.FindAll(x => x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet && x.IsActive == true);
                List<Zone> offStreetZones = _context.Zones.Where(x => x.ClientId == updateClientModel.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet && x.IsActive == true).ToList();

                List<ClientModel> garagesModels = clientModels.FindAll(x => x.ParkingTypeId == (int)ParkingTypeEnum.Garages && x.IsActive == true);
                List<Zone> garagesZones = await _context.Zones.Where(x => x.ClientId == updateClientModel.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.Garages && x.IsActive == true).ToListAsync();

                List<Permit> onStreetPermitsAdd = new List<Permit>();
                foreach (AddPermitModel permitModel in updateClientModel.OnStreetPermits)
                {
                    if (permitModel.ActionType == ActionType.Created)
                    {
                        Permit permit = new Permit();
                        permit.PermitCode = 0;
                        permit.ClientId = parkingClient.ClientId;
                        permit.PermitName = permitModel.PermitName;
                        permit.ParkingTypeId = (int)ParkingTypeEnum.OnStreet;
                        permit.IsActive = true;
                        onStreetPermitsAdd.Add(permit);
                    }
                    else
                    {
                        if (permitModel.ActionType == ActionType.Deleted)
                        {
                            Permit permit = _context.Permits.Where(x => x.PermitCode == permitModel.PermitCode).FirstOrDefault();
                            if (permit != null)
                            {
                                permit.IsActive = false;
                                _context.Entry(permit).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            Permit permit = _context.Permits.Where(x => x.PermitCode == permitModel.PermitCode).FirstOrDefault();
                            if (permit != null)
                            {
                                permit.PermitName = permitModel.PermitName;
                                _context.Entry(permit).State = EntityState.Modified;
                                _context.SaveChanges();
                            }
                        }
                    }
                }
                _context.Permits.AddRange(onStreetPermitsAdd);
                await _context.SaveChangesAsync();
                foreach (var zone in onStreetZones)
                {
                    ClientModel hourlyModel = onStreetModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Hourly);
                    AddPermitsToZone(onStreetPermitsAdd, hourlyModel, zone.ZoneCode, parkingClient.HavePermits);

                    ClientModel timeOfDayZoneModel = onStreetModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.TimeOfDay);
                    AddPermitsToZone(onStreetPermitsAdd, timeOfDayZoneModel, zone.ZoneCode, parkingClient.HavePermits);

                    ClientModel escalatingModel = onStreetModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Escalating);
                    AddPermitsToZone(onStreetPermitsAdd, escalatingModel, zone.ZoneCode, parkingClient.HavePermits);
                }

                List<Permit> offStreetPermitsAdd = new List<Permit>();
                foreach (AddPermitModel permitModel in updateClientModel.OffStreetPermits)
                {
                    if (permitModel.ActionType == ActionType.Created)
                    {
                        Permit permit = new Permit();
                        permit.PermitCode = 0;
                        permit.ClientId = parkingClient.ClientId;
                        permit.PermitName = permitModel.PermitName;
                        permit.ParkingTypeId = (int)ParkingTypeEnum.OffStreet;
                        permit.IsActive = true;
                        offStreetPermitsAdd.Add(permit);
                    }
                    else
                    {
                        if (permitModel.ActionType == ActionType.Deleted)
                        {
                            Permit permit = _context.Permits.Where(x => x.PermitCode == permitModel.PermitCode).FirstOrDefault();
                            if (permit != null)
                            {
                                permit.IsActive = false;
                                _context.Entry(permit).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            Permit permit = _context.Permits.Where(x => x.PermitCode == permitModel.PermitCode).FirstOrDefault();
                            if (permit != null)
                            {
                                permit.PermitName = permitModel.PermitName;
                                _context.Entry(permit).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                _context.Permits.AddRange(offStreetPermitsAdd);
                await _context.SaveChangesAsync();
                foreach (var zone in offStreetZones)
                {
                    ClientModel hourlyModel = offStreetModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Hourly);
                    AddPermitsToZone(offStreetPermitsAdd, hourlyModel, zone.ZoneCode, parkingClient.HavePermits);

                    ClientModel timeOfDayZoneModel = offStreetModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.TimeOfDay);
                    AddPermitsToZone(offStreetPermitsAdd, timeOfDayZoneModel, zone.ZoneCode, parkingClient.HavePermits);

                    ClientModel escalatingModel = offStreetModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Escalating);
                    AddPermitsToZone(offStreetPermitsAdd, escalatingModel, zone.ZoneCode, parkingClient.HavePermits);
                }

                List<Permit> garagesPermitsAdd = new List<Permit>();
                foreach (AddPermitModel permitModel in updateClientModel.GaragesPermits)
                {
                    if (permitModel.ActionType == ActionType.Created)
                    {
                        Permit permit = new Permit();
                        permit.PermitCode = 0;
                        permit.ClientId = parkingClient.ClientId;
                        permit.PermitName = permitModel.PermitName;
                        permit.ParkingTypeId = (int)ParkingTypeEnum.Garages;
                        permit.IsActive = true;
                        garagesPermitsAdd.Add(permit);
                    }
                    else
                    {
                        if (permitModel.ActionType == ActionType.Deleted)
                        {
                            Permit permit = _context.Permits.Where(x => x.PermitCode == permitModel.PermitCode).FirstOrDefault();
                            if (permit != null)
                            {
                                permit.IsActive = false;
                                _context.Entry(permit).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                        else
                        {
                            Permit permit = _context.Permits.Where(x => x.PermitCode == permitModel.PermitCode).FirstOrDefault();
                            if (permit != null)
                            {
                                permit.PermitName = permitModel.PermitName;
                                _context.Entry(permit).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
                _context.Permits.AddRange(garagesPermitsAdd);
                await _context.SaveChangesAsync();
                foreach (var zone in garagesZones)
                {
                    ClientModel hourlyModel = garagesModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Hourly);
                    AddPermitsToZone(garagesPermitsAdd, hourlyModel, zone.ZoneCode, parkingClient.HavePermits);

                    ClientModel timeOfDayZoneModel = garagesModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.TimeOfDay);
                    AddPermitsToZone(garagesPermitsAdd, timeOfDayZoneModel, zone.ZoneCode, parkingClient.HavePermits);

                    ClientModel escalatingModel = garagesModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Escalating);
                    AddPermitsToZone(garagesPermitsAdd, escalatingModel, zone.ZoneCode, parkingClient.HavePermits);
                }

                foreach (AddZoneModel onStreetZone in updateClientModel.OnStreetZones)
                {
                    if (onStreetZone.ActionType == ActionType.Created)
                    {
                        List<Permit> onStreetPermits = _context.Permits.Where(x => x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet && x.ClientId == parkingClient.ClientId && x.IsActive == true).ToList();
                        AddZonesToModels(onStreetModels, onStreetZone, onStreetPermits, parkingClient, (int)ParkingTypeEnum.OnStreet);
                    }
                    else if (onStreetZone.ActionType == ActionType.Deleted)
                    {
                        DeleteZone(onStreetZone.ZoneCode);
                    }
                    else
                    {
                        Zone zone = _context.Zones.Where(x => x.ZoneCode == onStreetZone.ZoneCode).FirstOrDefault();
                        if (zone != null)
                        {
                            zone.ZoneName = onStreetZone.ZoneName;
                            _context.Entry(zone).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                foreach (AddZoneModel offStreetZone in updateClientModel.OffStreetZones)
                {
                    if (offStreetZone.ActionType == ActionType.Created)
                    {
                        List<Permit> offStreetPermits = _context.Permits.Where(x => x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet && x.ClientId == parkingClient.ClientId && x.IsActive == true).ToList();
                        AddZonesToModels(offStreetModels, offStreetZone, offStreetPermits, parkingClient, (int)ParkingTypeEnum.OffStreet);
                    }
                    else if (offStreetZone.ActionType == ActionType.Deleted)
                    {
                        DeleteZone(offStreetZone.ZoneCode);
                    }
                    else
                    {
                        Zone zone = _context.Zones.Where(x => x.ZoneCode == offStreetZone.ZoneCode).FirstOrDefault();
                        if (zone != null)
                        {
                            zone.ZoneName = offStreetZone.ZoneName;
                            _context.Entry(zone).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }

                foreach (AddZoneModel garagesZone in updateClientModel.GaragesZones)
                {
                    if (garagesZone.ActionType == ActionType.Created)
                    {
                        List<Permit> garagesPermits = _context.Permits.Where(x => x.ParkingTypeId == (int)ParkingTypeEnum.Garages && x.ClientId == parkingClient.ClientId && x.IsActive == true).ToList();
                        AddZonesToModels(garagesModels, garagesZone, garagesPermits, parkingClient, (int)ParkingTypeEnum.Garages);
                    }
                    else if (garagesZone.ActionType == ActionType.Deleted)
                    {
                        DeleteZone(garagesZone.ZoneCode);
                    }
                    else
                    {
                        Zone zone = _context.Zones.Where(x => x.ZoneCode == garagesZone.ZoneCode).FirstOrDefault();
                        if (zone != null)
                        {
                            zone.ZoneName = garagesZone.ZoneName;
                            _context.Entry(zone).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                }
            }
            await _context.SaveChangesAsync();
            return parkingClient;
        }


        public async Task<bool> UpdateModelAvailability(ClientModelDto clientModelDto)
        {
            ClientModel clientModel = _context.ClientModels.Where(x => x.ClientModelId == clientModelDto.ClientModelId).FirstOrDefault();
            clientModel.IsAvailable = clientModelDto.IsAvailable;
            _context.Entry(clientModel).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task UpdateZoneOperatingDays(AddZoneModel zone)
        {
            Zone zoneRow = _context.Zones.Where(x => x.ZoneCode == zone.ZoneCode).FirstOrDefault();
            if (zoneRow != null)
            {
                zoneRow.OperatingDays = zone.OperatingDays;
                _context.Entry(zoneRow).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<ParkingClientModel>> GetClientList(int clientId = 0)
        {
            if (clientId != 0)
            {
                return await (from c in _context.ParkingClients
                              where c.IsActive == true && c.ClientId == clientId
                              select new ParkingClientModel
                              {
                                  ClientId = c.ClientId,
                                  ClientName = c.ClientName,
                                  CreatedById = c.CreatedById,
                                  GaragesPermitCount = c.GaragesPermitCount,
                                  GaragesZoneCount = c.GaragesZoneCount,
                                  NumberOfUsers = c.NumberOfUsers,
                                  OffStreetPermitCount = c.OffStreetPermitCount,
                                  OffStreetZoneCount = c.OffStreetZoneCount,
                                  OnStreetPermitCount = c.OnStreetPermitCount,
                                  OnStreetZoneCount = c.OnStreetZoneCount,
                                  IsActive = c.IsActive,
                              }).AsNoTracking().ToListAsync();
            }
            else
            {
                return await (from c in _context.ParkingClients
                              where c.IsActive == true
                              select new ParkingClientModel
                              {
                                  ClientId = c.ClientId,
                                  ClientName = c.ClientName,
                                  CreatedById = c.CreatedById,
                                  GaragesPermitCount = c.GaragesPermitCount,
                                  GaragesZoneCount = c.GaragesZoneCount,
                                  NumberOfUsers = c.NumberOfUsers,
                                  OffStreetPermitCount = c.OffStreetPermitCount,
                                  OffStreetZoneCount = c.OffStreetZoneCount,
                                  OnStreetPermitCount = c.OnStreetPermitCount,
                                  OnStreetZoneCount = c.OnStreetZoneCount,
                                  IsActive = c.IsActive,
                              }).AsNoTracking().ToListAsync();
            }

        }

        public async Task<ParkingClientModel> GetClientById(int clientId)
        {
            return await (from c in _context.ParkingClients.AsNoTracking().Where(x => x.ClientId == clientId && x.IsActive == true)
                          select new ParkingClientModel
                          {
                              ClientId = c.ClientId,
                              ClientName = c.ClientName,
                              CreatedById = c.CreatedById,
                              GaragesPermitCount = c.GaragesPermitCount,
                              GaragesZoneCount = c.GaragesZoneCount,
                              NumberOfUsers = c.NumberOfUsers,
                              OffStreetPermitCount = c.OffStreetPermitCount,
                              OffStreetZoneCount = c.OffStreetZoneCount,
                              OnStreetPermitCount = c.OnStreetPermitCount,
                              OnStreetZoneCount = c.OnStreetZoneCount,
                              IsActive = c.IsActive,
                          }).FirstOrDefaultAsync();
        }

        public async Task<FinanclialDashboardModel> GetClientInfo(int clientId)
        {
            FinanclialDashboardModel financlialDashboard = new FinanclialDashboardModel();

            IQueryable<ClientModel> queryableClientModels = _context.ClientModels.AsNoTracking().Where(x => x.ClientId == clientId);

            ParkingClient parkingClient = _context.ParkingClients.AsNoTracking().Where(x => x.ClientId == clientId).FirstOrDefault();
            //Make Async call by creating separate service call
            List<SetupHourlyModelDto> hourlyModelList = await GetHourlyModels(queryableClientModels, parkingClient);
            List<SetupTimeOfDayModelDto> timeOfDayModelList = await GetTimeOfDayModels(queryableClientModels, parkingClient);
            List<SetupEscalatingModelDto> escalatingModelList = await GetEscalatingModels(queryableClientModels, parkingClient);

            //Make Async call by creating separate service call
            financlialDashboard.EditClientModel = GetEditClientModel(clientId);

            financlialDashboard.HourlyOnStreet = hourlyModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet);
            financlialDashboard.HourlyOffStreet = hourlyModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet);
            financlialDashboard.HourlyGarages = hourlyModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.Garages);

            financlialDashboard.TimeOfDayOnStreet = timeOfDayModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet);
            financlialDashboard.TimeOfDayOffStreet = timeOfDayModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet);
            financlialDashboard.TimeOfDayGarages = timeOfDayModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.Garages);

            financlialDashboard.EscalatingOnStreet = escalatingModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet);
            financlialDashboard.EscalatingOffStreet = escalatingModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet);
            financlialDashboard.EscalatingGarages = escalatingModelList.Find(x => x.ParkingTypeId == (int)ParkingTypeEnum.Garages);

            financlialDashboard.OnStreetEquipmentCost = await GetLocationEquipmentCost((int)ParkingTypeEnum.OnStreet, clientId);
            financlialDashboard.OffStreetEquipmentCost = await GetLocationEquipmentCost((int)ParkingTypeEnum.OffStreet, clientId);
            financlialDashboard.GaragesEquipmentCost = await GetLocationEquipmentCost((int)ParkingTypeEnum.Garages, clientId);

            return financlialDashboard;

        }

        private void UpdateClientModel(int clientModelId, bool isAvailable)
        {
            ClientModel clientModel = _context.ClientModels.Where(x => x.ClientModelId == clientModelId).FirstOrDefault();
            clientModel.IsSetupDone = true;
            clientModel.IsAvailable = isAvailable;
        }

        public SetupHourlyModelDto UpdateHourlyModel(SetupHourlyModelDto hourlyModelDto)
        {
            if (hourlyModelDto.HourlyZones.Any())
            {
                UpdateClientModel(hourlyModelDto.ClientModelId, hourlyModelDto.IsAvailable);

                foreach (var zone in hourlyModelDto.HourlyZones.Where(z => z.IsModified == true))
                {
                    HourlyZone hourlyZone = _context.HourlyZones.Where(x => x.ZoneId == zone.ZoneId).FirstOrDefault();
                    if (hourlyZone != null)
                    {
                        _context.Entry(hourlyZone).CurrentValues.SetValues(zone);
                        _context.Entry(hourlyZone).State = EntityState.Modified;
                    }

                    HourlyOperatingHourDto hourlyOperatingHourDto = zone.HourlyOperatingHour;
                    if (hourlyOperatingHourDto != null)
                    {
                        HourlyOperatingHour hourlyOperatingHour = _context.HourlyOperatingHours.Where(x => x.Id == hourlyOperatingHourDto.Id).FirstOrDefault();
                        if (hourlyOperatingHour != null)
                        {
                            _context.Entry(hourlyOperatingHour).CurrentValues.SetValues(hourlyOperatingHourDto);
                            _context.Entry(hourlyOperatingHour).State = EntityState.Modified;
                        }
                    }

                    OperatingDaysDto operatingDaysDto = zone.OperatingDays;
                    if (operatingDaysDto != null)
                    {
                        OperatingDays operatingDays = _context.OperatingDays.Where(x => x.Id == operatingDaysDto.Id).FirstOrDefault();
                        if (operatingDays != null)
                        {
                            _context.Entry(operatingDays).CurrentValues.SetValues(operatingDaysDto);
                            _context.Entry(operatingDays).State = EntityState.Modified;
                        }
                    }

                    //Updates Permits
                    UpdatePermits(zone.ClientPermitTypes);
                }
                _context.SaveChanges();
            }
            return hourlyModelDto;
        }

        public SetupTimeOfDayModelDto UpdateTimeOfDayModel(SetupTimeOfDayModelDto setupTimeOfDayModel)
        {
            if (setupTimeOfDayModel.TimeOfDayZones.Any())
            {
                UpdateClientModel(setupTimeOfDayModel.ClientModelId, setupTimeOfDayModel.IsAvailable);

                foreach (var zone in setupTimeOfDayModel.TimeOfDayZones)
                {
                    TimeOfDayZone timeOfDayZone = _context.TimeOfDayZones.Where(x => x.ZoneId == zone.ZoneId).FirstOrDefault();
                    if (timeOfDayZone != null)
                    {
                        _context.Entry(timeOfDayZone).CurrentValues.SetValues(zone);
                        _context.Entry(timeOfDayZone).State = EntityState.Modified;
                    }

                    OperatingDaysDto operatingDaysDto = zone.OperatingDays;
                    if (operatingDaysDto != null)
                    {
                        OperatingDays operatingDays = _context.OperatingDays.Where(x => x.Id == operatingDaysDto.Id).FirstOrDefault();
                        if (operatingDays != null)
                        {
                            _context.Entry(operatingDays).CurrentValues.SetValues(operatingDaysDto);
                            _context.Entry(operatingDays).State = EntityState.Modified;
                        }
                    }

                    List<TimeOfDayOperatingHourDto> timeOfDayOperatingHours = zone.HoursOfOperations;
                    if (timeOfDayOperatingHours.Any())
                    {
                        foreach (var opHour in timeOfDayOperatingHours)
                        {
                            switch (opHour.ActionType)
                            {
                                case ActionType.Created:
                                    TimeOfDayOperatingHour timeOfDayOperatingHour = new TimeOfDayOperatingHour();
                                    timeOfDayOperatingHour.Id = 0;
                                    timeOfDayOperatingHour.ClientId = timeOfDayZone.ClientId;
                                    timeOfDayOperatingHour.ZoneId = timeOfDayZone.ZoneId;
                                    timeOfDayOperatingHour.ClientModelId = timeOfDayZone.ClientModelId;

                                    timeOfDayOperatingHour.PeakSeasonHourlyRate = opHour.PeakSeasonHourlyRate;
                                    timeOfDayOperatingHour.NonPeakSeasonHourlyRate = opHour.NonPeakSeasonHourlyRate;

                                    timeOfDayOperatingHour.OperatingHoursStart = opHour.OperatingHoursStart;
                                    timeOfDayOperatingHour.OperatingHoursEnd = opHour.OperatingHoursEnd;
                                    timeOfDayOperatingHour.TotalHours = opHour.TotalHours;

                                    timeOfDayOperatingHour.NonPeakOccupancyPercentage = defaultModel.NonPeakOccupancyPercentage;
                                    timeOfDayOperatingHour.PeakOccupancyPercentage = defaultModel.PeakOccupancyPercentage;
                                    timeOfDayOperatingHour.IsPeak = false;
                                    timeOfDayOperatingHour.IsActive = true;
                                    _context.TimeOfDayOperatingHours.Add(timeOfDayOperatingHour);
                                    _context.SaveChanges();
                                    opHour.Id = timeOfDayOperatingHour.Id;
                                    opHour.ClientId = timeOfDayOperatingHour.ClientId;
                                    opHour.ZoneId = timeOfDayOperatingHour.ZoneId;
                                    opHour.ClientModelId = timeOfDayOperatingHour.ClientModelId;
                                    break;
                                case ActionType.Deleted:
                                    TimeOfDayOperatingHour deletedOperatingHour = _context.TimeOfDayOperatingHours.Where(x => x.Id == opHour.Id).FirstOrDefault();
                                    if (deletedOperatingHour != null)
                                        _context.Entry(deletedOperatingHour).State = EntityState.Deleted;
                                    break;
                                case ActionType.Modified:
                                default:
                                    TimeOfDayOperatingHour todOperatingHour = _context.TimeOfDayOperatingHours.Where(x => x.Id == opHour.Id).FirstOrDefault();
                                    if (todOperatingHour != null)
                                    {
                                        _context.Entry(todOperatingHour).CurrentValues.SetValues(opHour);
                                        _context.Entry(todOperatingHour).State = EntityState.Modified;
                                    }
                                    break;
                            }
                        }
                    }

                    //Updates Permits
                    UpdatePermits(zone.ClientPermitTypes);
                }
                _context.SaveChanges();
            }
            return setupTimeOfDayModel;
        }

        public SetupEscalatingModelDto UpdateEscalatingModel(SetupEscalatingModelDto setupEscalatingModel)
        {
            if (setupEscalatingModel.EscalatingZones.Any())
            {
                UpdateClientModel(setupEscalatingModel.ClientModelId, setupEscalatingModel.IsAvailable);

                foreach (var zone in setupEscalatingModel.EscalatingZones)
                {
                    EscalatingZone escalatingZone = _context.EscalatingZones.FirstOrDefault(x => x.ZoneId == zone.ZoneId);
                    if (escalatingZone != null)
                    {
                        _context.Entry(escalatingZone).CurrentValues.SetValues(zone);
                        _context.Entry(escalatingZone).State = EntityState.Modified;
                    }

                    OperatingDaysDto operatingDaysDto = zone.OperatingDays;
                    if (operatingDaysDto != null)
                    {
                        OperatingDays operatingDays = _context.OperatingDays.FirstOrDefault(x => x.Id == operatingDaysDto.Id);
                        if (operatingDays != null)
                        {
                            _context.Entry(operatingDays).CurrentValues.SetValues(operatingDaysDto);
                            _context.Entry(operatingDays).State = EntityState.Modified;
                        }
                    }

                    EscalatingOperatingHourDto escalatingOperatingHourDaily = zone.EscalatingOperatingHourDaily;
                    if (escalatingOperatingHourDaily != null)
                    {
                        EscalatingOperatingHour operatingHourDaily = _context.EscalatingOperatingHours.FirstOrDefault(x => x.Id == escalatingOperatingHourDaily.Id);
                        if (operatingHourDaily != null)
                        {
                            _context.Entry(operatingHourDaily).CurrentValues.SetValues(escalatingOperatingHourDaily);
                            _context.Entry(operatingHourDaily).State = EntityState.Modified;
                        }
                    }

                    EscalatingOperatingHourDto escalatingOperatingHourEvening = zone.EscalatingOperatingHourEvening;
                    if (escalatingOperatingHourEvening != null)
                    {
                        EscalatingOperatingHour operatingHourEvening = _context.EscalatingOperatingHours.FirstOrDefault(x => x.Id == escalatingOperatingHourEvening.Id);
                        if (operatingHourEvening != null)
                        {
                            _context.Entry(operatingHourEvening).CurrentValues.SetValues(escalatingOperatingHourEvening);
                            _context.Entry(operatingHourEvening).State = EntityState.Modified;
                        }
                    }

                    //Updates Permits
                    UpdatePermits(zone.ClientPermitTypes);
                }
                _context.SaveChanges();
            }

            return setupEscalatingModel;
        }

        public LocationEquipmentCostModel UpdateEquipmentCost(LocationEquipmentCostModel equipmentCostModel)
        {
            if (equipmentCostModel != null && equipmentCostModel.Zones.Any())
            {
                foreach (var zone in equipmentCostModel.Zones)
                {
                    if (zone.Equipments.Any())
                    {
                        foreach (var equipment in zone.Equipments)
                        {
                            switch (equipment.ActionType)
                            {
                                case ActionType.Created:
                                    EquipmentCost equipmentCost = new EquipmentCost()
                                    {
                                        EquipmentId = 0,
                                        UnitsOwned = equipment.UnitsOwned,
                                        UnitsPurchased = equipment.UnitsPurchased,

                                        CostOfBaseUnit = equipment.CostOfBaseUnit,
                                        WarrantyStartingYear = equipment.WarrantyStartingYear,
                                        MonthlyMeterSoftwareFees = equipment.MonthlyMeterSoftwareFees,

                                        QuantityOfUnits = equipment.QuantityOfUnits,
                                        MultiSpaceMeterCost = equipment.MultiSpaceMeterCost,
                                        EquipWithBNA = equipment.EquipWithBNA,
                                        EquipWithCreditCard = equipment.EquipWithCreditCard,
                                        AnnualSoftwareFee = equipment.AnnualSoftwareFee,
                                        IsWarrantyIncluded = equipment.IsWarrantyIncluded,

                                        MonthlyCreditCardProcessingFees = equipment.MonthlyCreditCardProcessingFees,
                                        EstimatedCreditCardTransaction = equipment.EstimatedCreditCardTransaction,
                                        EquipmentTypeId = equipment.EquipmentTypeId,
                                        ClientId = zone.ClientId,
                                        ZoneCode = zone.ZoneCode,
                                        ParkingTypeId = zone.ParkingTypeId,
                                        IsActive = true
                                    };
                                    _context.EquipmentCosts.Add(equipmentCost);
                                    _context.SaveChanges();
                                    equipment.EquipmentId = equipmentCost.EquipmentId;
                                    equipment.ZoneCode = equipmentCost.ZoneCode;
                                    equipment.ClientId = equipmentCost.ClientId;
                                    equipment.ActionType = ActionType.Modified;
                                    break;
                                case ActionType.Deleted:
                                    EquipmentCost deletedEquipmentCost = _context.EquipmentCosts.Where(x => x.EquipmentId == equipment.EquipmentId).FirstOrDefault();
                                    deletedEquipmentCost.IsActive = false;
                                    _context.Entry(deletedEquipmentCost).State = EntityState.Modified;
                                    _context.SaveChanges();
                                    break;
                                case ActionType.Modified:
                                default:
                                    EquipmentCost updatedEquipmentCost = _context.EquipmentCosts.Where(x => x.EquipmentId == equipment.EquipmentId).FirstOrDefault();
                                    if (updatedEquipmentCost != null)
                                    {
                                        updatedEquipmentCost.UnitsOwned = equipment.UnitsOwned;
                                        updatedEquipmentCost.UnitsPurchased = equipment.UnitsPurchased;

                                        updatedEquipmentCost.CostOfBaseUnit = equipment.CostOfBaseUnit;
                                        updatedEquipmentCost.WarrantyStartingYear = equipment.WarrantyStartingYear;
                                        updatedEquipmentCost.MonthlyMeterSoftwareFees = equipment.MonthlyMeterSoftwareFees;

                                        updatedEquipmentCost.QuantityOfUnits = equipment.QuantityOfUnits;
                                        updatedEquipmentCost.MultiSpaceMeterCost = equipment.MultiSpaceMeterCost;
                                        updatedEquipmentCost.EquipWithBNA = equipment.EquipWithBNA;
                                        updatedEquipmentCost.EquipWithCreditCard = equipment.EquipWithCreditCard;
                                        updatedEquipmentCost.AnnualSoftwareFee = equipment.AnnualSoftwareFee;
                                        updatedEquipmentCost.IsWarrantyIncluded = equipment.IsWarrantyIncluded;

                                        updatedEquipmentCost.MonthlyCreditCardProcessingFees = equipment.MonthlyCreditCardProcessingFees;
                                        updatedEquipmentCost.EstimatedCreditCardTransaction = equipment.EstimatedCreditCardTransaction;
                                        updatedEquipmentCost.EquipmentTypeId = equipment.EquipmentTypeId;
                                        _context.Entry(updatedEquipmentCost).State = EntityState.Modified;
                                        _context.SaveChanges();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            return equipmentCostModel;
        }
        #endregion Public Methods Exposed by service

        #region Private Helper Methods

        private void UpdatePermits(List<ClientPermitTypeDto> permitList)
        {
            if (permitList != null && permitList.Any())
            {
                foreach (ClientPermitTypeDto permit in permitList)
                {
                    PermitDetail permitDetail = _context.PermitDetails.Where(x => x.PermitId == permit.PermitId).FirstOrDefault();
                    permitDetail.AnnualCost = permit.AnnualCost;
                    permitDetail.QuantitySold = permit.QuantitySold;
                    _context.Entry(permitDetail).State = EntityState.Modified;
                }
            }
        }

        private List<AddZoneModel> GetZonesByParkingType(AddClientModel addClientModel, int parkingTypeId)
        {
            switch ((ParkingTypeEnum)parkingTypeId)
            {
                case ParkingTypeEnum.OnStreet:
                    return addClientModel.OnStreetZones;
                case ParkingTypeEnum.OffStreet:
                    return addClientModel.OffStreetZones;
                case ParkingTypeEnum.Garages:
                    return addClientModel.GaragesZones;
                default:
                    return new List<AddZoneModel>();
            }
        }

        private void AddZonesToModels(List<ClientModel> clientModels, AddZoneModel addZoneModel, List<Permit> permits, ParkingClient parkingClient, int parkingTypeId)
        {
            Zone zone = new Zone();
            zone.ZoneCode = 0;
            zone.OperatingDays = 0;
            zone.ZoneName = addZoneModel.ZoneName;
            zone.ClientId = parkingClient.ClientId;
            zone.ParkingTypeId = parkingTypeId;
            zone.IsActive = true;
            _context.Zones.Add(zone);
            _context.SaveChanges();
            //List<ClientModel> models = clientModels.FindAll(x => x.ParkingTypeId == parkingTypeId);

            ClientModel hourlyModel = clientModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Hourly);
            AddHourlyZone(parkingClient, hourlyModel, zone, permits);

            ClientModel timeOfDayZoneModel = clientModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.TimeOfDay);
            AddTimeOfDayZone(parkingClient, timeOfDayZoneModel, zone, permits);

            ClientModel escalatingModel = clientModels.Find(x => x.ModelTypeId == (int)ModelTypeEnum.Escalating);
            AddEscalatingZone(parkingClient, escalatingModel, zone, permits);

            AddDefaultEquipmentToZone(parkingClient.ClientId, parkingTypeId, zone.ZoneCode);
        }

        private void AddPermitsToZone(List<Permit> permits, ClientModel clientModel, int ZoneId, bool hasPermits)
        {
            foreach (Permit permit in permits)
            {
                PermitDetail permitDetail = new PermitDetail();
                permitDetail.PermitId = 0;
                permitDetail.PermitCode = permit.PermitCode;
                permitDetail.AnnualCost = hasPermits ? defaultModel.AnnualCost : 0;
                permitDetail.QuantitySold = hasPermits ? defaultModel.QuantitySold : 0;
                permitDetail.PermitId = permit.PermitCode;
                permitDetail.ClientId = clientModel.ClientId;
                permitDetail.ClientModelId = clientModel.ClientModelId;
                permitDetail.ZoneId = ZoneId;
                permitDetail.IsActive = true;
                _context.PermitDetails.Add(permitDetail);
                _context.SaveChanges();
            }
        }

        private void AddDefaultEquipmentToZone(int clientId, int parkingTypeId, int zoneCode)
        {
            EquipmentCost equipmentCost = new EquipmentCost();
            equipmentCost.EquipmentId = defaultModel.EquipmentId;
            equipmentCost.UnitsOwned = defaultModel.UnitsOwned;
            equipmentCost.UnitsPurchased = defaultModel.UnitsPurchased;
            equipmentCost.CostOfBaseUnit = defaultModel.CostOfBaseUnit;
            equipmentCost.WarrantyStartingYear = defaultModel.WarrantyStartingYear;
            equipmentCost.MonthlyMeterSoftwareFees = defaultModel.MonthlyMeterSoftwareFees;
            equipmentCost.MonthlyCreditCardProcessingFees = defaultModel.MonthlyCreditCardProcessingFees;
            equipmentCost.EstimatedCreditCardTransaction = defaultModel.EstimatedCreditCardTransaction;
            equipmentCost.EquipmentTypeId = defaultModel.EquipmentTypeId;

            equipmentCost.ClientId = clientId;
            equipmentCost.ParkingTypeId = parkingTypeId;
            equipmentCost.ZoneCode = zoneCode;
            equipmentCost.IsActive = true;
            _context.EquipmentCosts.Add(equipmentCost);
            _context.SaveChanges();
        }

        private void AddHourlyZone(ParkingClient parkingClient, ClientModel clientModel, Zone zone, List<Permit> permits)
        {
            HourlyZone hourlyZone = new HourlyZone();
            hourlyZone.ZoneId = 0;
            hourlyZone.ZoneCode = zone.ZoneCode;
            hourlyZone.ClientModelId = clientModel.ClientModelId;
            hourlyZone.ClientId = zone.ClientId;
            hourlyZone.NonPeakSeasonHourlyRate = defaultModel.NonPeakSeasonHourlyRate;
            hourlyZone.PeakSeasonHourlyRate = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakSeasonHourlyRate;
            hourlyZone.NumberOfSpacesPerZone = defaultModel.NumberOfSpacesPerZone;
            hourlyZone.PercentOfSpaceOccupied = defaultModel.PercentOfSpaceOccupied;
            hourlyZone.NumberOfSpacesRemaining = defaultModel.NumberOfSpacesRemaining;
            hourlyZone.CompliancePercentage = defaultModel.CompliancePercentage;
            hourlyZone.NonPeakOccupancyPercentage = defaultModel.NonPeakOccupancyPercentage;
            hourlyZone.PeakOccupancyPercentage = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakOccupancyPercentage;
            hourlyZone.IsActive = true;
            _context.HourlyZones.Add(hourlyZone);
            _context.SaveChanges();

            HourlyOperatingHour hourlyOperatingHour = new HourlyOperatingHour();
            hourlyOperatingHour.Id = 0;
            hourlyOperatingHour.ClientId = hourlyZone.ClientId;
            hourlyOperatingHour.ZoneId = hourlyZone.ZoneId;
            hourlyOperatingHour.StartTime = defaultModel.HourlyStartTime;
            hourlyOperatingHour.EndTime = defaultModel.HourlyEndTime;
            hourlyOperatingHour.TotalHours = defaultModel.HourlyTotalHours;
            hourlyOperatingHour.IsActive = true;
            _context.HourlyOperatingHours.Add(hourlyOperatingHour);

            OperatingDays operatingDays = new OperatingDays();
            operatingDays.Id = 0;
            operatingDays.ClientId = hourlyZone.ClientId;
            operatingDays.ClientModelId = clientModel.ClientModelId;
            operatingDays.ZoneId = hourlyZone.ZoneId;
            operatingDays.DaysPerYear = defaultModel.DaysPerYear;
            operatingDays.PeakDays = defaultModel.PeakDays;
            operatingDays.OffDays = defaultModel.OffDays;
            operatingDays.NonPeakDays = defaultModel.NonPeakDays;
            operatingDays.IsActive = true;
            _context.OperatingDays.Add(operatingDays);
            _context.SaveChanges();

            AddPermitsToZone(permits, clientModel, zone.ZoneCode, parkingClient.HavePermits);

            //foreach (AddPermitModel permit in addClientModel.OnStreetPermits)
            //{
            //    ClientPermitType clientPermitType = new ClientPermitType();
            //    clientPermitType.PermitId = 0;
            //    clientPermitType.PermitCode = permit.PermitCode;
            //    clientPermitType.PermitName = permit.PermitName;
            //    clientPermitType.AnnualCost = defaultModel.AnnualCost;
            //    clientPermitType.QuantitySold = defaultModel.QuantitySold;
            //    clientPermitType.ClientId = clientModel.ClientId;
            //    clientPermitType.ClientModelId = clientModel.ClientModelId;
            //    clientPermitType.ZoneId = hourlyZone.ZoneId;
            //    _context.ClientPermitTypes.Add(clientPermitType);
            //}
            _context.SaveChanges();
        }

        private void AddTimeOfDayZone(ParkingClient parkingClient, ClientModel clientModel, Zone zone, List<Permit> permits)
        {
            TimeOfDayZone timeOfDayZone = new TimeOfDayZone();
            timeOfDayZone.ZoneId = 0;
            timeOfDayZone.ZoneCode = zone.ZoneCode;
            timeOfDayZone.ClientModelId = clientModel.ClientModelId;
            timeOfDayZone.ClientId = clientModel.ClientId;

            timeOfDayZone.NumberOfSpacesPerZone = defaultModel.NumberOfSpacesPerZone;
            timeOfDayZone.PercentOfSpaceOccupied = defaultModel.PercentOfSpaceOccupied;
            timeOfDayZone.NumberOfSpacesRemaining = defaultModel.NumberOfSpacesRemaining;

            timeOfDayZone.CompliancePercentage = defaultModel.CompliancePercentage;
            timeOfDayZone.IsActive = true;
            _context.TimeOfDayZones.Add(timeOfDayZone);
            _context.SaveChanges();


            TimeOfDayOperatingHour timeOfDayOperatingHour = new TimeOfDayOperatingHour();
            timeOfDayOperatingHour.Id = 0;
            timeOfDayOperatingHour.ClientId = clientModel.ClientId;
            timeOfDayOperatingHour.ZoneId = timeOfDayZone.ZoneId;
            timeOfDayOperatingHour.ClientModelId = timeOfDayZone.ClientModelId;

            timeOfDayOperatingHour.PeakSeasonHourlyRate = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakSeasonHourlyRate;
            timeOfDayOperatingHour.NonPeakSeasonHourlyRate = defaultModel.NonPeakSeasonHourlyRate;

            timeOfDayOperatingHour.OperatingHoursStart = defaultModel.TimeOfDayStartTime;
            timeOfDayOperatingHour.OperatingHoursEnd = defaultModel.TimeOfDayEndTime;
            timeOfDayOperatingHour.TotalHours = defaultModel.TimeOfDayTotalHours;

            timeOfDayOperatingHour.NonPeakOccupancyPercentage = defaultModel.NonPeakOccupancyPercentage;
            timeOfDayOperatingHour.PeakOccupancyPercentage = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakOccupancyPercentage;
            timeOfDayOperatingHour.IsActive = true;
            _context.TimeOfDayOperatingHours.Add(timeOfDayOperatingHour);

            OperatingDays operatingDays = new OperatingDays();
            operatingDays.Id = 0;
            operatingDays.ClientId = clientModel.ClientId;
            operatingDays.ClientModelId = clientModel.ClientModelId;
            operatingDays.ZoneId = timeOfDayZone.ZoneId;
            operatingDays.DaysPerYear = defaultModel.DaysPerYear;
            operatingDays.PeakDays = defaultModel.PeakDays;
            operatingDays.OffDays = defaultModel.OffDays;
            operatingDays.NonPeakDays = defaultModel.NonPeakDays;
            operatingDays.IsActive = true;
            _context.OperatingDays.Add(operatingDays);
            _context.SaveChanges();

            AddPermitsToZone(permits, clientModel, zone.ZoneCode, parkingClient.HavePermits);
        }

        private void AddEscalatingZone(ParkingClient parkingClient, ClientModel clientModel, Zone zone, List<Permit> permits)
        {
            EscalatingZone escalatingZone = new EscalatingZone();
            escalatingZone.ZoneId = 0;
            escalatingZone.ZoneCode = zone.ZoneCode;
            escalatingZone.ClientModelId = clientModel.ClientModelId;
            escalatingZone.ClientId = clientModel.ClientId;

            escalatingZone.NonPeakHourlyRate = defaultModel.NonPeakHourlyRate;
            escalatingZone.NonPeakEscalatingRate = defaultModel.NonPeakEscalatingRate;
            escalatingZone.NonPeakHourEscalatingRateBegins = defaultModel.NonPeakHourEscalatingRateBegins;
            escalatingZone.NonPeakDailyMaxOrAllDayRate = defaultModel.NonPeakDailyMaxOrAllDayRate;
            escalatingZone.NonPeakEveningFlatRate = defaultModel.NonPeakEveningFlatRate;

            escalatingZone.PeakHourlyRate = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakHourlyRate;
            escalatingZone.PeakEscalatingRate = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakEscalatingRate;
            escalatingZone.PeakHourEscalatingRateBegins = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakHourEscalatingRateBegins;
            escalatingZone.PeakDailyMaxOrAllDayRate = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakDailyMaxOrAllDayRate;
            escalatingZone.PeakEveningFlatRate = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakEveningFlatRate;

            escalatingZone.NumberOfSpacesPerZone = defaultModel.NumberOfSpacesPerZone;
            escalatingZone.PercentOfSpaceOccupied = defaultModel.PercentOfSpaceOccupied;
            escalatingZone.NumberOfSpacesRemaining = defaultModel.NumberOfSpacesRemaining;

            escalatingZone.CompliancePercentage = defaultModel.CompliancePercentage;

            escalatingZone.NonPeakOccupancyPercentage = defaultModel.NonPeakOccupancyPercentage;
            escalatingZone.PeakOccupancyPercentage = parkingClient.IsPeakSeasonPricing == false ? 0 : defaultModel.PeakOccupancyPercentage;
            escalatingZone.IsActive = true;
            escalatingZone.DailyHourlyPercentValuesJson = defaultModel.DailyHourlyPercentValuesJson;

            _context.EscalatingZones.Add(escalatingZone);
            _context.SaveChanges();

            List<int> escalatingHours = new List<int>() { (int)OperatingHourTypeEnum.Daily, (int)OperatingHourTypeEnum.Evening };
            foreach (int operatingHourTypeId in escalatingHours)
            {
                EscalatingOperatingHour escalatingOperatingHour = new EscalatingOperatingHour();
                escalatingOperatingHour.Id = 0;
                escalatingOperatingHour.ClientId = clientModel.ClientId;
                escalatingOperatingHour.ZoneId = escalatingZone.ZoneId;
                escalatingOperatingHour.ClientModelId = escalatingZone.ClientModelId;

                escalatingOperatingHour.StartTime = defaultModel.EscalatingStartTime;
                escalatingOperatingHour.EndTime = defaultModel.EscalatingEndTime;
                escalatingOperatingHour.TotalHours = defaultModel.EscalatingTotalHours;

                escalatingOperatingHour.OperatingHourType = operatingHourTypeId;
                escalatingOperatingHour.IsActive = true;
                _context.EscalatingOperatingHours.Add(escalatingOperatingHour);
                _context.SaveChanges();
            }

            OperatingDays operatingDays = new OperatingDays();
            operatingDays.Id = 0;
            operatingDays.ClientId = clientModel.ClientId;
            operatingDays.ClientModelId = clientModel.ClientModelId;
            operatingDays.ZoneId = escalatingZone.ZoneId;
            operatingDays.DaysPerYear = defaultModel.DaysPerYear;
            operatingDays.PeakDays = defaultModel.PeakDays;
            operatingDays.OffDays = defaultModel.OffDays;
            operatingDays.NonPeakDays = defaultModel.NonPeakDays;
            operatingDays.IsActive = true;
            _context.OperatingDays.Add(operatingDays);
            _context.SaveChanges();

            AddPermitsToZone(permits, clientModel, zone.ZoneCode, parkingClient.HavePermits);
        }

        private async Task<List<SetupHourlyModelDto>> GetHourlyModels(IQueryable<ClientModel> clientModels, ParkingClient parkingClient)
        {
            List<SetupHourlyModelDto> hourlyModelList = await (from cm in clientModels.AsNoTracking().Where(x => x.ModelTypeId == (int)ModelTypeEnum.Hourly && x.IsActive == true)
                                                                   //join pc in _context.ParkingClients on cm.ClientId equals pc.ClientId
                                                               join lpt in _context.LuParkingTypes.AsNoTracking() on cm.ParkingTypeId equals lpt.ParkingTypeId
                                                               join lmt in _context.LuModelTypes.AsNoTracking() on cm.ModelTypeId equals lmt.ModelTypeId
                                                               select new SetupHourlyModelDto
                                                               {
                                                                   ClientId = cm.ClientId,
                                                                   ClientModelId = cm.ClientModelId,
                                                                   ClientName = parkingClient.ClientName,
                                                                   ModelTypeId = lmt.ModelTypeId,
                                                                   IsSetupDone = cm.IsSetupDone,
                                                                   IsAvailable = cm.IsAvailable,
                                                                   SelectedModelType = new ModelTypeDto()
                                                                   {
                                                                       ModelTypeId = lmt.ModelTypeId,
                                                                       ModelTypeName = lmt.ModelTypeName,
                                                                   },
                                                                   ParkingTypeId = lpt.ParkingTypeId,
                                                                   SelectedParkingType = new ParkingTypeDto()
                                                                   {
                                                                       ParkingTypeId = lpt.ParkingTypeId,
                                                                       ParkingTypeName = lpt.ParkingTypeName,
                                                                   },
                                                                   HourlyZones = (from zi in _context.HourlyZones.Where(x => x.ClientModelId == cm.ClientModelId && x.IsActive == true)
                                                                                  join z in _context.Zones on zi.ZoneCode equals z.ZoneCode
                                                                                  select new HourlyZoneDto
                                                                                  {
                                                                                      ZoneId = zi.ZoneId,
                                                                                      ZoneCode = z.ZoneCode,
                                                                                      ZoneName = z.ZoneName,
                                                                                      ClientModelId = zi.ClientModelId,
                                                                                      ClientId = z.ClientId,

                                                                                      NonPeakSeasonHourlyRate = zi.NonPeakSeasonHourlyRate,
                                                                                      PeakSeasonHourlyRate = zi.PeakSeasonHourlyRate,

                                                                                      NumberOfSpacesPerZone = zi.NumberOfSpacesPerZone,
                                                                                      PercentOfSpaceOccupied = zi.PercentOfSpaceOccupied,
                                                                                      NumberOfSpacesRemaining = zi.NumberOfSpacesRemaining,

                                                                                      CompliancePercentage = zi.CompliancePercentage,

                                                                                      NonPeakOccupancyPercentage = zi.NonPeakOccupancyPercentage,
                                                                                      PeakOccupancyPercentage = zi.PeakOccupancyPercentage,
                                                                                      OperatingDays = (from d in _context.OperatingDays.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneId && c.IsActive == true)
                                                                                                       select new OperatingDaysDto
                                                                                                       {
                                                                                                           Id = d.Id,
                                                                                                           DaysPerYear = d.DaysPerYear,
                                                                                                           PeakDays = d.PeakDays,
                                                                                                           OffDays = d.OffDays,
                                                                                                           NonPeakDays = d.NonPeakDays,
                                                                                                           ClientId = d.ClientId,
                                                                                                           ClientModelId = d.ClientModelId,
                                                                                                           ZoneId = d.ZoneId
                                                                                                       }).FirstOrDefault(),
                                                                                      HourlyOperatingHour = (from oh in _context.HourlyOperatingHours
                                                                                                             where oh.IsActive == true
                                                                                                             select new HourlyOperatingHourDto
                                                                                                             {
                                                                                                                 Id = oh.Id,
                                                                                                                 ClientId = oh.ClientId,
                                                                                                                 ZoneId = oh.ZoneId,

                                                                                                                 StartTime = oh.StartTime,
                                                                                                                 EndTime = oh.EndTime,
                                                                                                                 TotalHours = oh.TotalHours,
                                                                                                             }).Where(c => c.ZoneId == zi.ZoneId).FirstOrDefault(),

                                                                                      ClientPermitTypes = (from pi in _context.PermitDetails.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneCode && c.IsActive == true)
                                                                                                           join p in _context.Permits on pi.PermitCode equals p.PermitCode
                                                                                                           select new ClientPermitTypeDto
                                                                                                           {
                                                                                                               PermitId = pi.PermitId,
                                                                                                               PermitCode = p.PermitCode,
                                                                                                               PermitName = p.PermitName,
                                                                                                               AnnualCost = pi.AnnualCost,
                                                                                                               QuantitySold = pi.QuantitySold,
                                                                                                               ClientId = pi.ClientId,
                                                                                                               ClientModelId = pi.ClientModelId,
                                                                                                               ZoneId = pi.ZoneId,
                                                                                                           }).ToList()
                                                                                  }).ToList()
                                                               }).ToListAsync();
            return hourlyModelList;
        }

        private async Task<List<SetupTimeOfDayModelDto>> GetTimeOfDayModels(IQueryable<ClientModel> clientModels, ParkingClient parkingClient)
        {
            List<SetupTimeOfDayModelDto> timeOfDayModelList = await (from cm in clientModels.AsNoTracking().Where(x => x.ModelTypeId == (int)ModelTypeEnum.TimeOfDay && x.IsActive == true)
                                                                         //join pc in _context.ParkingClients on cm.ClientId equals pc.ClientId
                                                                     join lpt in _context.LuParkingTypes.AsNoTracking() on cm.ParkingTypeId equals lpt.ParkingTypeId
                                                                     join lmt in _context.LuModelTypes.AsNoTracking() on cm.ModelTypeId equals lmt.ModelTypeId
                                                                     select new SetupTimeOfDayModelDto
                                                                     {
                                                                         ClientId = cm.ClientId,
                                                                         ClientModelId = cm.ClientModelId,
                                                                         ClientName = parkingClient.ClientName,
                                                                         ModelTypeId = lmt.ModelTypeId,
                                                                         IsSetupDone = cm.IsSetupDone,
                                                                         IsAvailable = cm.IsAvailable,
                                                                         SelectedModelType = new ModelTypeDto()
                                                                         {
                                                                             ModelTypeId = lmt.ModelTypeId,
                                                                             ModelTypeName = lmt.ModelTypeName,
                                                                         },
                                                                         ParkingTypeId = lpt.ParkingTypeId,
                                                                         SelectedParkingType = new ParkingTypeDto()
                                                                         {
                                                                             ParkingTypeId = lpt.ParkingTypeId,
                                                                             ParkingTypeName = lpt.ParkingTypeName,
                                                                         },
                                                                         TimeOfDayZones = (from zi in _context.TimeOfDayZones.Where(x => x.ClientModelId == cm.ClientModelId && x.IsActive == true)
                                                                                           join z in _context.Zones on zi.ZoneCode equals z.ZoneCode
                                                                                           select new TimeOfDayZoneDto
                                                                                           {
                                                                                               ZoneId = zi.ZoneId,
                                                                                               ZoneCode = z.ZoneCode,
                                                                                               ZoneName = z.ZoneName,
                                                                                               ClientModelId = zi.ClientModelId,
                                                                                               ClientId = z.ClientId,
                                                                                               NumberOfSpacesPerZone = zi.NumberOfSpacesPerZone,
                                                                                               PercentOfSpaceOccupied = zi.PercentOfSpaceOccupied,
                                                                                               NumberOfSpacesRemaining = zi.NumberOfSpacesRemaining,
                                                                                               CompliancePercentage = zi.CompliancePercentage,

                                                                                               OperatingDays = (from d in _context.OperatingDays.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneId && c.IsActive == true)
                                                                                                                select new OperatingDaysDto
                                                                                                                {
                                                                                                                    Id = d.Id,
                                                                                                                    DaysPerYear = d.DaysPerYear,
                                                                                                                    PeakDays = d.PeakDays,
                                                                                                                    OffDays = d.OffDays,
                                                                                                                    NonPeakDays = d.NonPeakDays,
                                                                                                                    ClientId = d.ClientId,
                                                                                                                    ClientModelId = d.ClientModelId,
                                                                                                                    ZoneId = d.ZoneId
                                                                                                                }).FirstOrDefault(),
                                                                                               HoursOfOperations = (from oh in _context.TimeOfDayOperatingHours.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneId && c.IsActive == true)
                                                                                                                    select new TimeOfDayOperatingHourDto
                                                                                                                    {
                                                                                                                        Id = oh.Id,
                                                                                                                        ClientId = oh.ClientId,
                                                                                                                        ClientModelId = oh.ClientModelId,
                                                                                                                        ZoneId = oh.ZoneId,
                                                                                                                        PeakSeasonHourlyRate = oh.PeakSeasonHourlyRate,
                                                                                                                        NonPeakSeasonHourlyRate = oh.NonPeakSeasonHourlyRate,
                                                                                                                        OperatingHoursStart = oh.OperatingHoursStart,
                                                                                                                        OperatingHoursEnd = oh.OperatingHoursEnd,
                                                                                                                        TotalHours = oh.TotalHours,
                                                                                                                        NonPeakOccupancyPercentage = oh.NonPeakOccupancyPercentage,
                                                                                                                        PeakOccupancyPercentage = oh.PeakOccupancyPercentage,
                                                                                                                        ActionType = ActionType.Modified
                                                                                                                    }).ToList(),
                                                                                               ClientPermitTypes = (from pi in _context.PermitDetails.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneCode && c.IsActive == true)
                                                                                                                    join p in _context.Permits on pi.PermitCode equals p.PermitCode
                                                                                                                    select new ClientPermitTypeDto
                                                                                                                    {
                                                                                                                        PermitId = pi.PermitId,
                                                                                                                        PermitCode = p.PermitCode,
                                                                                                                        PermitName = p.PermitName,
                                                                                                                        AnnualCost = pi.AnnualCost,
                                                                                                                        QuantitySold = pi.QuantitySold,
                                                                                                                        ClientId = pi.ClientId,
                                                                                                                        ClientModelId = pi.ClientModelId,
                                                                                                                        ZoneId = pi.ZoneId,
                                                                                                                    }).ToList()
                                                                                           }).ToList()
                                                                     }).ToListAsync();
            return timeOfDayModelList;
        }

        private async Task<List<SetupEscalatingModelDto>> GetEscalatingModels(IQueryable<ClientModel> clientModels, ParkingClient parkingClient)
        {
            List<SetupEscalatingModelDto> escalatingModelList = await (from cm in clientModels.AsNoTracking().Where(x => x.ModelTypeId == (int)ModelTypeEnum.Escalating && x.IsActive == true)
                                                                           //join pc in _context.ParkingClients on cm.ClientId equals pc.ClientId
                                                                       join lpt in _context.LuParkingTypes.AsNoTracking() on cm.ParkingTypeId equals lpt.ParkingTypeId
                                                                       join lmt in _context.LuModelTypes.AsNoTracking() on cm.ModelTypeId equals lmt.ModelTypeId
                                                                       select new SetupEscalatingModelDto
                                                                       {
                                                                           ClientId = cm.ClientId,
                                                                           ClientModelId = cm.ClientModelId,
                                                                           ClientName = parkingClient.ClientName,
                                                                           ModelTypeId = lmt.ModelTypeId,
                                                                           IsSetupDone = cm.IsSetupDone,
                                                                           IsAvailable = cm.IsAvailable,
                                                                           SelectedModelType = new ModelTypeDto()
                                                                           {
                                                                               ModelTypeId = lmt.ModelTypeId,
                                                                               ModelTypeName = lmt.ModelTypeName,
                                                                           },
                                                                           ParkingTypeId = lpt.ParkingTypeId,
                                                                           SelectedParkingType = new ParkingTypeDto()
                                                                           {
                                                                               ParkingTypeId = lpt.ParkingTypeId,
                                                                               ParkingTypeName = lpt.ParkingTypeName,
                                                                           },
                                                                           EscalatingZones = (from zi in _context.EscalatingZones.Where(x => x.ClientModelId == cm.ClientModelId && x.IsActive == true)
                                                                                              join z in _context.Zones on zi.ZoneCode equals z.ZoneCode
                                                                                              select new EscalatingZoneDto
                                                                                              {
                                                                                                  ZoneId = zi.ZoneId,
                                                                                                  ZoneCode = z.ZoneCode,
                                                                                                  ZoneName = z.ZoneName,
                                                                                                  ClientModelId = cm.ClientModelId,
                                                                                                  ClientId = cm.ClientId,

                                                                                                  NonPeakHourlyRate = zi.NonPeakHourlyRate,
                                                                                                  NonPeakEscalatingRate = zi.NonPeakEscalatingRate,
                                                                                                  NonPeakHourEscalatingRateBegins = zi.NonPeakHourEscalatingRateBegins,
                                                                                                  NonPeakDailyMaxOrAllDayRate = zi.NonPeakDailyMaxOrAllDayRate,
                                                                                                  NonPeakEveningFlatRate = zi.NonPeakEveningFlatRate,

                                                                                                  PeakHourlyRate = zi.PeakHourlyRate,
                                                                                                  PeakEscalatingRate = zi.PeakEscalatingRate,
                                                                                                  PeakHourEscalatingRateBegins = zi.PeakHourEscalatingRateBegins,
                                                                                                  PeakDailyMaxOrAllDayRate = zi.PeakDailyMaxOrAllDayRate,
                                                                                                  PeakEveningFlatRate = zi.PeakEveningFlatRate,

                                                                                                  NumberOfSpacesPerZone = zi.NumberOfSpacesPerZone,
                                                                                                  PercentOfSpaceOccupied = zi.PercentOfSpaceOccupied,
                                                                                                  NumberOfSpacesRemaining = zi.NumberOfSpacesRemaining,

                                                                                                  CompliancePercentage = zi.CompliancePercentage,

                                                                                                  NonPeakOccupancyPercentage = zi.NonPeakOccupancyPercentage,
                                                                                                  PeakOccupancyPercentage = zi.PeakOccupancyPercentage,
                                                                                                  DailyHourlyPercentValuesJson = zi.DailyHourlyPercentValuesJson,
                                                                                                  //DailyHourlyPercentValuesList = JsonConvert.DeserializeObject<List<HourlyPercentValue>>(zi.DailyHourlyPercentValuesJson),

                                                                                                  OperatingDays = (from d in _context.OperatingDays.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneId && c.IsActive == true)
                                                                                                                   select new OperatingDaysDto
                                                                                                                   {
                                                                                                                       Id = d.Id,
                                                                                                                       DaysPerYear = d.DaysPerYear,
                                                                                                                       PeakDays = d.PeakDays,
                                                                                                                       OffDays = d.OffDays,
                                                                                                                       NonPeakDays = d.NonPeakDays,
                                                                                                                       ClientId = d.ClientId,
                                                                                                                       ClientModelId = d.ClientModelId,
                                                                                                                       ZoneId = d.ZoneId
                                                                                                                   }).FirstOrDefault(),
                                                                                                  EscalatingOperatingHourDaily = (from oh in _context.EscalatingOperatingHours.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneId && c.OperatingHourType == (int)OperatingHourTypeEnum.Daily && c.IsActive == true)
                                                                                                                                  select new EscalatingOperatingHourDto
                                                                                                                                  {
                                                                                                                                      Id = oh.Id,
                                                                                                                                      ClientId = oh.ClientId,
                                                                                                                                      ClientModelId = oh.ClientModelId,
                                                                                                                                      ZoneId = oh.ZoneId,

                                                                                                                                      StartTime = oh.StartTime,
                                                                                                                                      EndTime = oh.EndTime,
                                                                                                                                      TotalHours = oh.TotalHours,
                                                                                                                                      OperatingHourType = oh.OperatingHourType
                                                                                                                                  }).FirstOrDefault(),
                                                                                                  EscalatingOperatingHourEvening = (from oh in _context.EscalatingOperatingHours.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneId && c.OperatingHourType == (int)OperatingHourTypeEnum.Evening && c.IsActive == true)
                                                                                                                                    select new EscalatingOperatingHourDto
                                                                                                                                    {
                                                                                                                                        Id = oh.Id,
                                                                                                                                        ClientId = oh.ClientId,
                                                                                                                                        ClientModelId = oh.ClientModelId,
                                                                                                                                        ZoneId = oh.ZoneId,

                                                                                                                                        StartTime = oh.StartTime,
                                                                                                                                        EndTime = oh.EndTime,
                                                                                                                                        TotalHours = oh.TotalHours,
                                                                                                                                        OperatingHourType = oh.OperatingHourType
                                                                                                                                    }).FirstOrDefault(),
                                                                                                  ClientPermitTypes = (from pi in _context.PermitDetails.Where(c => c.ClientId == cm.ClientId && c.ClientModelId == cm.ClientModelId && c.ZoneId == zi.ZoneCode && c.IsActive == true)
                                                                                                                       join p in _context.Permits on pi.PermitCode equals p.PermitCode
                                                                                                                       select new ClientPermitTypeDto
                                                                                                                       {
                                                                                                                           PermitId = pi.PermitId,
                                                                                                                           PermitCode = p.PermitCode,
                                                                                                                           PermitName = p.PermitName,
                                                                                                                           AnnualCost = pi.AnnualCost,
                                                                                                                           QuantitySold = pi.QuantitySold,
                                                                                                                           ClientId = pi.ClientId,
                                                                                                                           ClientModelId = pi.ClientModelId,
                                                                                                                           ZoneId = pi.ZoneId,
                                                                                                                       }).ToList()
                                                                                              }).ToList()
                                                                       }).ToListAsync();

            return escalatingModelList;
        }

        private EditClientModel GetEditClientModel(int clientId)
        {
            return (from pc in _context.ParkingClients.AsNoTracking()
                    where pc.IsActive == true
                    select new EditClientModel
                    {
                        ClientId = pc.ClientId,
                        ClientName = pc.ClientName,
                        IsPeakSeasonPricing = pc.IsPeakSeasonPricing,
                        HavePermits = pc.HavePermits,
                        IsActive = pc.IsActive,
                        OnStreetZones = (from z in _context.Zones.Where(x => x.ClientId == pc.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet && x.IsActive == true)
                                         select new AddZoneModel
                                         {
                                             ZoneCode = z.ZoneCode,
                                             ZoneName = z.ZoneName
                                         }).ToList(),
                        OffStreetZones = (from z in _context.Zones.Where(x => x.ClientId == pc.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet && x.IsActive == true)
                                          select new AddZoneModel
                                          {
                                              ZoneCode = z.ZoneCode,
                                              ZoneName = z.ZoneName
                                          }).ToList(),
                        GaragesZones = (from z in _context.Zones.Where(x => x.ClientId == pc.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.Garages && x.IsActive == true)
                                        select new AddZoneModel
                                        {
                                            ZoneCode = z.ZoneCode,
                                            ZoneName = z.ZoneName
                                        }).ToList(),
                        OnStreetPermits = (from p in _context.Permits.Where(x => x.ClientId == pc.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.OnStreet && x.IsActive == true)
                                           select new AddPermitModel
                                           {
                                               PermitCode = p.PermitCode,
                                               PermitName = p.PermitName
                                           }).ToList(),
                        OffStreetPermits = (from p in _context.Permits.Where(x => x.ClientId == pc.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.OffStreet && x.IsActive == true)
                                            select new AddPermitModel
                                            {
                                                PermitCode = p.PermitCode,
                                                PermitName = p.PermitName
                                            }).ToList(),
                        GaragesPermits = (from p in _context.Permits.Where(x => x.ClientId == pc.ClientId && x.ParkingTypeId == (int)ParkingTypeEnum.Garages && x.IsActive == true)
                                          select new AddPermitModel
                                          {
                                              PermitCode = p.PermitCode,
                                              PermitName = p.PermitName
                                          }).ToList(),
                    }).Where(x => x.ClientId == clientId).FirstOrDefault();
        }


        private async Task<LocationEquipmentCostModel> GetLocationEquipmentCost(int parkingTypeId, int clientId)
        {
            LocationEquipmentCostModel locationEquipmentCostModel = new LocationEquipmentCostModel();
            locationEquipmentCostModel.Zones = await (from z in _context.Zones.AsNoTracking().Where(x => x.ClientId == clientId && x.ParkingTypeId == parkingTypeId && x.IsActive == true)
                                                          //join e in _context.EquipmentCosts on z.ZoneCode equals e.ZoneCode
                                                      select new EquipmentZoneModel
                                                      {
                                                          ZoneCode = z.ZoneCode,
                                                          OperatingDays = z.OperatingDays,
                                                          ZoneName = z.ZoneName,
                                                          ClientId = clientId,
                                                          LocationType = ((ParkingTypeEnum)parkingTypeId).ToString(),
                                                          ParkingTypeId = parkingTypeId,
                                                          Equipments = (from e in _context.EquipmentCosts.Where(x => x.ZoneCode == z.ZoneCode && x.IsActive == true)
                                                                        select new EquipmentCostModel
                                                                        {
                                                                            EquipmentId = e.EquipmentId,
                                                                            UnitsOwned = e.UnitsOwned,
                                                                            UnitsPurchased = e.UnitsPurchased,
                                                                            CostOfBaseUnit = e.CostOfBaseUnit,
                                                                            WarrantyStartingYear = e.WarrantyStartingYear,
                                                                            MonthlyMeterSoftwareFees = e.MonthlyMeterSoftwareFees,

                                                                            QuantityOfUnits = e.QuantityOfUnits,
                                                                            MultiSpaceMeterCost = e.MultiSpaceMeterCost,
                                                                            EquipWithBNA = e.EquipWithBNA,
                                                                            EquipWithCreditCard = e.EquipWithCreditCard,
                                                                            AnnualSoftwareFee = e.AnnualSoftwareFee,
                                                                            IsWarrantyIncluded = e.IsWarrantyIncluded,

                                                                            MonthlyCreditCardProcessingFees = e.MonthlyCreditCardProcessingFees,
                                                                            EstimatedCreditCardTransaction = e.EstimatedCreditCardTransaction,
                                                                            EquipmentTypeId = e.EquipmentTypeId,
                                                                            ClientId = clientId,
                                                                            ZoneCode = z.ZoneCode,
                                                                            ParkingTypeId = parkingTypeId,
                                                                            ActionType = ActionType.Modified
                                                                        }).ToList()
                                                      }).ToListAsync();
            return locationEquipmentCostModel; ;
        }

        private void DeleteZone(int zoneCode)
        {
            Zone zone = _context.Zones.Where(x => x.ZoneCode == zoneCode).FirstOrDefault();
            if (zone != null)
            {
                zone.IsActive = false;
                _context.Entry(zone).State = EntityState.Modified;
                _context.SaveChanges();
            }
            HourlyZone hourlyZone = _context.HourlyZones.Where(x => x.ZoneCode == zoneCode).FirstOrDefault();
            if (hourlyZone != null)
            {
                hourlyZone.IsActive = false;
                _context.Entry(hourlyZone).State = EntityState.Modified;
                _context.SaveChanges();

                HourlyOperatingHour hourlyOperatingHour = _context.HourlyOperatingHours.Where(x => x.ZoneId == hourlyZone.ZoneId).FirstOrDefault();
                if (hourlyOperatingHour != null)
                {
                    hourlyOperatingHour.IsActive = false;
                    _context.Entry(hourlyOperatingHour).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                OperatingDays hourlyOperatingDays = _context.OperatingDays.Where(x => x.ZoneId == hourlyZone.ZoneId).FirstOrDefault();
                if (hourlyOperatingDays != null)
                {
                    hourlyOperatingDays.IsActive = false;
                    _context.Entry(hourlyOperatingDays).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            TimeOfDayZone timeOfDayZone = _context.TimeOfDayZones.Where(x => x.ZoneCode == zoneCode).FirstOrDefault();
            if (timeOfDayZone != null)
            {
                timeOfDayZone.IsActive = false;
                _context.Entry(timeOfDayZone).State = EntityState.Modified;
                _context.SaveChanges();

                TimeOfDayOperatingHour timeOfDayOperatingHour = _context.TimeOfDayOperatingHours.Where(x => x.ZoneId == timeOfDayZone.ZoneId).FirstOrDefault();
                if (timeOfDayOperatingHour != null)
                {
                    timeOfDayOperatingHour.IsActive = false;
                    _context.Entry(timeOfDayOperatingHour).State = EntityState.Modified;
                    _context.SaveChanges();
                }

                OperatingDays timeOfDayOperatingDays = _context.OperatingDays.Where(x => x.ZoneId == timeOfDayZone.ZoneId).FirstOrDefault();
                if (timeOfDayOperatingDays != null)
                {
                    timeOfDayOperatingDays.IsActive = false;
                    _context.Entry(timeOfDayOperatingDays).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            EscalatingZone escalatingZone = _context.EscalatingZones.Where(x => x.ZoneCode == zoneCode).FirstOrDefault();
            if (escalatingZone != null)
            {
                escalatingZone.IsActive = false;
                _context.Entry(escalatingZone).State = EntityState.Modified;
                _context.SaveChanges();

                List<EscalatingOperatingHour> escalatingOperatingHours = _context.EscalatingOperatingHours.Where(x => x.ZoneId == escalatingZone.ZoneId).ToList();
                if (escalatingOperatingHours.Any())
                {
                    foreach (var opHoure in escalatingOperatingHours)
                    {
                        opHoure.IsActive = false;
                        _context.Entry(opHoure).State = EntityState.Modified;
                        _context.SaveChanges();
                    }
                }

                OperatingDays escalatingOperatingDays = _context.OperatingDays.Where(x => x.ZoneId == timeOfDayZone.ZoneId).FirstOrDefault();
                if (escalatingOperatingDays != null)
                {
                    escalatingOperatingDays.IsActive = false;
                    _context.Entry(escalatingOperatingDays).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }

            EquipmentCost equipmentCost = _context.EquipmentCosts.Where(x => x.ZoneCode == zoneCode).FirstOrDefault();
            if (equipmentCost != null)
            {
                equipmentCost.IsActive = false;
                _context.Entry(equipmentCost).State = EntityState.Modified;
            }

            _context.SaveChanges();
        }


        #endregion Private Helper Methods
    }
}