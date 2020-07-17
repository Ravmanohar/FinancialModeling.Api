using FinancialModeling.Extensions;
using FinancialModeling.Models;
using FinancialModeling.Models.DataModels.Report;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialModeling.Services
{
    public class ExportService : IExportService
    {
        private static List<LuEquipmentType> luEquipmentTypes = new List<LuEquipmentType>() {
            new LuEquipmentType(){ Id= 1, Name= "Single Space Meters", TypeId= 1 },
            new LuEquipmentType(){ Id= 2, Name= "Dual Space Meters", TypeId= 2 },
            new LuEquipmentType(){ Id= 3, Name= "Pay Stations", TypeId= 3 },
            new LuEquipmentType(){ Id= 4, Name= "Mobile Payment", TypeId= 4 },
            new LuEquipmentType(){ Id= 5, Name= "PARCS Equipment", TypeId= 5 },
        };

        /// <summary>
        /// Adds Revenue Row to Workbook Sheet
        /// </summary>
        private void AddRevenueRow(ExcelWorksheet worksheet, int rowNumber, int columnNumber, string[] columnValues, bool isHeader = false)
        {
            //int columnNumber = 0;
            foreach (string columnValue in columnValues)
            {
                columnNumber++;
                ExcelHorizontalAlignment align = isHeader ? ExcelHorizontalAlignment.Center : ExcelHorizontalAlignment.Right;
                worksheet.Cells[rowNumber, columnNumber].AddAmountValue(columnValue, align);
                if (isHeader)
                    worksheet.Cells[rowNumber, columnNumber].Style.Font.Bold = true;
            }
        }

        private RevenueInfo GetYearRevenue(int hourly, int permit, int total, int year)
        {
            return new RevenueInfo()
            {
                Hourly = hourly * year,
                Permit = permit * year,
                Total = total * year,
            };
        }

        /// <summary>
        /// Adds Header Row To Sheet
        /// </summary>
        private void AddHeaderRowToSheet(ExcelWorksheet workSheet, ProjectedRevenueSummary projectedRevenueSummary, int rowNumber)
        {
            int rowEndColumn = (projectedRevenueSummary.ZoneSummaryList.Count() * 3) + 1;
            int col1 = 1;
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Value = projectedRevenueSummary.ReportHeader;
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Merge = true;
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Style.Font.Bold = true;
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Style.Font.Size = 18;
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 85, 146));
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[rowNumber, col1, rowNumber, rowEndColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        /// <summary>
        /// Adds Financial Dashboard Models Header Row To Sheet
        /// </summary>
        private void AddModelHeaderRowToFDSheet(ExcelWorksheet workSheet, int startColumn, int endColumn, string headerValue)
        {
            int rowNum = 2;
            endColumn = endColumn - 1;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Value = headerValue;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Merge = true;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Font.Bold = true;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Font.Size = 18;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 85, 146));
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        int parkingTypeColumnStart = 0, parkingTypeColumnEnd = 2;
        /// <summary>
        /// Adds Parking Type Header Row To Sheet
        /// </summary>
        private void AddParkingTypeHeaderToSheet(ExcelWorksheet workSheet, int startColumn, int endColumn, string headerValue)
        {
            int rowNum = 3;
            endColumn = endColumn - 1;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Value = headerValue;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Merge = true;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Font.Bold = true;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.Font.Size = 18;
            workSheet.Cells[rowNum, startColumn, rowNum, endColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

            parkingTypeColumnStart++;
            parkingTypeColumnEnd++;
        }

        /// <summary>
        /// Adds Header Row To Sheet
        /// </summary>
        private void AddEquipmentCostHeaderRowToSheet(ExcelWorksheet workSheet, ProjectedEquipmentCostSummary projectedCostSummary)
        {
            int rowEndColumn = projectedCostSummary.ZoneEquipmentList.Count() + 1;
            workSheet.Cells[1, 1, 1, rowEndColumn].Value = projectedCostSummary.ReportHeader;
            workSheet.Cells[1, 1, 1, rowEndColumn].Merge = true;
            workSheet.Cells[1, 1, 1, rowEndColumn].Style.Font.Bold = true;
            workSheet.Cells[1, 1, 1, rowEndColumn].Style.Font.Size = 18;
            workSheet.Cells[1, 1, 1, rowEndColumn].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[1, 1, 1, rowEndColumn].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 85, 146));
            workSheet.Cells[1, 1, 1, rowEndColumn].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[1, 1, 1, rowEndColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        /// <summary>
        /// Add Header Row Common Method
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="headerValue"></param>
        /// <param name="row"></param>
        /// <param name="endCol"></param>
        private void AddHeaderRow(ExcelWorksheet workSheet, string headerValue, int row, int endCol)
        {
            int col = 1;
            workSheet.Cells[row, col, row, endCol].Value = headerValue;
            workSheet.Cells[row, col, row, endCol].Merge = true;
            workSheet.Cells[row, col, row, endCol].Style.Font.Bold = true;
            workSheet.Cells[row, col, row, endCol].Style.Font.Size = 18;
            workSheet.Cells[row, col, row, endCol].Style.Fill.PatternType = ExcelFillStyle.Solid;
            workSheet.Cells[row, col, row, endCol].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(0, 85, 146));
            workSheet.Cells[row, col, row, endCol].Style.Font.Color.SetColor(Color.White);
            workSheet.Cells[row, col, row, endCol].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
        }

        private string GetPropertyNameByReportName(string reportName)
        {
            switch (reportName)
            {
                //case "Financial Dashboard":
                //    return "FinancialDashboard";
                case "Hourly On":
                    return "HourlyOnStreet";
                case "Hourly Off":
                    return "HourlyOffStreet";
                case "Hourly Garage":
                    return "HourlyGarages";
                case "Time of Day On":
                    return "TimeOfDayOnStreet";
                case "Time of Day Off":
                    return "TimeOfDayOffStreet";
                case "Time of Day Garage":
                    return "TimeOfDayGarages";
                case "Escalating On":
                    return "EscalatingOnStreet";
                case "Escalating Off":
                    return "EscalatingOffStreet";
                case "Escalating Garage":
                    return "EscalatingGarages";
                case "Equipment Cost On Street":
                    return "OnStreetEquipmentCost";
                case "Equipment Cost Off Street":
                    return "OffStreetEquipmentCost";
                case "Equipment Cost Garages":
                    return "GaragesEquipmentCost";
            }
            return null;
        }

        /// <summary>
        /// Add HourlyReportInputValuesToSheet
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="reportPropertyName"></param>
        /// <param name="financialDashboard"></param>
        private int AddHourlyReportInputValuesToSheet(ExcelWorksheet workSheet, string reportPropertyName, FinanclialDashboardModel financialDashboard)
        {
            SetupHourlyModelDto hourlyModel = (SetupHourlyModelDto)financialDashboard.GetType().GetProperty(reportPropertyName).GetValue(financialDashboard);
            int startRow = 3, r2 = 2, c1 = 1, c2 = 2, zoneRow1 = 1, zoneCol = 2, endCol = (hourlyModel.HourlyZones.Count + 1);
            string parkingTypeLable = hourlyModel.SelectedParkingType.ParkingTypeName.ToUpper().Replace(' ', '-');
            AddHeaderRow(workSheet, "HOURLY RATE MODEL REVENUE PROJECTIONS: " + parkingTypeLable, 1, endCol);

            Dictionary<string, string> labelInfo = new Dictionary<string, string>();
            labelInfo.Add("NonPeakSeasonHourlyRate", "NonPeak Season Hourly Rate");
            labelInfo.Add("PeakSeasonHourlyRate", "Peak Season Hourly Rate");
            labelInfo.Add("NumberOfSpacesPerZone", "Number Of Spaces Per Zone");
            labelInfo.Add("PercentOfSpaceOccupied", "Percent Of Space Occupied");
            labelInfo.Add("NumberOfSpacesRemaining", "Number Of Spaces Remaining");

            labelInfo.Add("StartTime", "Start Time");
            labelInfo.Add("EndTime", "End Time");

            labelInfo.Add("DaysPerYear", "Days Per Year");
            labelInfo.Add("PeakDays", "Peak Days");
            labelInfo.Add("NonPeakDays", "NonPeak Days");
            labelInfo.Add("OffDays", "Off Days");
            labelInfo.Add("CompliancePercentage", "Compliance Percentage");
            labelInfo.Add("NonPeakOccupancyPercentage", "NonPeak Occupancy Percentage");
            labelInfo.Add("PeakOccupancyPercentage", "Peak Occupancy Percentage");

            labelInfo.Add("QuantitySold", "Quantity Sold");
            labelInfo.Add("AnnualCost", "Annual Cost");

            //Start : Add Zone Names Headers
            foreach (var zone in hourlyModel.HourlyZones)
            {
                workSheet.Cells[r2, c2].AddValueWithStyles(zone.ZoneName, ExcelHorizontalAlignment.Center);
                c2++;
            }
            r2++;
            //End : Add Zone Names Headers


            string[] columns = new string[14] { "NonPeakSeasonHourlyRate", "PeakSeasonHourlyRate", "NumberOfSpacesPerZone", "PercentOfSpaceOccupied", "NumberOfSpacesRemaining", "StartTime", "EndTime", "DaysPerYear", "PeakDays", "NonPeakDays", "OffDays", "CompliancePercentage", "NonPeakOccupancyPercentage", "PeakOccupancyPercentage" };
            foreach (string column in columns)
            {
                workSheet.Cells[r2, c1].AddValueWithStyles(labelInfo[column]);
                r2++;
            }
            c1 = 2;
            foreach (HourlyZoneDto zone in hourlyModel.HourlyZones)
            {
                workSheet.Cells[zoneRow1, zoneCol++].AddValueWithStyles(zone.ZoneName);
                foreach (string column in columns)
                {
                    r2 = startRow;
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.NonPeakSeasonHourlyRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.PeakSeasonHourlyRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.NumberOfSpacesPerZone, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPercentValue(zone.PercentOfSpaceOccupied, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.NumberOfSpacesRemaining, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[r2++, c1].AddPlainValue(zone.HourlyOperatingHour.StartTime, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.HourlyOperatingHour.EndTime, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.DaysPerYear, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.PeakDays, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.NonPeakDays, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.OffDays, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[r2++, c1].AddPercentValue(zone.CompliancePercentage, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[r2++, c1].AddPercentValue(zone.NonPeakOccupancyPercentage, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPercentValue(zone.PeakOccupancyPercentage, ExcelHorizontalAlignment.Center);
                }
                c1++;
            }

            //Start: Add Permits to sheet
            int startPermitRow = r2 + 1;
            AddHeaderRow(workSheet, reportPropertyName + " PERMITS", startPermitRow, endCol);
            int permitRow = startPermitRow, col1 = 1, startCol = 2;

            int permitsCount = hourlyModel.HourlyZones.FirstOrDefault().ClientPermitTypes.Count;
            permitRow++;
            workSheet.Cells[permitRow, col1].AddValueWithStyles("Zones");

            foreach (var zone in hourlyModel.HourlyZones)
            {
                workSheet.Cells[permitRow, startCol].AddValueWithStyles(zone.ZoneName, ExcelHorizontalAlignment.Center);
                startCol++;
            }
            int pCol1 = 1;
            for (int i = 0; i < permitsCount; i++)
            {
                startCol = 2;
                foreach (var zone in hourlyModel.HourlyZones)
                {
                    workSheet.Cells[permitRow + 1, col1].AddValueWithStyles("Quantity Sold (" + zone.ClientPermitTypes[i].PermitName + ")");
                    workSheet.Cells[permitRow + 1, pCol1 + 1].AddPlainValue(zone.ClientPermitTypes[i].QuantitySold, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[permitRow + 2, col1].AddValueWithStyles("Annual Cost (" + zone.ClientPermitTypes[i].PermitName + ")");
                    workSheet.Cells[permitRow + 2, pCol1 + 1].AddPlainValue(zone.ClientPermitTypes[i].AnnualCost, ExcelHorizontalAlignment.Center);
                    pCol1++;
                }
                pCol1 = 1;
                permitRow = permitRow + 2;
            }
            //End: Add Permits to sheet
            int endingRow = permitRow;
            return endingRow;
        }

        //private void AddCellStyles(ExcelRangeBase cell, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left)
        //{
        //    cell.Style.Font.Bold = true;
        //    cell.Style.HorizontalAlignment = align;
        //}

        private void AddNormalCell(ExcelWorksheet workSheet, int row, int col, string value, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Right)
        {
            workSheet.Cells[row, col].AddPlainValue(value);
            workSheet.Cells[row, col].Style.HorizontalAlignment = align;
        }

        private void AddCell(ExcelWorksheet workSheet, int row, int col, string value, bool isBold = false, bool isMerge = false, int slots = 0, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Right)
        {
            if (isMerge)
            {
                int startCol = col; int endColm = col + slots - 1;
                workSheet.Cells[row, startCol, row, endColm].AddAmountValue(value);
                workSheet.Cells[row, startCol, row, endColm].Merge = true;
                workSheet.Cells[row, startCol, row, endColm].Style.HorizontalAlignment = align;
                if (isBold)
                    workSheet.Cells[row, startCol, row, endColm].Style.Font.Bold = true;
            }
            else
            {
                workSheet.Cells[row, col].AddAmountValue(value);
                if (isBold)
                    workSheet.Cells[row, col].Style.Font.Bold = true;
            }
        }

        /// <summary>
        /// Add TimeOfDayReportInputValuesToSheet
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="reportPropertyName"></param>
        /// <param name="financialDashboard"></param>
        private int AddTimeOfDayReportInputValuesToSheet(ExcelWorksheet workSheet, string reportPropertyName, FinanclialDashboardModel financialDashboard)
        {
            SetupTimeOfDayModelDto timeOfDayModel = (SetupTimeOfDayModelDto)financialDashboard.GetType().GetProperty(reportPropertyName).GetValue(financialDashboard);

            List<string> timeSlots = timeOfDayModel.TimeOfDayZones.FirstOrDefault().HoursOfOperations.Select(x => x.OperatingHoursStart + " - " + x.OperatingHoursEnd).ToList();
            int r2 = 2, c1 = 1, zoneRow1 = 1, zoneCol = 2, endCol = (timeOfDayModel.TimeOfDayZones.Count * timeSlots.Count) + 1;
            string parkingTypeLable = timeOfDayModel.SelectedParkingType.ParkingTypeName.ToUpper().Replace(' ', '-');
            AddHeaderRow(workSheet, "TIME OF DAY RATE MODEL REVENUE PROJECTIONS: " + parkingTypeLable, 1, endCol);

            Dictionary<string, string> labelInfo = new Dictionary<string, string>();
            labelInfo.Add("NumberOfSpacesPerZone", "NumberOfSpacesPerZone");
            labelInfo.Add("PercentOfSpaceOccupied", "PercentOfSpaceOccupied");
            labelInfo.Add("NumberOfSpacesRemaining", "NumberOfSpacesRemaining");
            labelInfo.Add("CompliancePercentage", "CompliancePercentage");

            labelInfo.Add("DaysPerYear", "DaysPerYear");
            labelInfo.Add("PeakDays", "PeakDays");
            labelInfo.Add("OffDays", "OffDays");
            labelInfo.Add("NonPeakDays", "NonPeakDays");

            labelInfo.Add("QuantitySold", "Quantity Sold");
            labelInfo.Add("AnnualCost", "Annual Cost");

            //Start : Add Zone Names Lable First
            r2++;
            workSheet.Cells[r2, c1++].Value = "";
            foreach (TimeOfDayZoneDto zone in timeOfDayModel.TimeOfDayZones)
            {
                //workSheet.Cells[r2, c1++, r2, c1 + timeSlots.Count].Value = zone.ZoneName;
                int endColm = c1 + zone.HoursOfOperations.Count - 1;
                workSheet.Cells[r2, c1, r2, endColm].Merge = true;
                workSheet.Cells[r2, c1, r2, endColm].Style.Font.Bold = true;
                workSheet.Cells[r2, c1, r2, endColm].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                workSheet.Cells[r2, c1, r2, endColm].Value = zone.ZoneName;
                c1 = endColm + 1;
            }
            //End : Add Zone Names Lable First

            c1 = 1;
            int cell1 = 1;
            r2++;
            int loopIndex = 0;
            bool mergeCells = false;
            int sc = mergeCells ? 1 : 2;//Stating Column
            int startPermitRow = 0;
            foreach (TimeOfDayZoneDto zone in timeOfDayModel.TimeOfDayZones)
            {
                int timeSlotsCount = zone.HoursOfOperations.Count;
                mergeCells = timeSlotsCount > 1;

                int sr = r2 + 1;//Starting Row
                foreach (var hour in zone.HoursOfOperations)
                {
                    sr = r2 + 1;//Starting Row

                    c1++;
                    workSheet.Cells[r2, c1].AddValueWithStyles(hour.OperatingHoursStart + " - " + hour.OperatingHoursEnd, ExcelHorizontalAlignment.Center);

                    AddNormalCell(workSheet, sr++, c1, hour.NonPeakSeasonHourlyRate.ToString().ToAmountFormat());
                    AddNormalCell(workSheet, sr++, c1, hour.PeakSeasonHourlyRate.ToString().ToAmountFormat());
                    AddNormalCell(workSheet, sr++, c1, hour.NonPeakOccupancyPercentage.ToString().ToPercentFormat());
                    AddNormalCell(workSheet, sr++, c1, hour.PeakOccupancyPercentage.ToString().ToPercentFormat());

                    AddNormalCell(workSheet, sr++, c1, hour.OperatingHoursStart);
                    AddNormalCell(workSheet, sr++, c1, hour.OperatingHoursEnd);
                    AddNormalCell(workSheet, sr++, c1, hour.TotalHours.ToString());
                }

                AddCell(workSheet, sr++, sc, zone.NumberOfSpacesPerZone.ToString(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);
                AddCell(workSheet, sr++, sc, zone.PercentOfSpaceOccupied.ToString().ToPercentFormat(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);
                AddCell(workSheet, sr++, sc, zone.NumberOfSpacesRemaining.ToString(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);

                AddCell(workSheet, sr++, sc, zone.OperatingDays.PeakDays.ToString(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);
                AddCell(workSheet, sr++, sc, zone.OperatingDays.NonPeakDays.ToString(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);
                AddCell(workSheet, sr++, sc, zone.OperatingDays.OffDays.ToString(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);
                AddCell(workSheet, sr++, sc, zone.OperatingDays.DaysPerYear.ToString(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);

                AddCell(workSheet, sr++, sc, zone.CompliancePercentage.ToString().ToPercentFormat(), false, mergeCells, timeSlotsCount, ExcelHorizontalAlignment.Center);

                if (mergeCells)
                    sc = sc + zone.HoursOfOperations.Count;

                //Start : Add Left Lables
                sr = r2 + 1;//Starting Row
                if (loopIndex == 0)
                {
                    AddCell(workSheet, sr++, cell1, "Hourly Rate Non Peak", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Hourly Rate Peak", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Occupancy Percentage Non Peak", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Occupancy Percentage Peak", true, false, 0, ExcelHorizontalAlignment.Left);

                    AddCell(workSheet, sr++, cell1, "Start Time", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "End Time", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Total Hours", true, false, 0, ExcelHorizontalAlignment.Left);

                    AddCell(workSheet, sr++, cell1, "Number Of Spaces Per Zone", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Percent Of Space Occupied", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Number Of Spaces Remaining", true, false, 0, ExcelHorizontalAlignment.Left);

                    AddCell(workSheet, sr++, cell1, "Peak Days", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Non Peak Days", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Off Days", true, false, 0, ExcelHorizontalAlignment.Left);
                    AddCell(workSheet, sr++, cell1, "Annual", true, false, 0, ExcelHorizontalAlignment.Left);

                    AddCell(workSheet, sr++, cell1, "Compliance Percentage", true, false, 0, ExcelHorizontalAlignment.Left);

                    //Add More Columns Here
                    startPermitRow = sr + 1;
                }
                //End : Add Left Lables
                loopIndex++;
            }

            //Start: Add Permits to sheet
            startPermitRow = startPermitRow + 1;
            AddHeaderRow(workSheet, reportPropertyName + " PERMITS", startPermitRow, endCol);
            int permitRow = startPermitRow, col1 = 1, startCol = 2;

            int permitsCount = timeOfDayModel.TimeOfDayZones.FirstOrDefault().ClientPermitTypes.Count;
            permitRow++;
            workSheet.Cells[permitRow, col1].AddValueWithStyles("Zones", ExcelHorizontalAlignment.Center);

            foreach (var zone in timeOfDayModel.TimeOfDayZones)
            {
                workSheet.Cells[permitRow, startCol].AddValueWithStyles(zone.ZoneName, ExcelHorizontalAlignment.Center);
                startCol++;
            }
            int pCol1 = 1;
            for (int i = 0; i < permitsCount; i++)
            {
                startCol = 2;
                foreach (var zone in timeOfDayModel.TimeOfDayZones)
                {
                    workSheet.Cells[permitRow + 1, col1].AddValueWithStyles("Quantity Sold (" + zone.ClientPermitTypes[i].PermitName + ")");
                    workSheet.Cells[permitRow + 1, pCol1 + 1].AddPlainValue(zone.ClientPermitTypes[i].QuantitySold, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[permitRow + 2, col1].AddValueWithStyles("Annual Cost (" + zone.ClientPermitTypes[i].PermitName + ")");
                    workSheet.Cells[permitRow + 2, pCol1 + 1].AddPlainValue(zone.ClientPermitTypes[i].AnnualCost, ExcelHorizontalAlignment.Center);
                    pCol1++;
                }
                pCol1 = 1;
                permitRow = permitRow + 2;
            }
            //End: Add Permits to sheet
            int endingRow = permitRow;
            return endingRow;
        }

        /// <summary>
        /// Add EscalatingReportInputValuesToSheet
        /// </summary>
        /// <param name="workSheet"></param>
        /// <param name="reportPropertyName"></param>
        /// <param name="financialDashboard"></param>
        private int AddEscalatingReportInputValuesToSheet(ExcelWorksheet workSheet, string reportPropertyName, FinanclialDashboardModel financialDashboard)
        {
            SetupEscalatingModelDto escalatingModel = (SetupEscalatingModelDto)financialDashboard.GetType().GetProperty(reportPropertyName).GetValue(financialDashboard);

            int startRow = 3, r2 = 2, c1 = 1, c2 = 2, zoneRow1 = 1, zoneCol = 2, endCol = (escalatingModel.EscalatingZones.Count + 1);
            string parkingTypeLable = escalatingModel.SelectedParkingType.ParkingTypeName.ToUpper().Replace(' ', '-');
            AddHeaderRow(workSheet, "ESCALATING RATE MODEL REVENUE PROJECTIONS: " + parkingTypeLable, 1, endCol);

            Dictionary<string, string> labelInfo = new Dictionary<string, string>();
            labelInfo.Add("NonPeakHourlyRate", "NonPeak Hourly Rate");
            labelInfo.Add("NonPeakEscalatingRate", "NonPeak Escalating Rate");
            labelInfo.Add("NonPeakHourEscalatingRateBegins", "NonPeak Escalating Rate Begins Hour");
            labelInfo.Add("NonPeakDailyMaxOrAllDayRate", "NonPeak Daily Max/All Day Rate");
            labelInfo.Add("NonPeakEveningFlatRate", "NonPeak Evening Flat Rate");
            labelInfo.Add("PeakHourlyRate", "Peak Hourly Rate");
            labelInfo.Add("PeakEscalatingRate", "Peak Escalating Rate");
            labelInfo.Add("PeakHourEscalatingRateBegins", "Peak Escalating Rate Begins Hour");
            labelInfo.Add("PeakDailyMaxOrAllDayRate", "Peak Daily Max/All Day Rate");
            labelInfo.Add("PeakEveningFlatRate", "Peak Evening Flat Rate");
            labelInfo.Add("NumberOfSpacesPerZone", "Number Of Spaces/Zone");
            labelInfo.Add("PercentOfSpaceOccupied", "Percent Of Spaces Occupied");
            labelInfo.Add("NumberOfSpacesRemaining", "Number Of Spaces Remaining");
            labelInfo.Add("CompliancePercentage", "Compliance Percentage");
            labelInfo.Add("NonPeakOccupancyPercentage", "NonPeak Occupancy Percentage");
            labelInfo.Add("PeakOccupancyPercentage", "Peak Occupancy Percentage");

            labelInfo.Add("DaysPerYear", "Days Per Year");
            labelInfo.Add("PeakDays", "Peak Days");
            labelInfo.Add("OffDays", "Off  Days");
            labelInfo.Add("NonPeakDays", "NonPeak Days");

            labelInfo.Add("StartTime", "Start Time");
            labelInfo.Add("EndTime", "End Time");
            labelInfo.Add("TotalHours", "Total Hours");
            labelInfo.Add("EveningStartTime", "Evening Start Time");
            labelInfo.Add("EveningEndTime", "Evening End Time");
            labelInfo.Add("EveningTotalHours", "Evening Total Hours");

            labelInfo.Add("QuantitySold", "Quantity Sold");
            labelInfo.Add("AnnualCost", "Annual Cost");

            //Start : Add Zone Names Headers
            foreach (var zone in escalatingModel.EscalatingZones)
            {
                workSheet.Cells[r2, c2].AddValueWithStyles(zone.ZoneName, ExcelHorizontalAlignment.Center);
                c2++;
            }
            r2++;
            //End : Add Zone Names Headers


            string[] columns = new string[26] { "NonPeakHourlyRate", "NonPeakEscalatingRate", "NonPeakHourEscalatingRateBegins", "NonPeakDailyMaxOrAllDayRate", "NonPeakEveningFlatRate", "PeakHourlyRate", "PeakEscalatingRate", "PeakHourEscalatingRateBegins", "PeakDailyMaxOrAllDayRate", "PeakEveningFlatRate", "NumberOfSpacesPerZone", "PercentOfSpaceOccupied", "NumberOfSpacesRemaining", "CompliancePercentage", "NonPeakOccupancyPercentage", "PeakOccupancyPercentage", "DaysPerYear", "PeakDays", "OffDays", "NonPeakDays", "StartTime", "EndTime", "TotalHours", "EveningStartTime", "EveningEndTime", "EveningTotalHours", };
            foreach (string column in columns)
            {
                workSheet.Cells[r2, c1].AddValueWithStyles(labelInfo[column]);
                r2++;
            }
            c1 = 2;
            foreach (EscalatingZoneDto zone in escalatingModel.EscalatingZones)
            {
                workSheet.Cells[zoneRow1, zoneCol++].Value = zone.ZoneName;
                foreach (string column in columns)
                {
                    r2 = startRow;
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.NonPeakHourlyRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.NonPeakEscalatingRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.NonPeakHourEscalatingRateBegins, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.NonPeakDailyMaxOrAllDayRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.NonPeakEveningFlatRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.PeakHourlyRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.PeakEscalatingRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.PeakHourEscalatingRateBegins, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.PeakDailyMaxOrAllDayRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddAmountValue(zone.PeakEveningFlatRate, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.NumberOfSpacesPerZone, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPercentValue(zone.PercentOfSpaceOccupied, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.NumberOfSpacesRemaining, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPercentValue(zone.CompliancePercentage, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPercentValue(zone.NonPeakOccupancyPercentage, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPercentValue(zone.PeakOccupancyPercentage, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.DaysPerYear, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.PeakDays, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.OffDays, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.OperatingDays.NonPeakDays, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[r2++, c1].AddPlainValue(zone.EscalatingOperatingHourDaily.StartTime, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.EscalatingOperatingHourDaily.EndTime, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.EscalatingOperatingHourDaily.TotalHours, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[r2++, c1].AddPlainValue(zone.EscalatingOperatingHourEvening.StartTime, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.EscalatingOperatingHourEvening.EndTime, ExcelHorizontalAlignment.Center);
                    workSheet.Cells[r2++, c1].AddPlainValue(zone.EscalatingOperatingHourEvening.TotalHours, ExcelHorizontalAlignment.Center);
                }
                c1++;
            }

            //Start: Add Permits to sheet
            int startPermitRow = r2 + 1;
            AddHeaderRow(workSheet, reportPropertyName + " PERMITS", startPermitRow, endCol);
            int permitRow = startPermitRow, col1 = 1, startCol = 2;

            int permitsCount = escalatingModel.EscalatingZones.FirstOrDefault().ClientPermitTypes.Count;
            permitRow++;
            workSheet.Cells[permitRow, col1].AddValueWithStyles("Zones", ExcelHorizontalAlignment.Center);

            foreach (var zone in escalatingModel.EscalatingZones)
            {
                workSheet.Cells[permitRow, startCol].AddValueWithStyles(zone.ZoneName, ExcelHorizontalAlignment.Center);
                startCol++;
            }
            int pCol1 = 1;
            for (int i = 0; i < permitsCount; i++)
            {
                startCol = 2;
                foreach (var zone in escalatingModel.EscalatingZones)
                {
                    workSheet.Cells[permitRow + 1, col1].AddValueWithStyles("Quantity Sold (" + zone.ClientPermitTypes[i].PermitName + ")");
                    workSheet.Cells[permitRow + 1, pCol1 + 1].AddPlainValue(zone.ClientPermitTypes[i].QuantitySold, ExcelHorizontalAlignment.Center);

                    workSheet.Cells[permitRow + 2, col1].AddValueWithStyles("Annual Cost (" + zone.ClientPermitTypes[i].PermitName + ")");
                    workSheet.Cells[permitRow + 2, pCol1 + 1].AddPlainValue(zone.ClientPermitTypes[i].AnnualCost, ExcelHorizontalAlignment.Center);
                    pCol1++;
                }
                pCol1 = 1;
                permitRow = permitRow + 2;
            }
            //End: Add Permits to sheet
            int endingRow = permitRow;
            return endingRow;
        }

        /// <summary>
        /// Adds Revenue calculations to Sheet
        /// </summary>
        private void AddParkingModelTypeSheet(ExcelPackage excel, ProjectedRevenueSummary projectedRevenueSummary, FinanclialDashboardModel financialDashboard)
        {
            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add(projectedRevenueSummary.ReportName);

            int startingRow = 1;
            var reportPropertyName = GetPropertyNameByReportName(projectedRevenueSummary.ReportName);
            if (reportPropertyName != null)
            {
                string[] hourlyReports = new string[3] { "HourlyOnStreet", "HourlyOffStreet", "HourlyGarages" };
                if (hourlyReports.FirstOrDefault(x => x == reportPropertyName) != null)
                {
                    startingRow = AddHourlyReportInputValuesToSheet(workSheet, reportPropertyName, financialDashboard);
                }
                string[] timeOfDayReports = new string[3] { "TimeOfDayOnStreet", "TimeOfDayOffStreet", "TimeOfDayGarages" };
                if (timeOfDayReports.FirstOrDefault(x => x == reportPropertyName) != null)
                {
                    startingRow = AddTimeOfDayReportInputValuesToSheet(workSheet, reportPropertyName, financialDashboard);
                }
                string[] escalatingReports = new string[3] { "EscalatingOnStreet", "EscalatingOffStreet", "EscalatingGarages" };
                if (escalatingReports.FirstOrDefault(x => x == reportPropertyName) != null)
                {
                    startingRow = AddEscalatingReportInputValuesToSheet(workSheet, reportPropertyName, financialDashboard);
                }
                startingRow = startingRow + 2;
            }

            string[] columnValues;
            int row1 = startingRow, row2 = row1 + 1, row3 = row1 + 2, row4 = row1 + 3, row5 = row1 + 4, row6 = row1 + 5;
            int col1 = 1;
            int startColumn, endColumn, totalColumns = 3;
            ReportColumns reportColumns = new ReportColumns();
            int[] calcuateForYears = new int[5] { 1, 2, 3, 4, 5 };

            int startSideHeadersFromRow = row2 + 1;

            int skipRows = 0;
            RevenueInfo revenue;
            AddHeaderRowToSheet(workSheet, projectedRevenueSummary, row1);

            foreach (int year in calcuateForYears)
            {
                reportColumns = new ReportColumns();
                if (financialDashboard.EditClientModel.IsPeakSeasonPricing)
                {
                    columnValues = new string[4] { "", "Revenue Without Peak Season", "Revenue With Peak Season", "Variance" };
                    skipRows = 4;
                }
                else
                {
                    columnValues = new string[2] { "", "Revenue Without Peak Season" };
                    skipRows = 2;
                }

                foreach (var sideHeader in columnValues)
                {
                    string yearString = "";
                    if (!string.IsNullOrEmpty(sideHeader) && sideHeader != "Variance")
                        yearString = year == 1 ? " - Year " + year : " - Years 1 - " + year;

                    workSheet.Cells[startSideHeadersFromRow, col1].AddValueWithStyles(sideHeader + yearString);
                    startSideHeadersFromRow++;
                }

                foreach (var zoneSummary in projectedRevenueSummary.ZoneSummaryList)
                {
                    if (year == 1)
                    {
                        startColumn = reportColumns.ZoneColumn + 1;
                        reportColumns.ZoneColumn = reportColumns.ZoneColumn + totalColumns;
                        endColumn = reportColumns.ZoneColumn;
                        workSheet.Cells[row2, startColumn, row2, endColumn].AddValueWithStyles(zoneSummary.ZoneName, ExcelHorizontalAlignment.Center);
                        workSheet.Cells[row2, startColumn, row2, endColumn].Merge = true;

                        columnValues = new string[3] { "Revenue", "Permit", "Total" };
                        AddRevenueRow(workSheet, row3, reportColumns.HeaderColumn, columnValues, true);
                        reportColumns.HeaderColumn = reportColumns.HeaderColumn + totalColumns;
                    }

                    revenue = GetYearRevenue(zoneSummary.NonPeak.Hourly, zoneSummary.NonPeak.Permit, zoneSummary.NonPeak.Total, year);
                    columnValues = new string[3] { revenue.Hourly.ToString(), revenue.Permit.ToString(), revenue.Total.ToString() };
                    AddRevenueRow(workSheet, row4, reportColumns.HourlyColumn, columnValues);
                    reportColumns.HourlyColumn = reportColumns.HourlyColumn + totalColumns;

                    if (financialDashboard.EditClientModel.IsPeakSeasonPricing)
                    {
                        revenue = GetYearRevenue(zoneSummary.Peak.Hourly, zoneSummary.Peak.Permit, zoneSummary.Peak.Total, year);
                        columnValues = new string[3] { revenue.Hourly.ToString(), revenue.Permit.ToString(), revenue.Total.ToString() };
                        AddRevenueRow(workSheet, row5, reportColumns.PermitColumn, columnValues);
                        reportColumns.PermitColumn = reportColumns.PermitColumn + totalColumns;

                        revenue = GetYearRevenue(zoneSummary.Variance.Hourly, zoneSummary.Variance.Permit, zoneSummary.Variance.Total, year);
                        columnValues = new string[3] { revenue.Hourly.ToString(), revenue.Permit.ToString(), revenue.Total.ToString() };
                        AddRevenueRow(workSheet, row6, reportColumns.TotalColumn, columnValues);
                        reportColumns.TotalColumn = reportColumns.TotalColumn + totalColumns;
                    }
                }
                row4 = row4 + skipRows;
                row5 = row5 + skipRows;
                row6 = row6 + skipRows;
                startSideHeadersFromRow = (row4 - 1);
            }

            workSheet.Cells.AutoFitColumns();
        }

        /// <summary>
        /// Adds Equipment Cost calculations to Sheet
        /// </summary>
        private void AddEquipmentCostSheet(ExcelPackage excel, ProjectedEquipmentCostSummary projectedCostSummary, FinanclialDashboardModel financialDashboard)
        {
            List<string> zoneEquipmentRows = new List<string> {
                "EquipmentCost", "EstimatedSoftwareFees", "EstimatedCreditCardTransactionFees", "EstimatedCostOfAdditionalSparesAndMisc", "SubtotalOperatingCost", "Total",
                //"emptyRow",
                "WarrantyCostYear2", "WarrantyCostYear3", "WarrantyCostYear4", "WarrantyCostYear5",
                //"emptyRow",
                "TotalEstimatedEquipmentAndOperatingCost1", "TotalEstimatedEquipmentAndOperatingCost2", "TotalEstimatedEquipmentAndOperatingCost3", "TotalEstimatedEquipmentAndOperatingCost4", "TotalEstimatedEquipmentAndOperatingCost5"
            };

            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add(projectedCostSummary.ReportName);

            string reportPropertyName = GetPropertyNameByReportName(projectedCostSummary.ReportName);
            LocationEquipmentCostModel equipmentCostModel = (LocationEquipmentCostModel)financialDashboard.GetType().GetProperty(reportPropertyName).GetValue(financialDashboard);

            int c1 = 2, rowNumber = 1, columnNumber = 0;
            int colSpan = 10;
            AddHeaderRow(workSheet, "SUMMARY OF COSTS", rowNumber, colSpan);
            rowNumber++;

            foreach (EquipmentZoneModel zone in equipmentCostModel.Zones)
            {
                int startCol = c1; int endColm = c1 + zone.Equipments.Count - 1;
                workSheet.Cells[rowNumber, startCol, rowNumber, endColm].AddValueWithStyles(zone.ZoneName, ExcelHorizontalAlignment.Center);
                workSheet.Cells[rowNumber, startCol, rowNumber, endColm].Merge = true;
                c1 = endColm + 1;
            }
            rowNumber++;
            int headerRowStart = rowNumber, cell1 = 1;

            AddCell(workSheet, headerRowStart++, cell1, "Equipment Type", true, false, 0, ExcelHorizontalAlignment.Left);
            AddCell(workSheet, headerRowStart++, cell1, "Units Owned", true, false, 0, ExcelHorizontalAlignment.Left);
            AddCell(workSheet, headerRowStart++, cell1, "Units Purchased", true, false, 0, ExcelHorizontalAlignment.Left);
            if (reportPropertyName == "OnStreetEquipmentCost")
            {
                AddCell(workSheet, headerRowStart++, cell1, "Cost of Base Unit", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Annual Warranty", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Monthly Meter Software Fees", true, false, 0, ExcelHorizontalAlignment.Left);
            }
            else
            {
                AddCell(workSheet, headerRowStart++, cell1, "Garage Access Points (Ingress/Egress)", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Cost of Base Unit", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Pay on Foot Equip with BNA", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Pay on Foot Equip with Credit Card", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Monthly Software Fees - Per Unit / Garage", true, false, 0, ExcelHorizontalAlignment.Left);
                AddCell(workSheet, headerRowStart++, cell1, "Warranty", true, false, 0, ExcelHorizontalAlignment.Left);
            }
            AddCell(workSheet, headerRowStart++, cell1, "Monthly CC Processing Fees - Per Transaction", true, false, 0, ExcelHorizontalAlignment.Left);
            AddCell(workSheet, headerRowStart++, cell1, "Estimated # of Credit Card Trans Per Unit / Per Day", true, false, 0, ExcelHorizontalAlignment.Left);

            c1 = 2;
            int sr = 0;
            string equipmentName = "";
            foreach (EquipmentZoneModel zone in equipmentCostModel.Zones)
            {
                foreach (EquipmentCostModel equipment in zone.Equipments)
                {
                    sr = rowNumber;
                    LuEquipmentType luEquipmentType = luEquipmentTypes.Find(x => x.Id == equipment.EquipmentTypeId);
                    equipmentName = equipment.EquipmentTypeId.ToString();
                    if (luEquipmentType != null)
                        equipmentName = luEquipmentType.Name;

                    AddNormalCell(workSheet, sr++, c1, equipmentName);
                    AddNormalCell(workSheet, sr++, c1, equipment.UnitsOwned.ToString());
                    AddNormalCell(workSheet, sr++, c1, equipment.UnitsPurchased.ToString());
                    if (reportPropertyName == "OnStreetEquipmentCost")
                    {
                        AddCell(workSheet, sr++, c1, equipment.CostOfBaseUnit.ToString().ToAmountFormat());
                        AddCell(workSheet, sr++, c1, equipment.WarrantyStartingYear.ToString().ToAmountFormat());
                        AddCell(workSheet, sr++, c1, equipment.MonthlyMeterSoftwareFees.ToString().ToAmountFormat());
                    }
                    else
                    {
                        AddNormalCell(workSheet, sr++, c1, equipment.QuantityOfUnits.ToString());
                        AddCell(workSheet, sr++, c1, equipment.CostOfBaseUnit.ToString().ToAmountFormat());
                        AddCell(workSheet, sr++, c1, equipment.EquipWithBNA.ToString());
                        AddCell(workSheet, sr++, c1, equipment.EquipWithCreditCard.ToString());
                        AddCell(workSheet, sr++, c1, equipment.AnnualSoftwareFee.ToString().ToAmountFormat());
                        AddCell(workSheet, sr++, c1, equipment.WarrantyStartingYear.ToString().ToAmountFormat());
                    }
                    AddCell(workSheet, sr++, c1, equipment.MonthlyCreditCardProcessingFees.ToString().ToAmountFormat());
                    AddNormalCell(workSheet, sr++, c1, equipment.EstimatedCreditCardTransaction.ToString());
                    c1++;
                }
            }

            rowNumber = sr + 1;
            AddHeaderRow(workSheet, projectedCostSummary.ReportHeader, rowNumber, colSpan);
            //AddEquipmentCostHeaderRowToSheet(workSheet, projectedCostSummary);

            List<string> columnValues = new List<string>();

            rowNumber++;
            columnNumber = 0;
            columnValues.Add("");
            foreach (ZoneEquipmentSummary zoneEquipmentSummary in projectedCostSummary.ZoneEquipmentList)
            {
                columnValues.Add(zoneEquipmentSummary.ZoneName);
            }
            foreach (string columnValue in columnValues)
            {
                columnNumber++;
                workSheet.Cells[rowNumber, columnNumber].AddValueWithStyles(columnValue, ExcelHorizontalAlignment.Center);
            }
            rowNumber++;

            foreach (string row in zoneEquipmentRows)
            {
                columnNumber = 0;
                columnValues = new List<string>();
                columnValues.Add(row);
                foreach (ZoneEquipmentSummary zoneEquipmentSummary in projectedCostSummary.ZoneEquipmentList)
                {
                    string value = zoneEquipmentSummary.GetType().GetProperty(row).GetValue(zoneEquipmentSummary).ToString();
                    columnValues.Add(value);
                }

                foreach (string columnValue in columnValues)
                {
                    columnNumber++;
                    //This will add space in word with CamelCase letter
                    string sideLableValue = System.Text.RegularExpressions.Regex.Replace(columnValue, "(\\B[A-Z])", " $1");
                    workSheet.Cells[rowNumber, columnNumber].AddAmountValue(sideLableValue, ExcelHorizontalAlignment.Right);
                    //If columnNumber == 1 means its header column
                    if (columnNumber == 1)
                    {
                        workSheet.Cells[rowNumber, columnNumber].Style.Font.Bold = true;
                        workSheet.Cells[rowNumber, columnNumber].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }
                }
                rowNumber++;
            }
            workSheet.Cells.AutoFitColumns();
            //}
        }


        /// <summary>
        /// Adds Financial Dashboard Sheet
        /// </summary>
        /// <param name="reportModel"></param>
        private void AddFinancialDashboardSheet(ExcelPackage excel, ReportModel reportModel)
        {
            ExcelWorksheet workSheet = excel.Workbook.Worksheets.Add("Financial Dashboard");

            //int startColumn = 2;
            ColumnInfo columnInfo = new ColumnInfo();
            columnInfo.ColumnNumber = 2;
            columnInfo = AddRevenueModelToFinancialDashboardSheet(workSheet, reportModel.FinancialDashboardRevenue.HourlyRevenueModel, columnInfo.ColumnNumber, reportModel, "Hourly");
            if (reportModel.FinancialDashboard.HourlyOnStreet.IsAvailable || reportModel.FinancialDashboard.HourlyOffStreet.IsAvailable || reportModel.FinancialDashboard.HourlyGarages.IsAvailable)
                AddModelHeaderRowToFDSheet(workSheet, columnInfo.StartHeaderColumn, columnInfo.EndHeaderColumn, "Hourly Revenue Model");

            columnInfo = AddRevenueModelToFinancialDashboardSheet(workSheet, reportModel.FinancialDashboardRevenue.TimeOfDayRevenueModel, columnInfo.ColumnNumber, reportModel, "TimeOfDay");
            if (reportModel.FinancialDashboard.TimeOfDayOnStreet.IsAvailable || reportModel.FinancialDashboard.TimeOfDayOffStreet.IsAvailable || reportModel.FinancialDashboard.TimeOfDayGarages.IsAvailable)
                AddModelHeaderRowToFDSheet(workSheet, columnInfo.StartHeaderColumn, columnInfo.EndHeaderColumn, "TimeOfDay Revenue Model");

            columnInfo = AddRevenueModelToFinancialDashboardSheet(workSheet, reportModel.FinancialDashboardRevenue.EscalatingRevenueModel, columnInfo.ColumnNumber, reportModel, "Escalating");
            if (reportModel.FinancialDashboard.EscalatingOnStreet.IsAvailable || reportModel.FinancialDashboard.EscalatingOffStreet.IsAvailable || reportModel.FinancialDashboard.EscalatingGarages.IsAvailable)
                AddModelHeaderRowToFDSheet(workSheet, columnInfo.StartHeaderColumn, columnInfo.EndHeaderColumn, "Escalating Revenue Model");

            workSheet.Cells.AutoFitColumns();
        }

        /// <summary>
        /// Adds Revenue Model to Financial Dashboard Sheet
        /// </summary>
        private ColumnInfo AddRevenueModelToFinancialDashboardSheet(ExcelWorksheet workSheet, RevenueModel revenueModel, int startColumn, ReportModel reportModel, string modelName)
        {
            ColumnInfo columnInfo = new ColumnInfo();
            columnInfo.StartHeaderColumn = startColumn;

            List<int> years = new List<int>() { 1, 2, 3, 4, 5 };

            List<string> hourlyModelProperties = revenueModel.GetType().GetProperties().Select(x => x.Name).ToList();

            Years yearlyRevenue = null;
            int col1 = 1, columnNumber = startColumn;
            int startingColumnNumber = columnNumber, zonesCount = 0;
            foreach (string property in hourlyModelProperties)
            {
                zonesCount = 0;
                int startingRow = 4;
                int rowNumber = 4;
                yearlyRevenue = (Years)revenueModel.GetType().GetProperty(property).GetValue(revenueModel);

                string modelPropName = modelName + property;
                bool isModelAvailable = GetIsAvailable(reportModel, modelPropName);
                yearlyRevenue.IsAvailable = ((List<ZoneRevenueSummary>)yearlyRevenue.GetType().GetProperty("Year1").GetValue(yearlyRevenue)).Count > 1;
                if (isModelAvailable && yearlyRevenue.IsAvailable)
                {
                    columnNumber = startingColumnNumber;

                    foreach (int year in years)
                    {
                        int startingRowNumber = rowNumber;
                        List<ZoneRevenueSummary> zoneRevenueSummaries = (List<ZoneRevenueSummary>)yearlyRevenue.GetType().GetProperty("Year" + year).GetValue(yearlyRevenue);
                        zonesCount = zoneRevenueSummaries.Count;
                        foreach (ZoneRevenueSummary zoneRevenueSummary in zoneRevenueSummaries)
                        {
                            rowNumber = startingRowNumber;
                            if (rowNumber == startingRow)
                            {
                                workSheet.Cells[rowNumber, columnNumber].AddValueWithStyles(zoneRevenueSummary.ZoneName, ExcelHorizontalAlignment.Center);
                                rowNumber++;
                            }

                            workSheet.Cells[rowNumber, col1].AddValueWithStyles("Annual Revenue Without Peak Season Pricing - Year " + year);
                            workSheet.Cells[rowNumber, columnNumber].AddAmountValue(zoneRevenueSummary.NonPeak.Revenue);
                            rowNumber++;

                            workSheet.Cells[rowNumber, col1].AddValueWithStyles("Equipment & Operating Cost - Year " + year);
                            workSheet.Cells[rowNumber, columnNumber].AddAmountValue(zoneRevenueSummary.NonPeak.Cost);
                            rowNumber++;

                            workSheet.Cells[rowNumber, col1].AddValueWithStyles("Net Gain/Loss");
                            workSheet.Cells[rowNumber, columnNumber].AddAmountValue(zoneRevenueSummary.NonPeak.Gain);

                            if (reportModel.FinancialDashboard.EditClientModel.IsPeakSeasonPricing)
                            {
                                rowNumber++;
                                rowNumber++;

                                workSheet.Cells[rowNumber, col1].AddValueWithStyles("Annual Revenue Peak Season Pricing - Year " + year);
                                workSheet.Cells[rowNumber, columnNumber].AddAmountValue(zoneRevenueSummary.Peak.Revenue);
                                rowNumber++;

                                workSheet.Cells[rowNumber, col1].AddValueWithStyles("Equipment & Operating Cost - Year " + year);
                                workSheet.Cells[rowNumber, columnNumber].AddAmountValue(zoneRevenueSummary.Peak.Cost);
                                rowNumber++;

                                workSheet.Cells[rowNumber, col1].AddValueWithStyles("Net Gain/Loss");
                                workSheet.Cells[rowNumber, columnNumber].AddAmountValue(zoneRevenueSummary.Peak.Gain);
                            }

                            columnNumber++;
                        }
                        columnNumber = startingColumnNumber;
                        rowNumber = rowNumber + 2;
                    }
                    startingColumnNumber += (zonesCount + 1);
                    columnInfo.ZonesCount += (zonesCount + 1);

                    int endColumn = 0;
                    parkingTypeColumnStart = parkingTypeColumnEnd;
                    parkingTypeColumnEnd += zonesCount;
                    endColumn = parkingTypeColumnEnd;
                    AddParkingTypeHeaderToSheet(workSheet, parkingTypeColumnStart, endColumn, property);
                }
            }
            columnInfo.ColumnNumber = startingColumnNumber;
            columnInfo.EndHeaderColumn = columnInfo.ColumnNumber - 1;
            return columnInfo;
        }

        // Returns If Model is available
        private bool GetIsAvailable(ReportModel reportModel, string modelPropName)
        {
            bool isAvailable = false;
            switch (modelPropName)
            {
                case "HourlyOnStreet":
                    isAvailable = reportModel.FinancialDashboard.HourlyOnStreet.IsAvailable;
                    break;
                case "HourlyOffStreet":
                    isAvailable = reportModel.FinancialDashboard.HourlyOffStreet.IsAvailable;
                    break;
                case "HourlyGarages":
                    isAvailable = reportModel.FinancialDashboard.HourlyGarages.IsAvailable;
                    break;
                case "TimeOfDayOnStreet":
                    isAvailable = reportModel.FinancialDashboard.TimeOfDayOnStreet.IsAvailable;
                    break;
                case "TimeOfDayOffStreet":
                    isAvailable = reportModel.FinancialDashboard.TimeOfDayOffStreet.IsAvailable;
                    break;
                case "TimeOfDayGarages":
                    isAvailable = reportModel.FinancialDashboard.TimeOfDayGarages.IsAvailable;
                    break;
                case "EscalatingOnStreet":
                    isAvailable = reportModel.FinancialDashboard.EscalatingOnStreet.IsAvailable;
                    break;
                case "EscalatingOffStreet":
                    isAvailable = reportModel.FinancialDashboard.EscalatingOffStreet.IsAvailable;
                    break;
                case "EscalatingGarages":
                    isAvailable = reportModel.FinancialDashboard.EscalatingGarages.IsAvailable;
                    break;
            }
            return isAvailable;
        }


        public byte[] DownloadFiveYearExcelReport(ReportModel reportModel)
        {
            byte[] bytes;
            //ExcelPackage excel = new ExcelPackage();
            using (var excel = new ExcelPackage())
            {
                //Add financial dashboard revenue sheet
                AddFinancialDashboardSheet(excel, reportModel);

                foreach (ProjectedRevenueSummary projectedRevenueSummary in reportModel.ProjectedRevenueSummaries)
                {
                    if (projectedRevenueSummary.IsAvailable)
                        AddParkingModelTypeSheet(excel, projectedRevenueSummary, reportModel.FinancialDashboard);
                }

                foreach (ProjectedEquipmentCostSummary projectedCostSummary in reportModel.ProjectedEquipmentCostSummaries)
                {
                    if (projectedCostSummary.IsAvailable)
                        AddEquipmentCostSheet(excel, projectedCostSummary, reportModel.FinancialDashboard);
                }

                string path = System.Web.HttpRuntime.AppDomainAppPath;
                string clientName = reportModel.FinancialDashboard.EditClientModel.ClientName;

                //FileInfo excelFile = new FileInfo(path + "\\Excel\\" + clientName + ".xlsx");
                //excel.SaveAs(excelFile);
                bytes = excel.GetAsByteArray();
            }
            return bytes;
        }

        public class ColumnInfo
        {
            public int ColumnNumber { get; set; }
            public int ZonesCount { get; set; }

            public int StartHeaderColumn { get; set; }
            public int EndHeaderColumn { get; set; }
        }

        public class LuEquipmentType
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int TypeId { get; set; }
        }
    }
}