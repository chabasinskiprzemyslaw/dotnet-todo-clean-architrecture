namespace ToDo.Application.Abstractions.Email;

public interface IEmailService
{
    Task SendAsync(ToDo.Domain.Users.Email email, string subject, string body);
}
