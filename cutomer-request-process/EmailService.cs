using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using System.Text;

namespace cutomer_request_process
{
    class EmailService
    {
        static bool mailSent = false;
        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("[{0}] Send canceled.", token);
            }
            if (e.Error != null)
            {
                Console.WriteLine("[{0}] {1}", token, e.Error.ToString());
            }
            else
            {
                Console.WriteLine("Message sent.");
            }
            mailSent = true;
        }


        public static void PushNotification()
        {
            // Command-line argument must be the SMTP host.
            System.Net.Mail.SmtpClient client = new SmtpClient("smtp.gmail.com",465);
            // Specify the email sender.
            // Create a mailing address that includes a UTF8 character
            // in the display name.
            MailAddress from = new MailAddress("azadbsr3095@gmail.com",
               "Chandra" + (char)0xD8 + "Shekhar",
            System.Text.Encoding.UTF8);
            // Set destinations for the email message.
            MailAddress to = new MailAddress("azadbsr3095@gmail.com");
            // Specify the message content.
            MailMessage message = new MailMessage(from, to);
            message.Body = "This is a test email message sent by an application. ";
            // Include some non-ASCII characters in body and subject.
            string someArrows = new string(new char[] { '\u2190', '\u2191', '\u2192', '\u2193' });
            message.Body += Environment.NewLine + someArrows;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = "test message 1" + someArrows;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
            // Set the method that is called back when the send operation ends.
            client.SendCompleted += new
            SendCompletedEventHandler(SendCompletedCallback);
            // The userState can be any object that allows your callback
            // method to identify this send operation.
            // For this example, the userToken is a string constant.
            string userState = "test message1";
            client.SendAsync(message, userState);
            // Clean up.
            message.Dispose();
            Console.WriteLine("Goodbye.");
        }
    }
}
