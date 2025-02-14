namespace RestaurantReservationAPI.Helpers.Mail;

public interface IEmailService
{
  Task SendReservationConfirmationAsync(string toEmail, string reservationDetails);
}