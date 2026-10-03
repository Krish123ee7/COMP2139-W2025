using COMP2139_ICE2.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE2.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
        
    }
    
    public DbSet<Project> Projects { get; set; }
}