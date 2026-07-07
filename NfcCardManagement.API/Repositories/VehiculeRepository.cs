using Microsoft.EntityFrameworkCore;
using NfcCardManagement.API.Data;
using NfcCardManagement.API.Models;
using NfcCardManagement.API.Repositories.Interfaces;

namespace NfcCardManagement.API.Repositories;

public class VehiculeRepository : IVehiculeRepository
{
    private readonly AppDbContext _context;

    public VehiculeRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<RefVehicule>> GetAllAsync()
        => await _context.RefVehicules.ToListAsync();

    public async Task<RefVehicule?> GetByIdAsync(string id)
        => await _context.RefVehicules.FindAsync(id);

    public async Task<IEnumerable<RefVehicule>> SearchAsync(string keyword)
    {
        var lower = keyword.ToLower();
        return await _context.RefVehicules
            .Where(v =>
                v.CVehicule.ToLower().Contains(lower) ||
                (v.LibVehicule != null && v.LibVehicule.ToLower().Contains(lower)) ||
                (v.NumeroSerie != null && v.NumeroSerie.ToLower().Contains(lower)))
            .ToListAsync();
    }

    public async Task UpdateCTagAsync(string id, string ctag)
    {
        var vehicule = await _context.RefVehicules.FindAsync(id);
        if (vehicule is null) return;
        vehicule.CTag = ctag;
        await _context.SaveChangesAsync();
    }

    public async Task<bool> CTagExistsAsync(string ctag)
        => await _context.RefVehicules.AnyAsync(v => v.CTag == ctag);
}
