using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Text;

namespace QuickBooksCSVFormatter
{
    class QuickBooksCVSFormatConvert
    {
        public string Date { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }

        //maybe have a method to convert input from the paid-in & paid-out amounts
        public static double ConvertPaidInNOutToAmount(string valueOfAmount)
        {
            double parsedAmount;

            if (!String.IsNullOrEmpty(valueOfAmount))
            {
                parsedAmount = Double.Parse(valueOfAmount);
                return parsedAmount;
            }
            else
            {
                parsedAmount = 0;
                return parsedAmount;
            }

        }
    }
}
