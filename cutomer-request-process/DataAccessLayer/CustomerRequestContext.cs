using cutomer_request_process.Models;
using Microsoft.EntityFrameworkCore;

namespace cutomer_request_process.DataAccessLayer
{
    public class CustomerRequestContext : DbContext
    {
        public CustomerRequestContext(DbContextOptions<CustomerRequestContext> options)
           : base(options)
        { }
        public DbSet<Account> t_account { get; set; }
        public DbSet<UserDetails> t_userdata { get; set; }
        public DbSet<CustomerRequest> t_customer_requests { get; set; }

    }
}
