using System.Runtime.InteropServices.JavaScript;

namespace Domain.DTOs.Review;

public class ReviewResponse
{
    public string Description { get; set; }
    public string Owner  { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Grade { get; set; }
}