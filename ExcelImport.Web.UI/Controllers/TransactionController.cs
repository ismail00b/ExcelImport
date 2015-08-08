using ExcelImport.Domain.Entities;
using ExcelImport.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExcelImport.Web.UI.Controllers
{
    public class TransactionController : Controller
    {
        private ITransactionRepository repository;

        public TransactionController()
        {
        }

        public TransactionController(ITransactionRepository repository)
        {
            this.repository = repository;
        }

        public ActionResult Index()
        {
            IList<Transaction> lstTransactionData = repository.GetTransaction();
            return View(lstTransactionData);
        }

    }
}
