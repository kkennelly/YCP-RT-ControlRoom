using ControlRoomApplication.Entities;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using Amazon;
using ControlRoomApplication.Database;
using System.Threading.Tasks;

namespace ControlRoomApplication.Controllers.Communications
{
    public class EmailNotifications
    {
        public static Task<bool> sendToAllAdmins(string subject, string body, string sender = "system@ycpradiotelescope.com", bool testflag = false)
        {
            bool success = false;

            Thread t = new Thread(() =>
            {
        
               List<User> admins = new List<User>();
               if (testflag)
               {
                    // If you want physical proof that this tests, change the below fields to your own credentials
                    // I've already proven it works, so I took mine out. I don't like getting constant test emails.
                    // Or I guess you could create an account for good ol' Test User, but that's unnecessary in my opinion.
                    admins.Add(new User("Test", "User", "testradiotelescopeuser@ycp.edu", NotificationTypeEnum.ALL));
               }
               else
               {
                   admins = DatabaseOperations.GetAllAdminUsers();
               }

               foreach (User u in admins)
               {
                   if (u._Notification_Type == NotificationTypeEnum.ALL || u._Notification_Type == NotificationTypeEnum.EMAIL)
                   {
                       try
                       {
                            // All-admin notifications will always be sent from SYSTEM by default.
                            EmailNotifications.sendEmail(u, subject, body, sender);
                           success = true;
                       }
                       catch (Exception e)
                       {
                           Console.WriteLine($"ERROR: Email could not send: {e}");
                           success = false;
                       }
                   }
               }
            });

            t.Priority = ThreadPriority.Lowest;
            t.Start();

            // If it is a unit test, we want to wait for the thread to finish running before returning a value
            if (testflag) t.Join();

            return Task.FromResult(success);
        }

         /*
        public static Task<bool> sendToUser(User u, string subject, string body, string sender, string AttachmentPath = null, bool testflag = false)
        {
            bool success = false;

            Thread t = new Thread(() =>
            {
                try
                {
                    EmailNotifications.sendEmail(u, subject, body, sender, AttachmentPath);
                    success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR: Email could not send: {e}");
                    success = false;
                }
            });

            t.Priority = ThreadPriority.Lowest;
            t.Start();

            // If it is a unit test, we want to wait for the thread to finish running before returning a value
            if (testflag) t.Join();

            return Task.FromResult(success);
        }
        // */

        // Case for when we want to add multiple attachments to an email
        public static Task<bool> sendToUser(User u, string subject, string body, string sender, List<string> AttachmentPath = null, bool testflag = false)
        {
            bool success = false;

            Thread t = new Thread(() =>
            {
                try
                {
                    EmailNotifications.sendEmail(u, subject, body, sender, AttachmentPath);
                    success = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"ERROR: Email could not send: {e}");
                    success = false;
                }
            });

            t.Priority = ThreadPriority.Lowest;
            t.Start();

            // If it is a unit test, we want to wait for the thread to finish running before returning a value
            t.Join();

            return Task.FromResult(success);
        }

        private static void sendEmail(User user, string subject, string body, string sender, List<string> AttachPath = null)
        {
            using (var client = new AmazonSimpleEmailServiceClient(RegionEndpoint.USEast2))
            {
                if (AttachPath == null)
                {
                    var sendRequest = new SendEmailRequest
                    {
                        Source = sender,
                        Destination = new Destination
                        {
                            ToAddresses = new List<string> { user.email_address }
                        },
                        Message = new Message
                        {
                            Subject = new Content(subject),
                            Body = new Body
                            {
                                Html = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = generateHtml(subject, body)
                                },
                                Text = new Content
                                {
                                    Charset = "UTF-8",
                                    Data = body
                                }
                            }
                        }
                    };

                    try
                    {
                        Console.WriteLine("Sending email using Amazon SES...");
                        var response = client.SendEmail(sendRequest);
                        Console.WriteLine("The email was sent successfully.");
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The email was not sent.");
                        Console.WriteLine($"Error: {e}");
                    }
                }
                else
                {
                    SendRawEmailRequest request = new SendRawEmailRequest();
                    request.RawMessage = new RawMessage();

                    // message is an instance of a System.Net.Mail.MailMessage
                    MailMessage message = new MailMessage(
                        sender,
                        user.email_address,
                        subject,
                        body);

                    try
                    {
                        for (int i = 0; i < AttachPath.Count; i++)
                        {
                            Attachment data = new Attachment(AttachPath[i], MediaTypeNames.Application.Octet);
                            ContentDisposition disposition = data.ContentDisposition;
                            disposition.CreationDate = System.IO.File.GetCreationTime(AttachPath[i]);
                            disposition.ModificationDate = System.IO.File.GetLastWriteTime(AttachPath[i]);
                            disposition.ReadDate = System.IO.File.GetLastAccessTime(AttachPath[i]);

                            message.Attachments.Add(data);
                        }

                        request.RawMessage.Data = SendAttachmentHelper.ConvertMailMessageToMemoryStream(message);

                        try
                        {
                            Console.WriteLine("Sending email using Amazon SES...");
                            var response = client.SendRawEmail(request);
                            Console.WriteLine("The email was sent successfully.");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("The email was not sent.");
                            Console.WriteLine($"Error: {e}");
                        }
                    }
                    finally
                    {
                        message.Attachments.Dispose();
                    }
                }
            }
        }

        private static string generateHtml(string subject, string body)
        {
            return $@"<html>
<head></head>
<body>
    <h1>{subject}</h1>
    <p>{body}</p>
</body>
</html>";
        }
    }
}
