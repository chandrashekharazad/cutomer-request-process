using System.Net;
using System.Net.Mail;

namespace cutomer_request_process
{
    class EmailService
    {
        public static void PushNotification(string user_mail, string account_number, int balance, string _case)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("cs5721cdnproject@gmail.com");
                mail.To.Add(user_mail);
                switch (_case)
                {
                    case "new":
                        mail.Subject = "Your request for creation of account with CDN Bank Completed";
                        mail.Body = "Hi, This is to inform you that your request for creation of bank account with CDN Banking has been approved." +
                        "\n please find below the details of your bank account" +
                        "\n Account Number: " + account_number + "." +
                        "\n Account Balance:" + balance + "." +
                        "\n Thanks for Choosing CDN Banking.";
                        break;
                    case "OldAccount":
                        mail.Subject = "Your request for creation of account with CDN Bank Completed";
                        mail.Body = "Hi, This is to inform you that your request for creation of bank account with CDN Banking has been approved." +
                        "\n You had an exisitng account with CDN Banking which was in freezed or closed state which has been activated" +
                        "\n Account Number: " + account_number + "." +
                        "\n Account Balance:" + balance + "." +
                        "\n Thanks for Choosing CDN Banking.";
                        break;
                    case "AlreadyActive":
                        mail.Subject = "Your request for creation of account with CDN Bank Rejected";
                        mail.Body = "Hi, This is to inform you that your request for creation of bank account with CDN Banking has been rejected." +
                        "\n You have an exisitng account with CDN Banking which was in Active state a new account can't be created" +
                        "\n Account Number: " + account_number + "." +
                        "\n Account Balance:" + balance + "." +
                        "\n Thanks for Choosing CDN Banking.";
                        break;
                }
                mail.IsBodyHtml = true;
                //mail.Attachments.Add(new Attachment("D:\\TestFile.txt"));//--Uncomment this to send any attachment  
                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("cs5721cdnproject@gmail.com", "Zxcvbnm@123");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }
        }
    }
}
