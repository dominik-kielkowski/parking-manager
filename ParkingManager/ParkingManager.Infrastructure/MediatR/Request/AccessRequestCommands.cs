using MediatR;
using ParkingManager.ParkingManager.Domain;

namespace ParkingManager.ParkingManager.Infrastructure.MediatR.Request;

public static class AccessRequestCommands
{
    public record CreateAccessRequestCommand(int UserId, AccessType RequestedAccess)
        : IRequest<AccessRequest>;

    public record ReviewAccessRequestCommand(int RequestId, bool Approve, int AdminId)
        : IRequest<Unit>;
}

public record GetUserRequestsQuery(int UserId) : IRequest<List<AccessRequest>>;
public record GetAllAccessRequestsQuery() : IRequest<List<AccessRequest>>;