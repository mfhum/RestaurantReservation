namespace RestaurantReservationAPI.Middleware;

public static class ErrorHandlingExtensions
{
  public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
  {
    return builder.UseMiddleware<ErrorHandling>();
  }
}