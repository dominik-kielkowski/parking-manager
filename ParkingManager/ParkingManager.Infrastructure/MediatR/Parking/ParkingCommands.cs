using MediatR;

namespace ParkingManager.ParkingManager.Infrastructure.MediatR.Parking
{
    public record BookParkingCommand(string GithubId, string SpotNumber) : IRequest<string>;
    public record GetParkingSpotsQuery() : IRequest<IEnumerable<object>>;
}