namespace RestaurantReservationAPI.Models;

public class SmtpSettings
{
  public string Host { get; set; } = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "";
  public int Port { get; set; } = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var port) ? port : 587;
  public string Username { get; set; } = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "";
  public string Password { get; set; } = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "";
  public bool EnableSsl { get; set; } = bool.TryParse(Environment.GetEnvironmentVariable("SMTP_ENABLE_SSL"), out var enableSsl) && enableSsl;
}