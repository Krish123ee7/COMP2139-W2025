using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP2139_ICE2.Models;

namespace COMP2139_ICE2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    public IActionResult About()
    {
        
        return View();
    }
    
    //Lab 6 - notfound() Action added

    [Route("Home/Error/{statusCode?}")]
    public IActionResult NotFound(int? statusCode)
    {
        if (statusCode == 404)
        {
            return View("NotFound");
        }
        return View("Error");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
    // Lab 6 - Part1 - #3 - General Search for Projects or ProjectTasks
// Redirects users to the appropriate search function
    [HttpGet]
    public IActionResult GeneralSearch(string searchType, string searchString)
    {
        // Ensure searchType is not null and handle case-insensitivity
        searchType = searchType?.Trim().ToLower();  

        // Ensure the search string is not empty
        if (string.IsNullOrWhiteSpace(searchType) || string.IsNullOrWhiteSpace(searchString))
        {
            // Redirect back to home if the search is empty
            return RedirectToAction(nameof(Index), "Home");
        }

        // Determine where to redirect based on search type
        if (searchType == "projects")
        {
            // Redirect to Project search
            return RedirectToAction(nameof(ProjectController.Search), "Project", new { searchString });
        }
        else if (searchType == "tasks")
        {               
            // Redirect to ProjectTask search
            return RedirectToAction(nameof(ProjectTaskController.Search), "ProjectTask", new { searchString });             
        }

        // If searchType is invalid, redirect to Home page
        return RedirectToAction(nameof(Index), "Home");
    }
}

