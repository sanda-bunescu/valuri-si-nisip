using Domain.DTOs.Review;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;
public interface IReviewRepository : IBaseRepository<Review>
{
}
public class ReviewRepository : BaseRepository<Review>, IReviewRepository
{
    private readonly AppDbContext _context;
    
    public ReviewRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }
}