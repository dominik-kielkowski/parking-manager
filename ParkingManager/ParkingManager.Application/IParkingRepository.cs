namespace ParkingManager.ParkingManager.Application;

public interface IParkingRepository
{
    Task<string> BookSpotAsync(string githubId, string spotNumber);
    Task<IEnumerable<object>> GetSpotsStatusAsync();
}