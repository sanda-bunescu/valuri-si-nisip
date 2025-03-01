using Domain.DTOs.Beach;
using Domain.DTOs.Location;
using Domain.Entities;
using Domain.Repositories;
using Infrastructure.Mappers;

namespace Infrastructure.Services;

public interface IBeachService
{ 
    Task<BeachResponse> GetByIdAsync(Guid id);
    Task<List<BeachInformation>> GetAllAsync();
    Task<List<BeachWithGradeResponse>> GetRankingAsync();
}
public class BeachService : ServiceBase, IBeachService
{
    public BeachService(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public async Task<BeachResponse> GetByIdAsync(Guid id)
    {
        var beach =  await UnitOfWork.BeachRepository.GetByIdWithLocationAsync(id);
        var beachResponse = beach.ToBeachResponse();
        
        return beachResponse;
    }

    public async Task<List<BeachInformation>> GetAllAsync()
    {
        var beaches = await UnitOfWork.BeachRepository.GetAllAsync();
        var beachInformations = beaches.Select(b => b.ToBeachInformation()).ToList();
        
        return beachInformations;
    }

    public async Task<List<BeachWithGradeResponse>> GetRankingAsync()
    {
        var result = await UnitOfWork.BeachRepository.GetBeachesOrderedByGradeAsync();

        return result;
    }
}