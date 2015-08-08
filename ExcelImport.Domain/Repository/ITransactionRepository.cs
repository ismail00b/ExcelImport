using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelImport.Domain.Entities;

namespace ExcelImport.Domain.Repository
{
    public interface ITransactionRepository
    {
        IQueryable<Transaction> Transaction { get; }
        void SaveTransaction(IList<Transaction> lstTransactions);
        IList<Entities.Transaction> GetTransaction();
    }
}
