using NfcCardManagement.API.DTOs.Carte;
using NfcCardManagement.API.Exceptions;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Services;

/// <summary>Implémentation du service de génération des données NFC.</summary>
public class CarteService : ICarteService
{
    private readonly IEmployeRepository _employeRepo;
    private readonly IVehiculeRepository _vehiculeRepo;

    public CarteService(IEmployeRepository employeRepo, IVehiculeRepository vehiculeRepo)
    {
        _employeRepo = employeRepo;
        _vehiculeRepo = vehiculeRepo;
    }

    /// <inheritdoc />
    public async Task<NfcDataDto> GetCarteChaufeurAsync(string matricule)
    {
        var employe = await _employeRepo.GetByMatriculeAsync(matricule)
            ?? throw new NotFoundException($"Chauffeur '{matricule}' non trouvé.");

        if (employe.TagC == null)
            throw new UnprocessableEntityException(
                "Le CTag doit être généré avant la création de la carte.");

        if (employe.MotDePasseC == null)
            throw new UnprocessableEntityException(
                "Le mot de passe doit être généré avant la création de la carte.");

        return new NfcDataDto
        {
            Record1 = employe.TagC,
            Record2 = employe.MotDePasseC  // mot de passe en clair
        };
    }

    public async Task<NfcDataDto> GetCarteVehiculeAsync(string id)
    {
        var vehicule = await _vehiculeRepo.GetByIdAsync(id)
            ?? throw new NotFoundException($"Véhicule '{id}' non trouvé.");

        if (vehicule.CTag == null)
            throw new UnprocessableEntityException(
                "Le CTag doit être généré avant la création de la carte.");

        return new NfcDataDto { Record1 = vehicule.CTag, Record2 = null };
    }
}
