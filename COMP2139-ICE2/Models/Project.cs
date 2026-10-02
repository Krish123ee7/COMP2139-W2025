using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE2.Models;

public class Project
{
    // The unique identifier for the project.
    public int ProjectId { get; set; }

    // The name of the project.
    // [Required]: Ensures this property must have a value when the object is validated.
    // required: A C# 11 feature that enforces initialization during object creation.
    [Required]
    public required string Name { get; set; }

    // An optional description of the project.
    // Nullable: Allows this property to have a null value.
    public string? Description { get; set; }

    // The start date of the project.
    // [DataType(DataType.Date)]: Specifies that this property represents a date (not a time).
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }

    // The end date of the project.
    // [DataType(DataType.Date)]: Specifies that this property represents a date (not a time).
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }

    // The current status of the project (e.g., "In Progress", "Completed").
    public string? Status { get; set; }
}
