using COMP2139_ICE2.Data;
using COMP2139_ICE2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE2.Controllers;

/// <summary>
/// Controller responsible for managing project tasks.
/// </summary>
public class ProjectTaskController : Controller
{
    /// <summary>
    /// Database context used to access application data.
    /// </summary>
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="ProjectTaskController"/> class.
    /// </summary>
    /// <param name="context">Application database context.</param>
    public ProjectTaskController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Displays all tasks that belong to a specific project.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A list of project tasks.</returns>
    [HttpGet]
    public IActionResult Index(int projectId)
    {
        var tasks = _context.ProjectTasks
                            .Where(t => t.ProjectId == projectId)
                            .ToList();

        ViewBag.ProjectId = projectId;

        return View(tasks);
    }

    /// <summary>
    /// Displays the details of a selected project task.
    /// </summary>
    /// <param name="id">The task identifier.</param>
    /// <returns>The task details view.</returns>
    [HttpGet]
    public IActionResult Details(int id)
    {
        var task = _context.ProjectTasks
                           .Include(t => t.Project)
                           .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    /// <summary>
    /// Displays the Create Task form.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>The Create view.</returns>
    [HttpGet]
    public IActionResult Create(int projectId)
    {
        var project = _context.Projects.Find(projectId);

        if (project == null)
        {
            return NotFound();
        }

        var task = new ProjectTask
        {
            ProjectId = projectId,
            Title = string.Empty,
            Description = string.Empty
        };

        return View(task);
    }

    /// <summary>
    /// Creates a new project task.
    /// </summary>
    /// <param name="task">The task to create.</param>
    /// <returns>Redirects to the task list if successful.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (ModelState.IsValid)
        {
            _context.ProjectTasks.Add(task);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }

        ViewBag.Projects = new SelectList(
            _context.Projects,
            "ProjectId",
            "Name",
            task.ProjectId);

        return View(task);
    }

    /// <summary>
    /// Displays the Edit Task form.
    /// </summary>
    /// <param name="id">The task identifier.</param>
    /// <returns>The Edit view.</returns>
    [HttpGet]
    public IActionResult Edit(int id)
    {
        var task = _context.ProjectTasks
                           .Include(t => t.Project)
                           .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        ViewBag.Projects = new SelectList(
            _context.Projects,
            "ProjectId",
            "Name",
            task.ProjectId);

        return View(task);
    }

    /// <summary>
    /// Updates an existing project task.
    /// </summary>
    /// <param name="id">The task identifier.</param>
    /// <param name="task">The updated task information.</param>
    /// <returns>Redirects to the task list if successful.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(
        int id,
        [Bind("ProjectTaskId", "Title", "Description", "ProjectId")] ProjectTask task)
    {
        if (id != task.ProjectTaskId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            _context.ProjectTasks.Update(task);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
        }

        ViewBag.Projects = new SelectList(
            _context.Projects,
            "ProjectId",
            "Name",
            task.ProjectId);

        return View(task);
    }

    /// <summary>
    /// Displays the Delete confirmation page.
    /// </summary>
    /// <param name="id">The task identifier.</param>
    /// <returns>The Delete confirmation view.</returns>
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var task = _context.ProjectTasks
                           .Include(t => t.Project)
                           .FirstOrDefault(t => t.ProjectTaskId == id);

        if (task == null)
        {
            return NotFound();
        }

        return View(task);
    }

    /// <summary>
    /// Deletes the selected project task.
    /// </summary>
    /// <param name="projectTaskId">The task identifier.</param>
    /// <returns>Redirects to the task list if successful.</returns>
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int projectTaskId)
    {
        var task = _context.ProjectTasks.Find(projectTaskId);

        if (task == null)
        {
            return NotFound();
        }

        _context.ProjectTasks.Remove(task);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index), new { projectId = task.ProjectId });
    }
    // Lab 6 - Search ProjectTasks
// GET: ProjectTasks/Search/{projectId?}/{searchString?}
    [HttpGet("Search")]
    public async Task<IActionResult> Search(int? projectId, string searchString)
    {
        // Start with all tasks as an IQueryable query (deferred execution)
        var taskQuery = _context.ProjectTasks.AsQueryable();

        // Track whether a search was performed
        bool searchPerformed = !string.IsNullOrWhiteSpace(searchString);

        // If a projectId is provided, filter by project
        if (projectId.HasValue)
        {
            taskQuery = taskQuery.Where(t => t.ProjectId == projectId.Value);
        }

        // ❗ FIXED: Apply search filter when searchString is provided
        if (searchPerformed)
        {
            searchString = searchString.ToLower(); // Case-insensitive search

            // Ensure null-safe search on nullable Description
            taskQuery = taskQuery.Where(t =>
                t.Title.ToLower().Contains(searchString) ||
                (t.Description != null && t.Description.ToLower().Contains(searchString))
            );
        }

        // ❗ WHY ASYNC? ❗
        // The database query is executed asynchronously using `ToListAsync()`
        // This prevents blocking the main thread while waiting for the result.
        var tasks = await taskQuery.ToListAsync();

        // Pass search metadata to the view for UI updates
        ViewBag.ProjectId = projectId;
        ViewData["SearchPerformed"] = searchPerformed;
        ViewData["SearchString"] = searchString;

        // Reuse Index view to display filtered results
        return View("Index", tasks);
    }
}