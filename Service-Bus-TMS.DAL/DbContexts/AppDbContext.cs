using Microsoft.EntityFrameworkCore;
using Service_Bus_TMS.DAL.Entities;

namespace Service_Bus_TMS.DAL.DbContexts;

public class AppDbContext : DbContext
{
    public DbSet<Task?> Tasks { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
}