﻿using SparkPost;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace cutomer_request_process
{
    class EmailService
    {
        public static void PushNotification()
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("cs5721cdnproject@gmail.com");
                mail.To.Add("cs5721cdnproject@gmail.com");
                mail.Subject = "Hello from SparkPost!";
                mail.Body = "This is a test email.";
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
