using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Review;

public class ReviewRequest
{
    [Required]
    public Guid BeachId { get; set; } 
    [Required]
    public Guid AccountId { get; set; } 

    public string Description { get; set; }
    public int Grade { get; set; }
}