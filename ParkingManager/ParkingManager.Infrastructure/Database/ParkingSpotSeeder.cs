using parking_manager;

public class ParkingSpotSeeder
{
    private static ParkingSpotSeeder? _instance;
    public static ParkingSpotSeeder Instance =>
        _instance ??= new ParkingSpotSeeder();

    private ParkingSpotSeeder() { }

    public void Seed(AppDbContext context, IParkingSpotFactory factory)
    {
        if (context.ParkingSpots.Any())
            return;

        var spots = factory.CreateParkingSpots();
        context.ParkingSpots.AddRange(spots);
        context.SaveChanges();
    }
}