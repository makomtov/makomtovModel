using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace WebApiMTModel.Models.Models.View
{
    public class SendMailService
    {
        public  void  SendMail(SendMailRequest mail)
        {
            
           
           // mail.recipient=mailRequestValues.First()
            MailMessage msg = new MailMessage();
            // Separate the recipient array
            string[] emailAddress = mail.recipient.Split(',');

            foreach (string currentEmailAddress in emailAddress)
            {
                msg.To.Add(new MailAddress(currentEmailAddress.Trim()));
            }

            // Separate the cc array , if not null
            string[] ccAddress = null;

            if (mail.cc != null)
            {
                ccAddress = mail.cc.Split(',');
                foreach (string currentCCAddress in ccAddress)
                {
                    msg.CC.Add(new MailAddress(currentCCAddress.Trim()));
                }
            }
            // Include the file attachment if the filename is not null
            if (mail.filename != null)
            {
                string[] files = mail.filename.Split(',');
                // Declare a temp file path where we can assemble our file
                foreach (string currentfiles in files)
                {
                  //  msg.CC.Add(new MailAddress(currentCCAddress.Trim()));
                    string tempPath = Properties.Settings.Default["TempFile"].ToString();

                    string filePath = Path.Combine(tempPath, currentfiles);

                    using (System.IO.FileStream reader = System.IO.File.Create(filePath))
                    {

                        byte[] buffer = Convert.FromBase64String(mail.filecontent);
                        reader.Write(buffer, 0, buffer.Length);
                        reader.Dispose();
                    }

                    msg.Attachments.Add(new Attachment(filePath));
                }
               

            }
            // Include the reply to if not null
            if (mail.replyto != null)
            {
                msg.ReplyToList.Add(new MailAddress(mail.replyto));
            }
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

           

            msg.From = new MailAddress("makomtovapp@gmail.com");
            // mail.recipient("youraddress@gmail.com");
            msg.Subject = mail.subject;
            msg.Body = mail.body;
            try
            {
                SmtpServer.UseDefaultCredentials = false; ;
                SmtpServer.Host = "smtp.gmail.com";
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("makomtovapp@gmail.com", "!Q2w3e4r");
                SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
                SmtpServer.EnableSsl = true;
                SmtpServer.SendCompleted += SmtpServer_SendCompleted;
                
                    SmtpServer.Send(msg);
                   
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    msg.Dispose();
                }
          
           
        }

       

        private static void SmtpServer_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            //Get the Original MailMessage object
            MailMessage mail = (MailMessage)e.UserState;

            //write out the subject
            
            if (e.Cancelled)
            {
                //if you use using(message) {...} here, you'll get ObjectDisposedException again
                SmtpClient smtp;
                smtp = new SmtpClient();
                
                try
                {
                    //sending message again
                    smtp.SendAsync(mail, mail);

                }
                catch (ObjectDisposedException ex)
                {
                    throw ex;
            
     }
            }

        }

       
    }
}