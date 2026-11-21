using COMP2139_ICE2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Components.ProjectSummary;

public class ProjectSummaryViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _context;

    public ProjectSummaryViewComponent(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync(int projectId)
    {
        // Query the Projects table asynchronously to retrieve the project with the specified ID.
        // Includes the related 'Tasks' navigation property to load associated tasks with the project.
        var project = await _context.Projects
            .Include(p => p.ProjectTasks) // Load related tasks for the project (eager loading).
            .FirstOrDefaultAsync(p => p.ProjectId == projectId); // Retrieve the project with the matching ID.

        // Check if the project is null (not found in the database).
        if (project == null)
        {
            // Return a plain text response indicating the project was not found.
            return Content("Project not found");
        }

        // Return the default view for this ViewComponent, passing the project as the model.
        return View(project);
    }

}