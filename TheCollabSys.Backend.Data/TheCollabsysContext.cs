using Microsoft.EntityFrameworkCore;
using TheCollabSys.Backend.Entity.Models;

namespace TheCollabSys.Backend.Data;

public partial class TheCollabsysContext : DbContext
{
    public TheCollabsysContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<DD_Clients> DD_Clients { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DD_Clients>().HasKey(e => e.ClientID);

        base.OnModelCreating(modelBuilder);
    }
}
