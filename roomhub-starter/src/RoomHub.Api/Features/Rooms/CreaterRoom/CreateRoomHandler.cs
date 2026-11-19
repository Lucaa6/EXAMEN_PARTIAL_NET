using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Rooms.CreateRoom;

public sealed class CreateRoomHandler(IRoomRepository roomRepository, IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, Result<CreateRoomResponse>>
{
    public async Task<Result<CreateRoomResponse>> HandleAsync(
        CreateRoomCommand command,
        CancellationToken cancellationToken = default)
    {
        var roomResult = Room.Create(command.Name, command.Capacity);
        if (!roomResult.IsSuccess)
            return Result<CreateRoomResponse>.Failure(roomResult.Error!);

        var room = roomResult.Value!;
        roomRepository.Add(room);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateRoomResponse>.Success(new CreateRoomResponse(room.Id, room.Name, room.Capacity));
    }
}