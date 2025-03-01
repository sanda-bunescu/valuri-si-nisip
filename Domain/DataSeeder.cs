using Domain.Entities;

namespace Domain;
public interface IDataSeeder
{
    void Seed(); 
}
public class DataSeeder : IDataSeeder
{
    private readonly AppDbContext _context;

    public DataSeeder(AppDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        SeedLocations();
        SeedBeaches();
        SeedAmenities();
        SeedBeachAmenities();
    }

    private void SeedLocations()
    {
        if (!_context.Locations.Any())
        {
            var locations = new List<Location>
            {
                new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "Constanța"
                },
                new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "Eforie"
                },
                new Location
                {
                    Id = Guid.NewGuid(),
                    Name = "Costinești"
                }
            };
            _context.Locations.AddRange(locations);
            _context.SaveChanges();
        }
    }
    private void SeedBeaches()
    {
        if (!_context.Beaches.Any())
        {
            var locationsDict = _context.Locations.ToDictionary(loc => loc.Name, loc => loc.Id);
            IEnumerable<Beach> beaches = new List<Beach>()
            {
                new Beach()
                {
                    Id = Guid.NewGuid(),
                    LocationId = locationsDict["Constanța"],
                    Name = "Constanța Modern Beach",
                    Latitude = 44.18060775708171,
                    Longitude = 28.656102833264722
                },
                new Beach
                {
                    Id = Guid.NewGuid(),
                    LocationId = locationsDict["Constanța"],
                    Name = "Trei Papuci Beach",
                    Latitude = 44.20983647474423,
                    Longitude = 28.651208019081068
                },
                new Beach
                {
                    Id = Guid.NewGuid(),
                    LocationId = locationsDict["Constanța"],
                    Name = "Faleza Nord Beach",
                    Latitude = 44.21028252961649,
                    Longitude = 28.65227721787001
                },
                new Beach
                {
                    Id = Guid.NewGuid(),
                    LocationId = locationsDict["Eforie"],
                    Name = "Eforie Nord Beach",
                    Latitude = 44.06128150370384,
                    Longitude = 28.640683175987498
                },
                new Beach
                {
                    Id = Guid.NewGuid(),
                    LocationId = locationsDict["Eforie"],
                    Name = "Eforie Sud Beach",
                    Latitude = 44.031675896713466,
                    Longitude = 28.653906445878807
                },
                new Beach
                {
                    Id = Guid.NewGuid(),
                    LocationId = locationsDict["Costinești"],
                    Name = "Costinești Beach",
                    Latitude = 43.94780351324858,
                    Longitude = 28.638085932408234
                },
            };
            _context.Beaches.AddRange(beaches);
            _context.SaveChanges();
        }
    }
    
    private void SeedAmenities()
    {
        if (!_context.Amentities.Any())
        {
            var amenities = new List<Amentity>
            {
                new Amentity { Id = Guid.NewGuid(), Name = "Restaurant" },
                new Amentity { Id = Guid.NewGuid(), Name = "Lifeguard" },
                new Amentity { Id = Guid.NewGuid(), Name = "Shower" },
                new Amentity { Id = Guid.NewGuid(), Name = "Parking Lot" },
                new Amentity { Id = Guid.NewGuid(), Name = "Bar/Terrace" }
            };
            _context.Amentities.AddRange(amenities);
            _context.SaveChanges();
        }
    }
    
    private void SeedBeachAmenities()
    {
        if (!_context.BeachAmentities.Any())
        {
            var beaches = _context.Beaches.ToDictionary(b => b.Name, b => b.Id);
            var amenities = _context.Amentities.ToDictionary(a => a.Name, a => a.Id);

            var beachAmenities = new List<BeachAmentity>
            {
                new BeachAmentity { Id = Guid.NewGuid(), BeachId = beaches["Constanța Modern Beach"], AmentityId = amenities["Restaurant"] },
                new BeachAmentity { Id = Guid.NewGuid(), BeachId = beaches["Constanța Modern Beach"], AmentityId = amenities["Shower"] },
                new BeachAmentity { Id = Guid.NewGuid(), BeachId = beaches["Eforie Nord Beach"], AmentityId = amenities["Lifeguard"] },
                new BeachAmentity { Id = Guid.NewGuid(), BeachId = beaches["Eforie Nord Beach"], AmentityId = amenities["Parking Lot"] },
                new BeachAmentity { Id = Guid.NewGuid(), BeachId = beaches["Costinești Beach"], AmentityId = amenities["Bar/Terrace"] },
                new BeachAmentity { Id = Guid.NewGuid(), BeachId = beaches["Costinești Beach"], AmentityId = amenities["Restaurant"] }
            };

            _context.BeachAmentities.AddRange(beachAmenities);
            _context.SaveChanges();
        }
    }

}