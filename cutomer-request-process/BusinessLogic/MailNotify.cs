using cutomer_request_process.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace cutomer_request_process.BusinessLogic
{
    class MailNotify : INotifyObserver
    {
        public void Notify(string user_mail)
        {

            EmailService.PushNotification(user_mail);
        }
    }
}
