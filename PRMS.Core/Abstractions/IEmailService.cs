namespace PRMS.Core.Abstractions;

    public interface IEmailService
    {
        Task<string> SendEmailAsync(string recipientEmail, string subject, string body);
    }

