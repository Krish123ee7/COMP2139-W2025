using COMP2139_ICE2.Areas.ProjectManagement.Models; // Namespace for models specific to the ProjectManagement area
using COMP2139_ICE2.Data; // Namespace for database context
using Microsoft.AspNetCore.Mvc; // Namespace for ASP.NET Core MVC features
using Microsoft.EntityFrameworkCore; // Namespace for Entity Framework Core functionalities

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers
{
    [Area("ProjectManagement")] // Specifies that this controller belongs to the "ProjectManagement" area
    [Route("[area]/[controller]/[action]")] // Defines the route template for the controller's actions
    public class ProjectCommentController : Controller
    {
        private readonly ApplicationDbContext _context; // The application's database context
        private readonly ILogger<ProjectCommentController> _logger;

        /// <summary>
        /// Constructor to inject the database context.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ProjectCommentController(ApplicationDbContext context, ILogger<ProjectCommentController> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of comments for a specific project.
        /// </summary>
        /// <param name="projectId">The ID of the project to fetch comments for.</param>
        /// <returns>A JSON object containing the comments ordered by most recent.</returns>
        [HttpGet]
        public async Task<IActionResult> GetComments(int projectId)
        {
            // Query the database to retrieve comments for the specified project,
            // ordered by the most recent posting date (descending order).
            var comments = await _context.ProjectComments
                .Where(c => c.ProjectId == projectId) // Filter comments by project ID
                .OrderByDescending(c => c.DatePosted) // Order comments by date posted (newest first)
                .ToListAsync(); // Execute the query and convert the result to a list

            // Return the comments as a JSON response
            return Json(comments);
        }

        /// <summary>
        /// Adds a new comment to a project.
        /// </summary>
        /// <param name="comment">The comment object provided in the request body.</param>
        /// <returns>
        /// A JSON response indicating success or failure of the operation,
        /// along with appropriate messages and error details (if any).
        /// </returns>
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
        {
            // Check if the comment object is valid based on the model's data annotations
            if (ModelState.IsValid)
            {
                // Set the current date and time for the DatePosted property
                comment.DatePosted = DateTime.Now;

                // Add the comment to the database context
                _context.ProjectComments.Add(comment);

                // Save changes to the database asynchronously
                await _context.SaveChangesAsync();

                // Return a success JSON response
                return Json(new { success = true, message = "Comment added successfully." });
            }

            // If the model state is invalid, collect the validation errors
            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);

            // Return a failure JSON response with error details
            return Json(new { success = false, message = "Invalid comment data.", errors = errors });
        }
    }
}
