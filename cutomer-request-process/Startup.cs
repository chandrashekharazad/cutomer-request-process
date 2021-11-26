using cutomer_request_process.BusinessLogic;
using cutomer_request_process.DataAccessLayer;
using cutomer_request_process.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(cutomer_request_process.Startup))]

namespace cutomer_request_process
{
    class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            builder.Services.AddDbContext<CustomerRequestContext>(
                options => SqlServerDbContextOptionsExtensions.UseSqlServer(options, connectionString));

            builder.Services.AddScoped<INotifyObserver, MailNotify>();
        }
    }
}
