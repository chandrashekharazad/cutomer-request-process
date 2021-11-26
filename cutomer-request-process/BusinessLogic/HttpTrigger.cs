using cutomer_request_process.BusinessLogic;
using cutomer_request_process.DataAccessLayer;
using cutomer_request_process.Interfaces;
using cutomer_request_process.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace cutomer_request_process
{
    public class HttpTrigger : IHttpTrigger, INotifyObserver
    {
        private readonly CustomerRequestContext _context;

        private readonly INotifyObserver _mailNotify;

        public HttpTrigger(CustomerRequestContext context, INotifyObserver mailNotify)
        {
            _context = context;
            _mailNotify = mailNotify;
        }

        public void Notify(string user_mail, string account_number, int balance, string _case)
        {
            Notifier O = new Notifier();
            O.AddService(_mailNotify);
            O.ExecuteNotifier(user_mail, account_number, balance, _case);
        }

        public async void CustomerRequest(Guid _userid, string _request_remark)
        {
            var customer_request = await _context.t_customer_requests.FirstOrDefaultAsync(i => i.userid == _userid);
            customer_request.request_remarks = _request_remark;
            customer_request.request_status = "Completed";
            _context.SaveChanges();
        }

        [FunctionName("HttpTrigger")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req, CancellationToken cts,
            ILogger log)
        {
            string responseMessage = "";

            string _case = "";

            log.LogInformation("C# HTTP trigger function processed a request.");

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Account data = JsonConvert.DeserializeObject<Account>(requestBody);

            if (await _context.t_account.AnyAsync(o => o.userid == data.userid && o.account_status == "Active"))
            {
                responseMessage = "The user already has an active account";

                CustomerRequest(data.userid, responseMessage);

                _case = "AlreadyActive";
                Account _account_details = await _context.t_account.FirstOrDefaultAsync(o => o.userid == data.userid);
                UserDetails userDetails = await _context.t_userdata.FirstOrDefaultAsync(o => o.userid == data.userid);
                Notify(userDetails.email_id, _account_details.account_number, _account_details.balance, _case);

                return new OkObjectResult(responseMessage);
            }
            else if (await _context.t_account.AnyAsync(o => o.userid == data.userid && o.account_status != "Active"))
            {
                Account _account_details = await _context.t_account.FirstOrDefaultAsync(o => o.userid == data.userid);
                responseMessage = "User already had an account in " + _account_details.account_status + " state and it is now activated";
                _account_details.account_status = "Active";
                _context.SaveChanges();

                _case = "OldAccount";
                UserDetails userDetails = await _context.t_userdata.FirstOrDefaultAsync(o => o.userid == data.userid);
                Notify(userDetails.email_id, _account_details.account_number, _account_details.balance, _case);
                CustomerRequest(data.userid, responseMessage);

                return new OkObjectResult(responseMessage);
            }

            UserDetails _userDetails = await _context.t_userdata.FirstOrDefaultAsync(o => o.userid == data.userid);
            var _account_type = 0;

            if (_userDetails.user_type == "Student")
            {
                _account_type = 1;
            }

            Account new_account = new Account {
                account_id = new Guid(),
                account_number = DateTimeOffset.Now.ToUnixTimeSeconds().ToString(),
                account_type = _account_type,
                balance = 0,
                account_status = "Active",
                userid = data.userid
            };

            string new_account_number = new_account.account_id.ToString();
            int new_balance = new_account.balance;

            var entity = await _context.t_account.AddAsync(new_account, cts);
            await _context.SaveChangesAsync(cts);

            _case = "new";

            Notify(_userDetails.email_id, new_account_number, new_balance, _case);
            responseMessage = "The account is Activated";

            CustomerRequest(data.userid, responseMessage);

            return new OkObjectResult(responseMessage);
        }
    }
}
