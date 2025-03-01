using Domain.DTOs.Review;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public interface IReviewService
{
    Task AddAsync(ReviewRequest review);
}

public class ReviewService : ServiceBase, IReviewService
{
    public ReviewService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task AddAsync(ReviewRequest reviewRequest)
    {
        var review = new Review();
        review.Description = reviewRequest.Description;
        review.BeachId = reviewRequest.BeachId;
        review.AccountId = reviewRequest.AccountId;
        review.Grade = reviewRequest.Grade;
        
        await UnitOfWork.ReviewRepository.AddAsync(review);
        await UnitOfWork.SaveChangesAsync();
    }
}