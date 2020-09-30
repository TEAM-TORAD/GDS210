using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using TMPro;


// https://www.youtube.com/watch?v=hw0XvUuzAcA
public class SendEmail : MonoBehaviour
{
    public TMP_InputField bodyMessage;
    //public TMP_InputField recipientEmail;

    public string recipientEmail;

    public void SendEmailStandard( string recipient, string subject, string message, string password)
    {
        MailMessage mail = new MailMessage();
        SmtpClient smtpServer = new SmtpClient("smtp.gmail.com");
        smtpServer.Timeout = 10000;
        smtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        smtpServer.UseDefaultCredentials = false;
        smtpServer.Port = 587; // 25 587 465 2525

        mail.From = new MailAddress("team.torad@gmail.com");
        mail.To.Add(new MailAddress(recipient));

        mail.Subject = subject;
        mail.Body = message;

        smtpServer.Credentials = new System.Net.NetworkCredential("team.torad@gmail.com", password);
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };
        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        smtpServer.Send(mail);
    }
}
