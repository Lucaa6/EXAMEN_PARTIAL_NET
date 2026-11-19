using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed class GetRoomHandler(IRoomRepository roomRepository)
    : IQueryHandler<GetRoomQuery, Result<GetRoomResponse>>
{
    public async Task<Result<GetRoomResponse>> HandleAsync(
        GetRoomQuery query,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(query.RoomId, cancellationToken);
        if (room is null)
            return Result<GetRoomResponse>.Failure(Error.NotFound("Room.NotFound", $"Room '{query.RoomId}' was not found."));

        return Result<GetRoomResponse>.Success(new GetRoomResponse(room.Id, room.Name, room.Capacity));
    }
}