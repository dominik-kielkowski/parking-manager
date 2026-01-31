using MediatR;
using Microsoft.EntityFrameworkCore;
using ParkingManager.ParkingManager.Domain;
using ParkingManager.ParkingManager.Infrastructure.Database;

namespace ParkingManager.ParkingManager.Infrastructure.MediatR.Request;

public class AccessRequestHandlers :
    IRequestHandler<AccessRequestCommands.CreateAccessRequestCommand, AccessRequest>,
    IRequestHandler<AccessRequestCommands.ReviewAccessRequestCommand, Unit>,
    IRequestHandler<GetUserRequestsQuery, List<AccessRequest>>,
    IRequestHandler<GetAllAccessRequestsQuery, List<AccessRequest>>
{
    private readonly AppDbContext _context;

    public AccessRequestHandlers(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AccessRequest> Handle(
        AccessRequestCommands.CreateAccessRequestCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .Include(u => u.AccessRequests)
            .FirstOrDefaultAsync(u => u.Id == command.UserId, cancellationToken)
            ?? throw new Exception("User not found");

        AccessRequest request = command.RequestedAccess switch
        {
            AccessType.DisabledPermit => new DisabledPermitRequest { User = user, UserId = user.Id },
            AccessType.Manager => new ManagerAccessRequest { User = user, UserId = user.Id },
            _ => throw new ArgumentOutOfRangeException()
        };

        _context.AccessRequests.Add(request);
        user.AccessRequests.Add(request);
        await _context.SaveChangesAsync(cancellationToken);

        return request;
    }

    public async Task<Unit> Handle(
        AccessRequestCommands.ReviewAccessRequestCommand command,
        CancellationToken cancellationToken)
    {
        var request = await _context.AccessRequests
            .Include(r => r.User)
            .FirstOrDefaultAsync(r => r.Id == command.RequestId, cancellationToken)
            ?? throw new Exception("Request not found");

        var admin = await _context.Users
            .FirstOrDefaultAsync(u => u.Id == command.AdminId && u.IsAdmin, cancellationToken)
            ?? throw new Exception("Admin not found");

        request.IsApproved = command.Approve;
        request.Apply();

        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }

    public async Task<List<AccessRequest>> Handle(
        GetUserRequestsQuery query,
        CancellationToken cancellationToken)
    {
        return await _context.AccessRequests
            .Include(r => r.User)
            .Where(r => r.UserId == query.UserId)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AccessRequest>> Handle(
        GetAllAccessRequestsQuery query,
        CancellationToken cancellationToken)
    {
        return await _context.AccessRequests
            .Include(r => r.User)
            .ToListAsync(cancellationToken);
    }
}