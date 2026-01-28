using MediatR;
using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Domain;
using ParkingManager.ParkingManager.Infrastructure.Database;

namespace ParkingManager.ParkingManager.Infrastructure.MediatR.Parking
{
    public class ParkingHandlers :
        IRequestHandler<BookParkingCommand, string>,
        IRequestHandler<GetParkingSpotsQuery, IEnumerable<object>>
    {
        private readonly AppDbContext _context;

        public ParkingHandlers(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> Handle(BookParkingCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.GitHubId == request.GithubId, cancellationToken);
            if (user == null) throw new InvalidOperationException("User not found");

            var spot = await _context.ParkingSpots
                .Include(s => s.Reservation)
                .FirstOrDefaultAsync(s => s.SpotNumber == request.SpotNumber, cancellationToken);

            if (spot == null) throw new InvalidOperationException("Parking spot not found");
            if (spot.Reservation != null) throw new InvalidOperationException("Spot already booked");

            bool hasAccess = spot.SpotType switch
            {
                ParkingSpotType.Disabled => user.HasDisabledPermit,
                ParkingSpotType.Manager => user.HasManagerAccess,
                ParkingSpotType.Regular => true
            };

            if (!hasAccess)
                throw new InvalidOperationException("User does not have permission for this spot");

            var reservation = new ParkingReservation
            {
                ParkingSpotId = spot.Id,
                UserId = user.Id,
                ReservedAt = DateTime.UtcNow
            };

            _context.ParkingReservations.Add(reservation);
            await _context.SaveChangesAsync(cancellationToken);

            return $"Spot {spot.SpotNumber} booked for {user.UserName}";
        }


        public async Task<IEnumerable<object>> Handle(GetParkingSpotsQuery request, CancellationToken cancellationToken)
        {
            var spots = await _context.ParkingSpots
                .Include(s => s.Reservation)
                .ToListAsync(cancellationToken);

            return spots.Select(s => new
            {
                s.SpotNumber,
                IsTaken = s.Reservation != null,
                s.SpotType
            });
        }
    }
}
