using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Features.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public static class GetBookingEndpoint
{
    public static void MapGetBookingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetBookingQuery, Result<GetBookingResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetBookingQuery(id), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.Error!.ToProblemDetails();
        }).WithTags("Bookings");
    }
}