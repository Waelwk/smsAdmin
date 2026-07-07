using Microsoft.EntityFrameworkCore;
using NfcCardManagement.API.Models;

namespace NfcCardManagement.API.Data;

/// <summary>
/// Contexte EF Core — Database First, aucune migration.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<GpEmploye> GpEmployes { get; set; } = null!;
    public DbSet<RefVehicule> RefVehicules { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<GpEmploye>(entity =>
        {
            entity.ToTable("GP_Employe");
            entity.HasKey(e => e.Matricule);
        });

        modelBuilder.Entity<RefVehicule>(entity =>
        {
            entity.ToTable("Ref_Vehicule");
            entity.HasKey(e => e.CVehicule);
        });
    }
}
