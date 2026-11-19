using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Features.Common;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed record CreateBookingRequest(
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees);

public static class CreateBookingEndpoint
{
    public static void MapCreateBookingEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (
            Guid roomId,
            CreateBookingRequest request,
            ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CreateBookingCommand(roomId, request.BookedBy, request.StartUtc, request.EndUtc, request.Attendees),
                cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/bookings/{result.Value!.Id}", result.Value)
                : result.Error!.ToProblemDetails();
        }).WithTags("Bookings");
    }
}