using CourseHub.API.Services.AppInfo;
using CourseHub.Core.Interfaces.Logging;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Web;

namespace CourseHub.API.Services.Email;

public class EmailService
{
    private readonly IWebHostEnvironment _hostEnvironment;
    private readonly string _senderMail;
    private readonly string _senderPassword;
    private readonly string _senderName;

    private readonly IAppLogger _logger;
    private const short PORT = 587;



    private const string REGISTRATION_SUBJECT = "CourseHub - Registration Confirmation";
    private const string RELATIVE_REGISTRATION_TEMPLATE = @"\emailTemplates\confirmation.html";

    private const string PASSWORD_RESET_SUBJECT = "CourseHub - Password Reset";
    private const string RELATIVE_PASSWORD_RESET_TEMPLATE = @"\emailTemplates\passwordreset.html";

    private const string TEMPLATE_NOTFOUND = "Email template not found";

    public EmailService(IWebHostEnvironment hostEnvironment, IOptions<EmailOptions> emailOptions, IOptions<AppInfoOptions> appInfo, IAppLogger logger)
    {
        _hostEnvironment = hostEnvironment;
        _senderMail = emailOptions.Value.SenderMail;
        _senderPassword = emailOptions.Value.SenderPassword;
        _senderName = appInfo.Value.AppName ?? "-App-";
        _logger = logger;
    }



    public async Task SendRegistrationEmail(string toAddress, string username, string link)
    {
        string path = _hostEnvironment.WebRootPath + RELATIVE_REGISTRATION_TEMPLATE;
        if (!File.Exists(path))
        {
            _logger.Warn("Email Template Not found");
            throw new Exception(TEMPLATE_NOTFOUND);
        }

        string template = File.ReadAllText(path);
        template = template.Replace("{username}", username).Replace("{app}", _senderName).Replace("{link}", link);

        await SendEmailAsync(toAddress, REGISTRATION_SUBJECT, template);
    }

    public async Task SendPasswordResetEmail(string toAddress, string link)
    {
        string path = _hostEnvironment.WebRootPath + RELATIVE_PASSWORD_RESET_TEMPLATE;
        if (!File.Exists(path))
            throw new Exception(TEMPLATE_NOTFOUND);

        string template = File.ReadAllText(path);
        template = template.Replace("{link}", link).Replace("{app}", _senderName);

        await SendEmailAsync(toAddress, PASSWORD_RESET_SUBJECT, template);
    }



    private async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        _logger.Inform("Sending to " + toAddress);

        var mail = new MimeMessage
        {
            Sender = new MailboxAddress(_senderName, _senderMail),
            Subject = subject,
            Body = new BodyBuilder { HtmlBody = HttpUtility.HtmlDecode(body) }.ToMessageBody()
        };
        mail.From.Add(mail.Sender);
        mail.To.Add(MailboxAddress.Parse(toAddress));

        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        try
        {
            smtp.Connect("smtp.gmail.com", PORT, SecureSocketOptions.StartTls);
            smtp.Authenticate(_senderMail, _senderPassword);
            await smtp.SendAsync(mail);
        }
        catch (Exception ex)
        {
            _logger.Warn(ex.Message);
        }
        smtp.Disconnect(true);
    }
}
