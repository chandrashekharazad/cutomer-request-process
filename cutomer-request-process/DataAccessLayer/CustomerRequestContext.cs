using cutomer_request_process.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace cutomer_request_process.DataAccessLayer
{
    public class CustomerRequestContext : DbContext
    {
        public CustomerRequestContext(DbContextOptions<CustomerRequestContext> options)
           : base(options)
        { }

        public DbSet<CustomerRequestEf> t_account { get; set; }

        public DbSet<UserDetails> t_userdata { get; set; }

    }
}
