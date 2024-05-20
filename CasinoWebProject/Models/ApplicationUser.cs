using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace CasinoWebProject.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Add the Balance property
        public decimal Balance { get; set; }

        // Add navigation property for transactions
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}