using NfcCardManagement.API.Helpers;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Services;

/// <summary>
/// Implémentation du service de gestion de l'unicité globale des CTags NFC.
/// Vérifie l'unicité dans les tables GP_Employe (TagC) et Ref_Vehicule (CTag).
/// </summary>
public class CTagService : ICTagService
{
    private const int MaxAttempts = 10;

    private readonly IEmployeRepository _employeRepository;
    private readonly IVehiculeRepository _vehiculeRepository;

    /// <summary>
    /// Initialise une nouvelle instance de <see cref="CTagService"/>.
    /// </summary>
    /// <param name="employeRepository">Repository d'accès aux données des employés.</param>
    /// <param name="vehiculeRepository">Repository d'accès aux données des véhicules.</param>
    public CTagService(
        IEmployeRepository employeRepository,
        IVehiculeRepository vehiculeRepository)
    {
        _employeRepository = employeRepository;
        _vehiculeRepository = vehiculeRepository;
    }

    /// <inheritdoc />
    public async Task<bool> IsCTagUniqueAsync(string ctag)
    {
        bool existsInEmployes = await _employeRepository.CTagExistsAsync(ctag);
        if (existsInEmployes)
            return false;

        bool existsInVehicules = await _vehiculeRepository.CTagExistsAsync(ctag);
        return !existsInVehicules;
    }

    /// <inheritdoc />
    public async Task<string> GenerateUniqueCTagAsync()
    {
        for (int attempt = 0; attempt < MaxAttempts; attempt++)
        {
            string candidate = CTagHelper.Generate();

            if (await IsCTagUniqueAsync(candidate))
                return candidate;
        }

        throw new InvalidOperationException(
            "Impossible de générer un CTag unique après 10 tentatives.");
    }
}
