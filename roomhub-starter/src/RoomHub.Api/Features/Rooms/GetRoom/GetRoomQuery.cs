using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Rooms.GetRoom;

public sealed record GetRoomQuery(Guid RoomId) : IQuery<Result<GetRoomResponse>>;

public sealed record GetRoomResponse(Guid Id, string Name, int Capacity);