using ExcelImport.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.IO;

namespace ExcelImport.Web.UI.Utility
{
    public class UploadHelper
    {
        #region Declarations
        private ITransactionRepository repository;
        #endregion

        #region Construtors
        public UploadHelper(ITransactionRepository repository)
        {
            this.repository = repository;
        }
        #endregion

        #region Read Excel

        ///This function is used to read and validate the Excel File
        public DataSet ReadExcelFile(string filePath)
        {
            try
            {
                DataSet dsTransaction = GetDataFromSheet(filePath);
                if (dsTransaction.Tables[0].Rows.Count > 0)
                {
                    ValidateExcelData(dsTransaction.Tables[0]);
                }
                return dsTransaction;
            }
            catch (Exception ex)
            {
                //Code to be written to log the exception details in a file
                throw new ApplicationException("Error Occured in ReadExcelFile method");
            }
        }
        #endregion

        #region Capture Excel Data

        /// This function is used for reading data From sheet1 and adding those worksheet data into dataset 
        private DataSet GetDataFromSheet(string filePath)
        {
            OleDbConnection oledbConn;
            try
            {
                string oledbConnectionString = ConfigurationManager.AppSettings["oledbConn"].ToString();
                oledbConnectionString = oledbConnectionString.Replace("path", filePath);
                oledbConn = new OleDbConnection(oledbConnectionString);
                oledbConn.Open();

                OleDbCommand cmd = new OleDbCommand(); ;
                OleDbDataAdapter oleda = new OleDbDataAdapter();
                DataSet ds = new DataSet();
                cmd.Connection = oledbConn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = cmd.CommandText = "SELECT [" + Constants.Account + "],[" + Constants.Description + "],[" + Constants.Currency + "],[" + Constants.Amount + "] FROM [Sheet1$]";
                oleda = new OleDbDataAdapter(cmd);
                oleda.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //Code to be written to log the exception details in a file
                throw new ApplicationException("Error occured in GetDataFromSheet method");
            }
        }
        #endregion

        #region Validate Excel Data

        ///This function  validates the data in the excel
        private void ValidateExcelData(DataTable dtUploadedExcel)
        {
            string error = string.Empty;
            decimal number = 0;
            try
            {
                //add an extra column named Error inside the DataTable which contains the excelUploadedData
                dtUploadedExcel.Columns.Add("Error");
                for (int row = 0; row < dtUploadedExcel.Rows.Count; row++)
                {
                    error = string.Empty;

                    for (int col = 0; col < dtUploadedExcel.Columns.Count - 1; col++)
                    {
                        //Validate Mandetory Fields/Columns
                        if (dtUploadedExcel.Rows[row][col].ToString() == null || dtUploadedExcel.Rows[row][col].ToString() == "")
                        {
                            error += dtUploadedExcel.Columns[col].ToString() + " Should not be Null in line " + (row + 1) + ";";
                        }

                        //Adding custom validations 
                        if (dtUploadedExcel.Columns[col].ColumnName == Constants.Amount)
                        {
                            string amount = dtUploadedExcel.Rows[row][col].ToString();
                            if (amount != null && amount != "")
                            {
                                if (!decimal.TryParse(amount, out number))
                                    error += "Amount value should be number in row: " + (row + 1) + ";";
                            }
                        }

                        if (dtUploadedExcel.Columns[col].ColumnName == Constants.Currency)
                        {
                            string currencyCode = dtUploadedExcel.Rows[row][col].ToString();
                            if (currencyCode != null && currencyCode != "")
                            {
                                if (!ValidateCurrencyCode(currencyCode))
                                    error += "Currency code should be valid in row: " + (row + 1) + ";";
                            }
                        }

                        //adding the error details to the Error coulmn of the DataTable which can be used for displaying the error details on the Page
                        dtUploadedExcel.Rows[row]["Error"] = error;
                    }
                }
            }
            catch (Exception ex)
            {
                //Code to be written to log the exception details in a file;
                throw new ApplicationException("Error occured in ValidateExcelData method");
            }
        }
        #endregion

        #region ValidateCurrency

        //This function validates currency code and returns boolean
        public bool ValidateCurrencyCode(string currencyCode)
        {
            try
            {
                System.Globalization.RegionInfo regionInfo = (from culture in System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.InstalledWin32Cultures)
                                                              where culture.Name.Length > 0 && !culture.IsNeutralCulture
                                                              let region = new System.Globalization.RegionInfo(culture.LCID)
                                                              where String.Equals(region.ISOCurrencySymbol, currencyCode, StringComparison.InvariantCultureIgnoreCase)
                                                              select region).FirstOrDefault();

                return (regionInfo == null ? false : true);
            }
            catch (Exception ex)
            {
                //Code to be written to log the exception details in a file;
                throw new ApplicationException("Error occured in ValidateCurrencyCode method");
            }

        }
        #endregion
    }
}