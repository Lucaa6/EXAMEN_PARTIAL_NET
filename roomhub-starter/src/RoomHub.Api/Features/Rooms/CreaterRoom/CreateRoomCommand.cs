using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed record CreateRoomCommand(string? Name, int Capacity) : ICommand<Result<CreateRoomResponse>>;

public sealed record CreateRoomResponse(Guid Id, string Name, int Capacity);