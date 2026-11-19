using RoomHub.Api.Domain.Common;
using RoomHub.Api.Domain.Rooms;

namespace RoomHub.Api.Domain.Bookings;

public sealed class Booking
{
    private static readonly TimeSpan MaxDuration = TimeSpan.FromHours(4);

    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking() { }

    private Booking(
        Guid id,
        Guid roomId,
        string bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
        Status = BookingStatus.Active;
    }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        TimeProvider timeProvider)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
            return Result<Booking>.Failure(Error.Validation("Booking.BookedByRequired", "BookedBy is required."));

        var normalizedStart = NormalizeUtc(startUtc);
        var normalizedEnd = NormalizeUtc(endUtc);

        if (normalizedEnd <= normalizedStart)
            return Result<Booking>.Failure(Error.Validation("Booking.InvalidInterval", "EndUtc must be strictly after StartUtc."));

        if (attendees <= 0 || attendees > room.Capacity)
            return Result<Booking>.Failure(Error.Validation("Booking.InvalidAttendees", $"Attendees must be between 1 and {room.Capacity}."));

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        if (normalizedStart < nowUtc)
            return Result<Booking>.Failure(Error.Validation("Booking.InThePast", "A booking cannot start in the past."));

        if (normalizedEnd - normalizedStart > MaxDuration)
            return Result<Booking>.Failure(Error.Validation("Booking.ExceedsMaxDuration", "The maximum duration of a booking is 4 hours."));

        return Result<Booking>.Success(new Booking(
            Guid.NewGuid(),
            room.Id,
            bookedBy.Trim(),
            normalizedStart,
            normalizedEnd,
            attendees));
    }

    public bool Overlaps(DateTime startUtc, DateTime endUtc) =>
        Status == BookingStatus.Active && StartUtc < endUtc && startUtc < EndUtc;

    public Result<Unit> Cancel()
    {
        if (Status == BookingStatus.Cancelled)
            return Result<Unit>.Failure(Error.Conflict("Booking.AlreadyCancelled", "Booking is already cancelled."));

        Status = BookingStatus.Cancelled;
        return Result<Unit>.Success(Unit.Value);
    }

    private static DateTime NormalizeUtc(DateTime value) =>
        value.Kind == DateTimeKind.Local
            ? value.ToUniversalTime()
            : DateTime.SpecifyKind(value, DateTimeKind.Utc);
}