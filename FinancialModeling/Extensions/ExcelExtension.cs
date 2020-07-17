using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FinancialModeling.Extensions
{
    public static class ExcelExtension
    {
        public static string ToAmountFormat(this string value)
        {
            return string.Format("${0:n}", value).ToString();
        }
        public static string ToPercentFormat(this string value)
        {
            return value.ToString() + "%";
        }

        public static void AddPlainValue(this ExcelRangeBase cell, object value, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left)
        {
            cell.Value = value.ToString();
            cell.Style.HorizontalAlignment = align;
        }

        public static void AddValueWithStyles(this ExcelRangeBase cell, object value, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left)
        {
            cell.Value = value.ToString();
            cell.Style.Font.Bold = true;
            cell.Style.HorizontalAlignment = align;
        }

        private static bool isNumeric;
        public static void AddAmountValue(this ExcelRangeBase cell, object value, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Right)
        {
            isNumeric = decimal.TryParse(value.ToString(), out decimal num);
            if (isNumeric)
            {
                cell.Value = string.Format("${0:n}", num);
                cell.Style.HorizontalAlignment = align;
            }
            else
            {
                cell.Value = value.ToString();
                cell.Style.HorizontalAlignment = align;
            }
        }

        public static void AddPercentValue(this ExcelRangeBase cell, object value, ExcelHorizontalAlignment align = ExcelHorizontalAlignment.Left)
        {
            cell.Value = value.ToString().ToPercentFormat();
            cell.Style.HorizontalAlignment = align;
        }
    }

}