using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Enums
{
    public class FinancialModelingEnums
    {
        public enum ModelTypeEnum
        {
            Hourly = 1,
            TimeOfDay = 2,
            Escalating = 3
        }

        public enum ParkingTypeEnum
        {
            OnStreet = 1,
            OffStreet = 2,
            Garages = 3
        }

        public enum OperatingHourTypeEnum
        {
            Daily = 1,
            Evening = 2,
        }

        public enum ActionType
        {
            Created = 1,
            Modified = 2,
            Deleted = 3
        }
    }
}