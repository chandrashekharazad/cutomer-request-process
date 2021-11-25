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
    public  class HttpTrigger : IHttpTrigger, INotifyObserver
    {

        private readonly CustomerRequestContext _context;

        private readonly INotifyObserver _mailNotify;


        public HttpTrigger(CustomerRequestContext context, INotifyObserver mailNotify)
        {
            _context = context;
            _mailNotify = mailNotify;

        }

        public void Notify(string user_mail)
        {
            Notifier O = new Notifier();
            O.AddService(_mailNotify);
            O.ExecuteNotifier(user_mail);
        }

        [FunctionName("HttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, CancellationToken cts,
            ILogger log)
        {

            string responseMessage = "";

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerRequestEf data = JsonConvert.DeserializeObject<CustomerRequestEf>(requestBody);

            if (await _context.t_account.AnyAsync(o => o.userid == data.userid && o.account_status == "Active"))
            {
                responseMessage = "The user already has an active account";
                return new OkObjectResult(responseMessage);
            }
            else if(await _context.t_account.AnyAsync(o => o.userid == data.userid && o.account_status != "Active"))
            {
                CustomerRequestEf _account_details = await _context.t_account.FirstOrDefaultAsync(o => o.userid == data.userid);
                responseMessage = "User already had an account in " + _account_details.account_status+" state and it is now activated";
                _account_details.account_status = "Active";
                _context.SaveChanges();
                return new OkObjectResult(responseMessage);
            }

            UserDetails _userDetails = await _context.t_userdata.FirstOrDefaultAsync(o => o.userid == data.userid);
            var _account_type = 0;

            if (_userDetails.user_type == "Student")
            {
                _account_type = 1;
            }

            CustomerRequestEf p = new CustomerRequestEf {
                account_number = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                account_type = _account_type,
                balance = 0,
                account_status = "Active",
                userid = data.userid
            };

            var entity = await _context.t_account.AddAsync(p, cts);
            await _context.SaveChangesAsync(cts);


            Notify(_userDetails.email_id);
            //INotifyObserver obj1 = new MailNotify();
            //Notifier O = new Notifier();
            //O.AddService(obj1);
            //O.ExecuteNotifier(_userDetails.email_id);

            responseMessage = "The account is Activated";

            //return new OkObjectResult(JsonConvert.SerializeObject(entity.Entity));

            return new OkObjectResult(responseMessage);

        }
    }
}
