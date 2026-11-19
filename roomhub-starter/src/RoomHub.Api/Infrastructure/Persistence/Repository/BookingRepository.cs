using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain.Bookings;

namespace RoomHub.Api.Infrastructure.Persistence.Repository;

public sealed class BookingRepository(RoomHubDbContext dbContext) : IBookingRepository
{
    public void Add(Booking booking) => dbContext.Bookings.Add(booking);

    public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        dbContext.Bookings.FirstOrDefaultAsync(b => b.Id == id, cancellationToken);

    public Task<List<Booking>> GetActiveForRoomAsync(Guid roomId, CancellationToken cancellationToken = default) =>
        dbContext.Bookings
            .Where(b => b.RoomId == roomId && b.Status == BookingStatus.Active)
            .ToListAsync(cancellationToken);
}