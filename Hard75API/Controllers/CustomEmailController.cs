using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Hard75API.Models;
using Serilog;

namespace Hard75API.Controllers
{
    [ApiController]

    [Route("api")]
    public class CustomEmailController
    {
        [HttpPost]
        [Route("SendEmail")]
        public bool sendEmail([FromBody] CustomEmail ce)
        {
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("logs/mylog.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            try
            {
                Log.Information("CustomEmail:Creating Email");
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new System.Net.Mail.MailAddress("anthuan.e.w@gmail.com");
                message.To.Add(new System.Net.Mail.MailAddress(ce.emailAddress));
                message.Subject = ce.subject;
                message.IsBodyHtml = true; //to make message body as html
                message.Body = ce.body;
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(message.From.Address, "ybyxeuzppzkwsykd");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                Log.Information("CustomEmail:Email Sent to "+ ce.emailAddress);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Log.Error("CustomEmail:"+ e.ToString());
                return false;
            }
        }
    }
}
