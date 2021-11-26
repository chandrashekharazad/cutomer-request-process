using cutomer_request_process.Interfaces;

namespace cutomer_request_process.BusinessLogic
{
    public class MailNotify : INotifyObserver
    {
        public void Notify(string user_mail, string account_number, int balance, string _case)
        {

            EmailService.PushNotification(user_mail, account_number, balance, _case);
        }
    }
}
