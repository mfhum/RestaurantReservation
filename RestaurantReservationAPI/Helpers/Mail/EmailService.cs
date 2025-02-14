using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using RestaurantReservationAPI.Helpers.Mail;
using RestaurantReservationAPI.Models;

public class EmailService : IEmailService
{
  private readonly SmtpSettings _smtpSettings = new();

  public async Task SendReservationConfirmationAsync(string toEmail, string reservationDetails)
  {
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Restaurant Reservations", _smtpSettings.Username));
    message.To.Add(new MailboxAddress("", toEmail));
    message.Subject = "Reservation Confirmation";

    message.Body = new TextPart("plain")
    {
      Text = $"""
              Hallo,

              Wir bestätigen Ihre Reservierung mit folgenden Details:
              {reservationDetails}

              Wir freuen uns darauf, Sie begrüssen zu dürfen!

              Mit freundlichen Grüssen,
              Ihr Restaurant-Team
              """
    };

    using var smtp = new SmtpClient();
    try
    {
      await smtp.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port);
      await smtp.AuthenticateAsync(_smtpSettings.Username, _smtpSettings.Password);
      await smtp.SendAsync(message);
      await smtp.DisconnectAsync(true);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Error sending email: {ex.Message}");
    }
  }
}