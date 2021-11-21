using cutomer_request_process.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace cutomer_request_process.BusinessLogic
{
    class MailNotify : INotifyObserver
    {
        public void Notify()
        {
            EmailService.PushNotification();
            //Console.WriteLine("Notify through Mail");
        }
    }
}
