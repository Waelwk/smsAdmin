using Microsoft.EntityFrameworkCore;
using NfcCardManagement.API.Data;
using NfcCardManagement.API.Models;
using NfcCardManagement.API.Repositories.Interfaces;

namespace NfcCardManagement.API.Repositories;

/// <summary>
/// Implémentation EF Core du repository d'accès aux données des employés/chauffeurs.
/// </summary>
public class EmployeRepository : IEmployeRepository
{
    private readonly AppDbContext _context;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="EmployeRepository"/>.
    /// </summary>
    /// <param name="context">Le contexte EF Core injecté.</param>
    public EmployeRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retourne tous les employés enregistrés en base.
    /// </summary>
    public async Task<IEnumerable<GpEmploye>> GetAllAsync()
    {
        return await _context.GpEmployes.ToListAsync();
    }

    /// <summary>
    /// Retourne l'employé correspondant au matricule spécifié, ou null s'il n'existe pas.
    /// </summary>
    /// <param name="matricule">Le matricule de l'employé à rechercher.</param>
    public async Task<GpEmploye?> GetByMatriculeAsync(string matricule)
    {
        return await _context.GpEmployes.FindAsync(matricule);
    }

    /// <summary>
    /// Recherche des employés dont le matricule ou la concaténation Nom+Prénom
    /// contient le mot-clé (insensible à la casse).
    /// </summary>
    /// <param name="keyword">Le mot-clé de recherche.</param>
    public async Task<IEnumerable<GpEmploye>> SearchAsync(string keyword)
    {
        var lowerKeyword = keyword.ToLower();

        return await _context.GpEmployes
            .Where(e =>
                e.Matricule.ToLower().Contains(lowerKeyword) ||
                (e.NomPrenom != null && e.NomPrenom.ToLower().Contains(lowerKeyword)))
            .ToListAsync();
    }

    /// <summary>
    /// Met à jour le mot de passe (stocké en clair) d'un employé.
    /// </summary>
    /// <param name="matricule">Le matricule de l'employé.</param>
    /// <param name="password">Le mot de passe en clair à stocker.</param>
    public async Task UpdatePasswordAsync(string matricule, string password)
    {
        var employe = await _context.GpEmployes.FindAsync(matricule);
        if (employe is null)
            return;

        employe.MotDePasseC = password;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Met à jour le CTag NFC d'un employé.
    /// </summary>
    /// <param name="matricule">Le matricule de l'employé.</param>
    /// <param name="ctag">La valeur du CTag à stocker.</param>
    public async Task UpdateCTagAsync(string matricule, string ctag)
    {
        var employe = await _context.GpEmployes.FindAsync(matricule);
        if (employe is null)
            return;

        employe.TagC = ctag;
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Vérifie si un CTag existe déjà dans la table GP_Employe.
    /// </summary>
    /// <param name="ctag">La valeur du CTag à vérifier.</param>
    /// <returns>True si le CTag est déjà utilisé, false sinon.</returns>
    public async Task<bool> CTagExistsAsync(string ctag)
    {
        return await _context.GpEmployes.AnyAsync(e => e.TagC == ctag);
    }
}
