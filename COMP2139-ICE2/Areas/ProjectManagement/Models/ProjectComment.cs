using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE2.Areas.ProjectManagement.Models;

public class ProjectComment
{
    //File name ProjectComments.cs

    public int ProjectCommentId { get; set; }

    [Required]
    [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
    public string? Content { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
    private DateTime _datePosted;

    public DateTime DatePosted
    {
        get => _datePosted;
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }

    // Foreign key for Project
    public int ProjectId { get; set; }

    // Navigation property to Project
    public Project? Project { get; set; }
}