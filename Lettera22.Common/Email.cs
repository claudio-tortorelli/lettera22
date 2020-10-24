using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Mail;
using OpenPop.Mime;
using OpenPop.Pop3;
using Lettera22.Common;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Net;
using AegisImplicitMail;
using System.IO;

namespace Lettera22.Common
{
    public class Email
    {
        /// <project>Lettera22</project>
        /// <copyright company="Claudio Tortorelli">
        /// Copyright (c) 2018 All Rights Reserved
        /// </copyright>        
        /// <author>Claudio Tortorelli</author>
        /// <email>claudio.tortorelli@gmail.com</email>
        /// <web>http://www.claudiotortorelli.it</web>
        /// <date>Nov 2018</date>
        /// <summary>
        /// 
        /// https://msdn.microsoft.com/it-it/library/system.net.mail.smtpclient(v=vs.110).aspx
        /// </summary>
        /// https://choosealicense.com/licenses/mit/
                
        public static void Send(string toAddress, string fromAddress, string subject, string bodyText, string attachmentPath = "")
        {
            Globals.m_Logger.Info(toAddress);
            Globals.m_Logger.Info(fromAddress);
            Globals.m_Logger.Info(subject);
            Globals.m_Logger.Info(bodyText);

            //Generate Message 
            var mailMessage = new MimeMailMessage();
            mailMessage.From = new MimeMailAddress(fromAddress);
            mailMessage.To.Add(toAddress);
            mailMessage.Subject = subject;
            mailMessage.Body = bodyText;
            if (attachmentPath.Length > 0 && File.Exists(attachmentPath))
            {
                MimeAttachment attachMessage = new MimeAttachment(attachmentPath);
                mailMessage.Attachments.Add(attachMessage);
            }

            //Create Smtp Client
            var mailer = new MimeMailer(Globals.SMTPHost(), Globals.SMTPPort());
            mailer.User = Globals.SMTPUsername();
            mailer.Password = Globals.SMTPPassword();
            mailer.SslType = SslMode.Ssl;
            mailer.AuthenticationMode = AuthenticationType.Base64;
            
            //Set a delegate function for call back
            mailer.SendCompleted += compEvent;
            mailer.SendMail(mailMessage);
        }

        //Call back function
        private static void compEvent(object sender, AsyncCompletedEventArgs e)
        {
            Globals.m_Logger.Info("sending email...");
            if (e.UserState != null)
                Globals.m_Logger.Info(e.UserState.ToString());

            if (e.Error != null)
                Globals.m_Logger.Error("Error : " + e.Error.Message);
        }
                        
        

        /**
         * http://hpop.sourceforge.net/examples.php
         */
        public static List<Message> Receive(string hostname, int port, bool useSsl, string username, string password, bool deleteMessages)
        {
            // The client disconnects from the server when being disposed
            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                    if (deleteMessages)
                        client.DeleteMessage(i);
                }
                

                // Now return the fetched messages
                return allMessages;
            }
        }

    }

   
}
