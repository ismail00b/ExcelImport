using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelImport.Domain.Repository;

namespace ExcelImport.Domain.Concrete
{
    public class TransactionRepository:ITransactionRepository
    {
        #region declarations
        EFDBContext context = new EFDBContext();
        public IQueryable<Entities.Transaction> Transaction
        {
            get { return context.Transaction; }
        }
        #endregion

        #region Save Transaction list
        //Function to save the data to the Transaction table
        public void SaveTransaction(IList<Entities.Transaction> lstTransactions)
        {
            try
            {
                context.Transaction.AddRange(lstTransactions);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Get the Transactions 
        //Function to get the data from Transaction table
        public IList<Entities.Transaction> GetTransaction()
        {
            try
            {
                IList<Entities.Transaction> lstTransactions =context.Transaction.ToList<Entities.Transaction>();
                return lstTransactions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

    }
}
