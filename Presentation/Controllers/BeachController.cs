using Domain.DTOs.Beach;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/beach")]
[ApiController]
public class BeachController : GenericController
{
    private readonly IBeachService _beachService;

    public BeachController(IBeachService beachService)
    {
        _beachService = beachService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<BeachResponse>> GetBeachByIdAsync([FromRoute] Guid id)
    {
        var beach = await _beachService.GetByIdAsync(id);
        
        return Ok(beach);
    }
    
    [HttpGet]
    public async Task<ActionResult<List<BeachInformation>>> GetAllAsync()
    {
        var beach = await _beachService.GetAllAsync();
        
        return Ok(beach);
    }
    
    [HttpGet("ranking")]
    public async Task<ActionResult<List<BeachInformation>>> GetBeachRankingAsync()
    {
        var beachRanking = await _beachService.GetRankingAsync();
        
        return Ok(beachRanking);
    }
}