namespace RestaurantReservationAPI.Middleware;

public class ErrorHandling(RequestDelegate next, ILogger<ErrorHandling> logger)
{
  public async Task InvokeAsync(HttpContext context)
  {
    try
    {
      await next(context);
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An unhandled exception occurred");
      await HandleExceptionAsync(context, ex);
    }
  }

  private static Task HandleExceptionAsync(HttpContext context, Exception exception)
  {
    context.Response.ContentType = "application/json";
    context.Response.StatusCode = exception switch
    {
      KeyNotFoundException => StatusCodes.Status404NotFound,
      UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
      _ => StatusCodes.Status500InternalServerError
    };

    var response = new
    {
      Message = exception.Message,
      Details = exception.InnerException?.Message,
      StatusCode = context.Response.StatusCode
    };

    return context.Response.WriteAsJsonAsync(response);
  }
}