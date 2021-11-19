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
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, CancellationToken cts,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            CustomerRequestEf data = JsonConvert.DeserializeObject<CustomerRequestEf>(requestBody);

            var _userid = data.userid;

            if (await _context.t_account.AnyAsync(o => o.userid == _userid))
            {
                string responseMessage = "The user already has an account";
                return new OkObjectResult(responseMessage);
            }

            CustomerRequestEf p = new CustomerRequestEf
            {
                account_number = data.account_number,
                account_type = data.account_type,
                balance = data.balance,
                account_status = data.account_status,
                userid = data.userid
            };
            var entity = await _context.t_account.AddAsync(p, cts);
            await _context.SaveChangesAsync(cts);

            //EmailService obj_email = new  EmailService();

            //obj_email.

            //EmailService.PushNotification();

            return new OkObjectResult(JsonConvert.SerializeObject(entity.Entity));

            //string name = req.Query["name"];

            //string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;

            //string responseMessage = string.IsNullOrEmpty(name)
            //    ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
            //    : $"Hello, {name}. This HTTP triggered function executed successfully.";

            //return new OkObjectResult(responseMessage);
        }
    }
}
