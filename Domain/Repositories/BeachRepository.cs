using Domain.DTOs.Beach;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface IBeachRepository : IBaseRepository<Beach>
{
    Task<Beach> GetByIdWithLocationAsync(Guid id);
    Task<List<BeachWithGradeResponse>> GetBeachesOrderedByGradeAsync();
}
public class BeachRepository : BaseRepository<Beach>, IBeachRepository
{
    private readonly AppDbContext _context;
    
    public BeachRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<Beach> GetByIdWithLocationAsync(Guid id)
    {
        var result = await _context.Beaches
            .Include(b => b.Location)
            .Include(b => b.Reviews)
                .ThenInclude(r => r.Account)
            .Include(b => b.BeachAmentity)
                .ThenInclude(ba => ba.Amentity)
            .FirstOrDefaultAsync(e => e.Id == id);
        
        return result;
    }

    public async Task<List<BeachWithGradeResponse>> GetBeachesOrderedByGradeAsync()
    {
        var result = await _context.Beaches
            .Select(beach => new BeachWithGradeResponse
            {
                Name = beach.Name,
                Grade = beach.Reviews.Any() ? beach.Reviews.Average(r => r.Grade) : 0
            })
            .OrderByDescending(b => b.Grade)
            .ToListAsync();

        return result;
    }
}