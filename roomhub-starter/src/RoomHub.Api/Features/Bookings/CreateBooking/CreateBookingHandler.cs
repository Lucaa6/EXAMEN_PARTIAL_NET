using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Features.Bookings.CreateBooking;

public sealed class CreateBookingHandler(
    IRoomRepository roomRepository,
    IBookingRepository bookingRepository,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<CreateBookingResponse>>
{
    public async Task<Result<CreateBookingResponse>> HandleAsync(
        CreateBookingCommand command,
        CancellationToken cancellationToken = default)
    {
        var room = await roomRepository.GetByIdAsync(command.RoomId, cancellationToken);
        if (room is null)
            return Result<CreateBookingResponse>.Failure(
                Error.NotFound("Room.NotFound", $"Room '{command.RoomId}' was not found."));

        var bookingResult = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            timeProvider);

        if (!bookingResult.IsSuccess)
            return Result<CreateBookingResponse>.Failure(bookingResult.Error!);

        var booking = bookingResult.Value!;

        var activeBookings = await bookingRepository.GetActiveForRoomAsync(room.Id, cancellationToken);
        if (activeBookings.Any(existing => existing.Overlaps(booking.StartUtc, booking.EndUtc)))
            return Result<CreateBookingResponse>.Failure(
                Error.Conflict("Booking.Overlap", "The room is already booked for the selected time interval."));

        bookingRepository.Add(booking);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<CreateBookingResponse>.Success(
            new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status));
    }
}