using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static FinancialModeling.Enums.FinancialModelingEnums;

namespace FinancialModeling.Models
{
    public class EquipmentCostModel
    {
        //public int EquipmentId { get; set; }

        //public int UnitsOwned { get; set; }
        //public int UnitsPurchased { get; set; }
        //public int CostOfBaseUnit { get; set; }
        //public int WarrantyStartingYear { get; set; }
        //public int MonthlyMeterSoftwareFees { get; set; }
        //public int MonthlyCreditCardProcessingFees { get; set; }
        //public int EstimatedCreditCardTransaction { get; set; }

        //public int EquipmentTypeId { get; set; }

        //public EquipmentType SelectedEquipmentType { get; set; }

        public int EquipmentId { get; set; }
        public int UnitsOwned { get; set; }
        public int UnitsPurchased { get; set; }


        //Start : OnStreet And OffStreet Fields
        public int CostOfBaseUnit { get; set; }
        public decimal MonthlyMeterSoftwareFees { get; set; }
        public int WarrantyStartingYear { get; set; }
        //End : OnStreet And OffStreet Fields

        //Start : Garages Fields
        public int QuantityOfUnits { get; set; }
        public int MultiSpaceMeterCost { get; set; }
        public int EquipWithBNA { get; set; }
        public int EquipWithCreditCard { get; set; }
        public int AnnualSoftwareFee { get; set; }
        public bool IsWarrantyIncluded { get; set; }
        //End : Garages Fields

        public decimal MonthlyCreditCardProcessingFees { get; set; }
        public int EstimatedCreditCardTransaction { get; set; }

        public int EquipmentTypeId { get; set; }
        public int ClientId { get; set; }
        public int ZoneCode { get; set; }
        public int ParkingTypeId { get; set; }//References LuParkingType

        public EquipmentType SelectedEquipmentType { get; set; } //References LuEquipmentType
        public ActionType ActionType { get; set; }
    }
}