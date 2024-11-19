using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System.Net;
using System.Net.Mail;

public class EmailService {
    public void SendEmail(string subject, string text)
    {
        var email = new MimeMessage();
        email.From.Add(MailboxAddress.Parse("abe.frami54@ethereal.email"));
        email.To.Add(MailboxAddress.Parse("abe.frami54@ethereal.email"));
        //email.Subject = "Test Email";
        //email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = "My lab 15 ASP!" };
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = text };

        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
        smtp.Authenticate("abe.frami54@ethereal.email", "Fp1jEb7kvCCAArWXB9");
        smtp.Send(email);
        smtp.Disconnect(true);
    }
}
