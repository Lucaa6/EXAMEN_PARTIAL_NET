using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Features.Common;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomRequest(string? Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void MapCreateRoomEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", async (
            CreateRoomRequest request,
            ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>> handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(
                new CreateRoomCommand(request.Name, request.Capacity),
                cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/rooms/{result.Value!.Id}", result.Value)
                : result.Error!.ToProblemDetails();
        }).WithTags("Rooms");
    }
}