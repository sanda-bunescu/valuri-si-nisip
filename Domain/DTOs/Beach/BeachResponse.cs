using Domain.DTOs.Location;
using Domain.DTOs.Review;
using Domain.Entities;

namespace Domain.DTOs.Beach;

public class BeachResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Grade { get; set; }
    public LocationResponse Location { get; set; }
    public List<ReviewResponse> Reviews { get; set; }
    public List<string> Amentities { get; set; }
}