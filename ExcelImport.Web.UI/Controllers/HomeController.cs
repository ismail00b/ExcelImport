using ExcelImport.Domain.Concrete;
using ExcelImport.Domain.Entities;
using ExcelImport.Domain.Repository;
using ExcelImport.Web.UI.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelImport.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        private ITransactionRepository repository;

        public HomeController()
        {
        } 

        public HomeController(ITransactionRepository repository)
        {
            this.repository = repository;
        } 

        public ActionResult Index()
        {
           return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            try
            {
                if (file != null)
                {
                    string extension = Path.GetExtension(file.FileName);
                    if (extension.Equals(".xlsx"))
                    {
                        DateTime dt = DateTime.Now;
                        string filePath = "TransactionData";
                        string format = dt.ToString();
                        format = format.Replace('/', '_');
                        format = format.Replace(':', '_');
                        format = format.Replace(' ', '_');
                        filePath += "_" + format + ".xlsx";
                        string finalFilePath = Server.MapPath("~/Uploads/" + filePath);
                        file.SaveAs(finalFilePath);

                        UploadHelper objHelper = new UploadHelper(repository);
                        IList<Transaction> lstData = new List<Transaction>();
                        DataSet dsTransactions = objHelper.ReadExcelFile(finalFilePath);
                        DataView dvTransactions = dsTransactions.Tables[0].DefaultView;
                        if (dvTransactions.ToTable().Rows.Count > 0)
                        {
                            dvTransactions.RowFilter = "Error = ''";

                            foreach (DataRow dr in dvTransactions.ToTable().Rows)
                            {
                                lstData.Add(new Transaction { Account = Convert.ToString(dr[Constants.Account]), Description = Convert.ToString(dr[Constants.Description]), CurrencyCode = Convert.ToString(dr[Constants.Currency]), Amount = Convert.ToDecimal(dr[Constants.Amount]) });
                            }

                            repository.SaveTransaction(lstData);
                            ViewBag.UploadMsg = dvTransactions.ToTable().Rows.Count + " row(s) have been saved successfully.";
                        }
                        else
                        {
                            ViewBag.UploadMsg = "No valid data to be uploaded.";
                        }

                        return View("Index", dsTransactions);
                    }
                    else
                    {
                        ViewBag.Error = "Invalid File Format " + extension + " Valid File Format allowed is .xlsx";
                        return View("Index");
                    }
                }
                else
                {
                    ViewBag.Error = "Please select an excel file to upload";
                    return View("Index");
                }
            }
            catch(Exception ex)
            {
                ViewBag.Error = "Error occurred while processing your request";
                return View("Index");
            }
            
        }

    }
}
