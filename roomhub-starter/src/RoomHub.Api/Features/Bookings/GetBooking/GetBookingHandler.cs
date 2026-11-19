using RoomHub.Api.Common.CQRS;
using RoomHub.Api.Domain.Bookings;
using RoomHub.Api.Domain.Common;

namespace RoomHub.Api.Features.Bookings.GetBooking;

public sealed class GetBookingHandler(IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingQuery, Result<GetBookingResponse>>
{
    public async Task<Result<GetBookingResponse>> HandleAsync(
        GetBookingQuery query,
        CancellationToken cancellationToken = default)
    {
        var booking = await bookingRepository.GetByIdAsync(query.BookingId, cancellationToken);
        if (booking is null)
            return Result<GetBookingResponse>.Failure(
                Error.NotFound("Booking.NotFound", $"Booking '{query.BookingId}' was not found."));

        return Result<GetBookingResponse>.Success(new GetBookingResponse(
            booking.Id,
            booking.RoomId,
            booking.BookedBy,
            booking.StartUtc,
            booking.EndUtc,
            booking.Attendees,
            booking.Status));
    }
}