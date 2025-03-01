using Domain.DTOs.Review;
using Domain.Entities;

namespace Infrastructure.Mappers;

public static class ReviewMapper
{
    public static ReviewResponse ToReviewResponse(this Review review)
    {
        var reviewResponse = new ReviewResponse();
        
        reviewResponse.Description = review.Description;
        reviewResponse.Owner = review.Account.Name;
        reviewResponse.CreatedAt = review.CreatedAt;
        reviewResponse.Grade = review.Grade;
        
        return reviewResponse;
    }
}