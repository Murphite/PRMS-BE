using PRMS.Domain.Entities;

namespace PRMS.Core.Dtos;

public class PhysicianReviewDto
{
    public string PhysicianId { get; set; }
    public string PatientId { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public string? Name { get; set; }
    public string? Image { get; set; }
    public int Rating { get; set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; }

    public Physician Physician { get; set; }
    public Patient Patient { get; set; }
    }

