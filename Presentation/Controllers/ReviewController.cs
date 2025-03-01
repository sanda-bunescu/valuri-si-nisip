using Domain.DTOs.Review;
using Domain.Entities;
using Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/review")]
[ApiController]
public class ReviewController : ControllerBase
{
    private readonly IReviewService _reviewService;

    public ReviewController(IReviewService reviewService)
    {
        _reviewService = reviewService;
    }
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> AddReview([FromBody]ReviewRequest reviewRequest)
    { 
        await _reviewService.AddAsync(reviewRequest);
        
        return Ok();
    }
}