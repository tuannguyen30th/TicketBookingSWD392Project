using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWD.TicketBooking.Repo.Entities
{
    [Table("Transaction")]
    public class Transaction
    {
        public Guid TransactionID { get; set; }
        public Guid? UserID { get; set; }
        [ForeignKey("UserID")]
        public User? User { get; set; }
        public double? Amount { get; set; }
        public DateTime? TransactionDate { get; set; }
        public double? BalanceAfterTransaction { get; set; }
        public string? TransactionType { get; set; }
    }
}
