using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ExcelImport.Domain.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
            [Key]
            public int TransactionID { get; set; }
            [Column(TypeName="ntext")]
            [Required(ErrorMessage = "Account is Required")]
            public string Account { get; set; }
            [Column(TypeName = "ntext")]
            [Required(ErrorMessage = "Description is Required")]
            public string Description { get; set; }
            [Required(ErrorMessage = "Currency code is Required")]
            public string CurrencyCode { get; set; }
            [Required(ErrorMessage = "Amount  is Required")]
            public decimal Amount { get; set; }
    }
}
