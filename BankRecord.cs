using System;
using System.Collections.Generic;
using System.Text;

namespace QuickBooksCSVFormatter
{
    class BankRecord
    //Read each line from the CSV file as a BankRecord
    {
        public string Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string PaidOut { get; set; }
        public string PaidIn { get; set; }
        public string Balance { get; set; }

        public BankRecord(string dataRow)
        {
          //Split input data here 
          string[] data = dataRow.Split(',');

          //parse data to props
          this.Date = data[0];
          this.Type = data[1];
          this.Description = data[2];
          this.PaidOut = (data[3]);
          this.PaidIn = (data[4]);
          this.Balance = (data[5]);
        }
    }
}
