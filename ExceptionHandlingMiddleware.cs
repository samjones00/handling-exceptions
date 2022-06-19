using Microsoft.AspNetCore.Diagnostics;

namespace PaymentGateway.DependencyInjection
{
    public static class ExceptionHandlingMiddleware
    {
        public static IApplicationBuilder AddExceptionHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(exceptionHandlerApp =>
            {
                exceptionHandlerApp.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var error = exceptionHandlerPathFeature?.Error;

                    if (error is PaymentNotFoundException)
                    {
                        context.Response.StatusCode = StatusCodes.Status404NotFound;

                        await context.Response.WriteAsync(error.Message);
                    }

                    if (error is PaymentAlreadyExistsException)
                    {
                        context.Response.StatusCode = StatusCodes.Status409Conflict;

                        await context.Response.WriteAsync(error.Message);
                    }
                });
            });

            return app;
        }
    }
}