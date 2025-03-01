using Domain.DTOs.Beach;
using Domain.Entities;

namespace Infrastructure.Mappers;

public static class BeachMapper
{
    public static BeachResponse ToBeachResponse(this Beach beach)
    {
        var beachResponse = new BeachResponse();

        beachResponse.Id = beach.Id;
        beachResponse.Name = beach.Name;
        beachResponse.Latitude = beach.Latitude;
        beachResponse.Longitude = beach.Longitude;
        beachResponse.Grade = Math.Round(beach.Reviews.Select(r => r.Grade).DefaultIfEmpty(0).Average(),2);
        beachResponse.Location = beach.Location.ToLocationResponse();
        beachResponse.Reviews = beach.Reviews.Select(r => r.ToReviewResponse()).ToList(); 
        beachResponse.Amentities = beach.BeachAmentity.Select(a => a.Amentity.Name).ToList();

        return beachResponse;
    }

    public static BeachInformation ToBeachInformation(this Beach beach)
    {
        var beachInformation = new BeachInformation();
        
        beachInformation.Id = beach.Id;
        beachInformation.Name = beach.Name;
        beachInformation.Latitude = beach.Latitude;
        beachInformation.Longitude = beach.Longitude;
        
        return beachInformation;
    }
}