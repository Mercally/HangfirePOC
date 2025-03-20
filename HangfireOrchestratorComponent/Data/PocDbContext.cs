using Microsoft.EntityFrameworkCore;

namespace HangfireOrchestratorComponent.Data;

public class PocDbContext : DbContext
{
    public DbSet<Workflow> Workflows { get; set; }

    public PocDbContext()
    {

    }

    public PocDbContext(DbContextOptions<PocDbContext> dbContextOptions)
        : base(dbContextOptions)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Workflow>();
    }
}
