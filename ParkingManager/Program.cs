using parking_manager;
using ParkingManager.ParkingManager.Application;
using ParkingManager.ParkingManager.Infrastructure;
using ParkingManager.ParkingManager.Infrastructure.Database;
using ParkingManager.ParkingManager.Infrastructure.MediatR;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter()
        )
    );

builder.Services.AddOpenApi();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSqliteDatabase<AppDbContext>(builder.Configuration);
builder.AddGitHubAuthentication();
builder.Services.ConfigureMediatR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    var factory = new JsonParkingSpotFactory("ParkingManager.Infrastructure/Data/ParkingSpots.json");

    ParkingSpotSeeder.Instance.Seed(context, factory);
}

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseSwagger();
app.UseSwaggerUI(c => 
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    c.RoutePrefix = "swagger";
});

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.Run();