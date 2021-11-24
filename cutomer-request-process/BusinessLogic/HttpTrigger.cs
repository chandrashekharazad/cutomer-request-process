using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using cutomer_request_process.DataAccessLayer;
using cutomer_request_process.Interfaces;
using cutomer_request_process.BusinessLogic;
using cutomer_request_process.Models;

namespace cutomer_request_process
{
    public  class HttpTrigger : IHttpTrigger
    {

        private readonly CustomerRequestContext _context;
        public HttpTrigger(CustomerRequestContext context)
        {
            _context = context;
        }

        [FunctionName("HttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, CancellationToken cts,
            ILogger log)
        {
            string user_mail;

            string responseMessage = "";

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerRequestEf data = JsonConvert.DeserializeObject<CustomerRequestEf>(requestBody);

            if (await _context.t_account.AnyAsync(o => o.userid == data.userid))
            {
                responseMessage = "The user already has an account";
                return new OkObjectResult(responseMessage);
            }

            UserDetails _userDetails = await _context.t_userdata.FirstOrDefaultAsync(o => o.userid == data.userid);
            user_mail = _userDetails.email_id;

            CustomerRequestEf p = new CustomerRequestEf {
                account_number = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                account_type = data.account_type,
                balance = data.balance,
                account_status = data.account_status,
                userid = data.userid
            };

            var entity = await _context.t_account.AddAsync(p, cts);
            await _context.SaveChangesAsync(cts);

            INotifyObserver obj1 = new MailNotify();
            Notifier O = new Notifier();
            O.AddService(obj1);
            O.ExecuteNotifier(user_mail);

            responseMessage = "The account is Activated";

            //return new OkObjectResult(JsonConvert.SerializeObject(entity.Entity));

            return new OkObjectResult(responseMessage);

        }
    }
}
