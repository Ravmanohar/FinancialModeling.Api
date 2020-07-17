using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Models
{
    public class FinanclialDashboardModel
    {

        public EditClientModel EditClientModel { get; set; }

        public SetupHourlyModelDto HourlyOnStreet { get; set; }
        public SetupHourlyModelDto HourlyOffStreet { get; set; }
        public SetupHourlyModelDto HourlyGarages { get; set; }

        public SetupTimeOfDayModelDto TimeOfDayOnStreet { get; set; }
        public SetupTimeOfDayModelDto TimeOfDayOffStreet { get; set; }
        public SetupTimeOfDayModelDto TimeOfDayGarages { get; set; }

        public SetupEscalatingModelDto EscalatingOnStreet { get; set; }
        public SetupEscalatingModelDto EscalatingOffStreet { get; set; }
        public SetupEscalatingModelDto EscalatingGarages { get; set; }


        public LocationEquipmentCostModel OnStreetEquipmentCost { get; set; }
        public LocationEquipmentCostModel OffStreetEquipmentCost { get; set; }
        public LocationEquipmentCostModel GaragesEquipmentCost { get; set; }


    }


}