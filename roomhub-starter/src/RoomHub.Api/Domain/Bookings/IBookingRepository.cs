namespace RoomHub.Api.Domain.Bookings;

public interface IBookingRepository
{
    void Add(Booking booking);
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken = default);
}