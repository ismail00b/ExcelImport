using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExcelImport.Domain.Entities;
using System.Data.Entity;

namespace ExcelImport.Domain.Concrete
{
    public class EFDBContext : DbContext
    {
        public DbSet<Transaction> Transaction { get; set; }
    }
}
