using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Forms;

namespace QuickBooksCSVFormatter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string inputBrowsePath;
        private string outputBrowsePath;
        private string outputPath;

        //Test output for a single CSV
        //Note: I want to take the filename input and append a suffix like _processed to that file in an output directory
        private string[] csvLines = System.IO.File.ReadAllLines(@"F:\BUSINESS\Banking\HSBC_2019-2020_TaxYear\20190503_62389967.csv");
        private List<BankRecord> bankRecords = new List<BankRecord>();
        private List<QuickBooksCVSFormatConvert> QBFormConvert = new List<QuickBooksCVSFormatConvert>(); //Note: an abandoned class
        private List<string> listOfCSVFiles = new List<string>();
        private List<double> AmountList = new List<double>();


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Clicking on the input button will show a dialogue to set the path of the input folder
            //containing the source CSV files to parse
            //we will go through each file, parsing the content and formatting it to the correct output
            //for QuickBooks

            FolderBrowserDialog folderBrowserDialog;

            using (folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    inputBrowsePath = folderBrowserDialog.SelectedPath;
                }
                else
                {
                    return;
                }
            }
            //Bank input CSV fields are - Date, Type, Description, Paid Out, Paid In, Balance


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //Pick output folder
            FolderBrowserDialog folderBrowserDialog;

            using (folderBrowserDialog = new FolderBrowserDialog())
            {
                DialogResult result = folderBrowserDialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK &&
                    !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                {
                    outputBrowsePath = folderBrowserDialog.SelectedPath;
                }
                else
                {
                    return;
                }
            }
            //streamwrite out the CSV's foreach file on input
            //initial test of 1 CSV file
            if (!String.IsNullOrEmpty(inputBrowsePath))
            {
                foreach (var file in Directory.GetFiles(inputBrowsePath))
                {
                    listOfCSVFiles.Add(file);
                    foreach (var specificFile in listOfCSVFiles)
                    {
                        string[] csvLineByLine = System.IO.File.ReadAllLines(specificFile);
                        for (int i = 1; i < csvLineByLine.Length; i++)
                        {
                            BankRecord bankRec = new BankRecord(csvLineByLine[i]);
                            bankRecords.Add(bankRec);
                        }

                        for (int i = 0; i < bankRecords.Count; i++)
                        {
                            double paidInAmnt = QuickBooksCVSFormatConvert.ConvertPaidInNOutToAmount(bankRecords[i].PaidIn);
                            double paidOutAmnt = QuickBooksCVSFormatConvert.ConvertPaidInNOutToAmount(bankRecords[i].PaidOut) * -1;
                            double totalAmount = paidInAmnt + paidOutAmnt;
                            AmountList.Add(totalAmount);
                        }

                        //Get a substring of just the file name append it with something like processed
                        //
                        int findLastSlash = specificFile.LastIndexOf('\\');
                        int findLastDot = specificFile.LastIndexOf('.');
                        string destinationFileOutput;

                        if (findLastDot != -1 && findLastSlash != -1)
                        {
                            var fileNameOnly = specificFile.Substring(++findLastSlash, findLastDot - findLastSlash);
                            destinationFileOutput =
                                System.IO.Path.Combine(outputBrowsePath, fileNameOnly + "_processed");
                        }
                        else
                        {
                            //throw a message box and return
                            System.Windows.Forms.MessageBox.Show("Something wrong with the output browse path",
                                "Please check Output folder");
                            return;
                        }

                        using (StreamWriter streamWrite = new StreamWriter(destinationFileOutput + ".csv"))
                        {
                            streamWrite.WriteLine("Date,Description,Amount");
                            for (int i = 0; i < bankRecords.Count; i++)
                            {
                                streamWrite.WriteLine(bankRecords[i].Date + "," + bankRecords[i].Description + "," + AmountList[i]);
                            }
                        }
                        ////////////////////////////////////////////////////////////////////////////////////////////
                        //Clear lists
                        bankRecords.Clear();
                        AmountList.Clear();
                    }
                }
            }
        }
    }
}
