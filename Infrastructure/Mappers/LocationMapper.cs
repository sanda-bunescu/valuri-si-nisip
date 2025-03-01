using Domain.DTOs.Location;
using Domain.Entities;

namespace Infrastructure.Mappers;

public static class LocationMapper
{
    public static LocationResponse ToLocationResponse(this Location location)
    {
        var locationResponse = new LocationResponse();
        
        locationResponse.Id = location.Id;
        locationResponse.Name = location.Name;
        
        return locationResponse;
    }
}