using FinancialModeling.Models.DBModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class DefaultModel
    {
        public int NonPeakSeasonHourlyRate { get; set; }
        public int PeakSeasonHourlyRate { get; set; }

        public int NumberOfSpacesPerZone { get; set; }
        public int PercentOfSpaceOccupied { get; set; }
        public int NumberOfSpacesRemaining { get; set; }


        public int CompliancePercentage { get; set; }

        public int NonPeakOccupancyPercentage { get; set; }
        public int PeakOccupancyPercentage { get; set; }

        public string HourlyStartTime { get; set; }
        public string HourlyEndTime { get; set; }
        public int HourlyTotalHours { get; set; }


        public int DaysPerYear { get; set; }
        public int PeakDays { get; set; }
        public int OffDays { get; set; }
        public int NonPeakDays { get; set; }

        public int AnnualCost { get; set; }
        public int QuantitySold { get; set; }


        public string TimeOfDayStartTime { get; set; }
        public string TimeOfDayEndTime { get; set; }
        public int TimeOfDayTotalHours { get; set; }




        public int NonPeakHourlyRate { get; set; }
        public int NonPeakEscalatingRate { get; set; }
        public int NonPeakHourEscalatingRateBegins { get; set; }
        public int NonPeakDailyMaxOrAllDayRate { get; set; }
        public int NonPeakEveningFlatRate { get; set; }

        public int PeakHourlyRate { get; set; }
        public int PeakEscalatingRate { get; set; }
        public int PeakHourEscalatingRateBegins { get; set; }
        public int PeakDailyMaxOrAllDayRate { get; set; }
        public int PeakEveningFlatRate { get; set; }


        public string EscalatingStartTime { get; set; }
        public string EscalatingEndTime { get; set; }
        public int EscalatingTotalHours { get; set; }
        public string DailyHourlyPercentValuesJson { get; set; }


        public int EquipmentId { get; set; }
        public int UnitsOwned { get; set; }
        public int UnitsPurchased { get; set; }
        public int CostOfBaseUnit { get; set; }
        public int WarrantyStartingYear { get; set; }
        public decimal MonthlyMeterSoftwareFees { get; set; }
        public decimal MonthlyCreditCardProcessingFees { get; set; }
        public int EstimatedCreditCardTransaction { get; set; }
        public int EquipmentTypeId { get; set; }


        //public DefaultModel()
        //{
        //    NonPeakSeasonHourlyRate = 1;
        //    PeakSeasonHourlyRate = 2;

        //    NumberOfSpacesPerZone = 100;
        //    PercentOfSpaceOccupied = 50;
        //    NumberOfSpacesRemaining = 50;

        //    CompliancePercentage = 50;

        //    NonPeakOccupancyPercentage = 50;
        //    PeakOccupancyPercentage = 60;


        //    HourlyStartTime = "08:00 AM";
        //    HourlyEndTime = "06:00 PM";
        //    HourlyTotalHours = 10;

        //    DaysPerYear = 365;
        //    PeakDays = 65;
        //    OffDays = 0;
        //    NonPeakDays = 300;

        //    AnnualCost = 100;
        //    QuantitySold = 0;

        //    TimeOfDayStartTime = "08:00 AM";
        //    TimeOfDayEndTime = "06:00 PM";
        //    TimeOfDayTotalHours = 10;

        //    NonPeakHourlyRate = 1;
        //    NonPeakEscalatingRate = 1;
        //    NonPeakHourEscalatingRateBegins = 1;
        //    NonPeakDailyMaxOrAllDayRate = 1;
        //    NonPeakEveningFlatRate = 1;

        //    PeakHourlyRate = 2;
        //    PeakEscalatingRate = 2;
        //    PeakHourEscalatingRateBegins = 2;
        //    PeakDailyMaxOrAllDayRate = 2;
        //    PeakEveningFlatRate = 2;

        //    EscalatingStartTime = "08:00 AM";
        //    EscalatingEndTime = "06:00 PM";
        //    EscalatingTotalHours = 10;


        //    EquipmentId = 0;
        //    UnitsOwned = 0;
        //    UnitsPurchased = 0;
        //    CostOfBaseUnit = 0;
        //    WarrantyStartingYear = 0;
        //    MonthlyMeterSoftwareFees = 0;
        //    MonthlyCreditCardProcessingFees = 0;
        //    EstimatedCreditCardTransaction = 0;
        //    EquipmentTypeId = 1;
        //}
        public DefaultModel()
        {
            NonPeakSeasonHourlyRate = 1;
            PeakSeasonHourlyRate = 0;

            NumberOfSpacesPerZone = 100;
            PercentOfSpaceOccupied = 50;
            NumberOfSpacesRemaining = 50;

            CompliancePercentage = 50;

            NonPeakOccupancyPercentage = 50;
            PeakOccupancyPercentage = 60;


            HourlyStartTime = "08:00 AM";
            HourlyEndTime = "06:00 PM";
            HourlyTotalHours = 10;

            DaysPerYear = 365;
            PeakDays = 0;
            OffDays = 10;
            NonPeakDays = 355;

            AnnualCost = 0;

            QuantitySold = 0;

            TimeOfDayStartTime = "08:00 AM";
            TimeOfDayEndTime = "06:00 PM";
            TimeOfDayTotalHours = 10;

            NonPeakHourlyRate = 1;
            NonPeakEscalatingRate = 1;
            NonPeakHourEscalatingRateBegins = 1;
            NonPeakDailyMaxOrAllDayRate = 1;
            NonPeakEveningFlatRate = 1;

            PeakHourlyRate = 2;
            PeakEscalatingRate = 2;
            PeakHourEscalatingRateBegins = 2;
            PeakDailyMaxOrAllDayRate = 2;
            PeakEveningFlatRate = 2;

            EscalatingStartTime = "08:00 AM";
            EscalatingEndTime = "06:00 PM";
            EscalatingTotalHours = 10;

            List<HourlyPercentValue> hourlyPercentValues = new List<HourlyPercentValue> {
                new HourlyPercentValue(){ Hour = 1, Percent = 10},
                new HourlyPercentValue(){ Hour = 2, Percent = 10},
                new HourlyPercentValue(){ Hour = 3, Percent = 10},
                new HourlyPercentValue(){ Hour = 4, Percent = 10},
                new HourlyPercentValue(){ Hour = 5, Percent = 10},
                new HourlyPercentValue(){ Hour = 6, Percent = 10},
                new HourlyPercentValue(){ Hour = 7, Percent = 10},
                new HourlyPercentValue(){ Hour = 8, Percent = 10},
                new HourlyPercentValue(){ Hour = 9, Percent = 10},
                new HourlyPercentValue(){ Hour = 10, Percent = 10},
                new HourlyPercentValue(){ Hour = 11, Percent = 0},
                new HourlyPercentValue(){ Hour = 12, Percent = 0},
            };

            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            DailyHourlyPercentValuesJson = JsonConvert.SerializeObject(hourlyPercentValues, jsonSerializerSettings);

            EquipmentId = 0;
            UnitsOwned = 0;
            UnitsPurchased = 0;
            CostOfBaseUnit = 0;
            WarrantyStartingYear = 0;
            MonthlyMeterSoftwareFees = 0;
            MonthlyCreditCardProcessingFees = 0;
            EstimatedCreditCardTransaction = 0;
            EquipmentTypeId = 1;
        }
    }
}