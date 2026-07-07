using NfcCardManagement.API.DTOs.Employe;

namespace NfcCardManagement.API.Services.Interfaces;

/// <summary>Service métier pour la gestion des chauffeurs.</summary>
public interface IEmployeService
{
    Task<IEnumerable<EmployeListDto>> GetAllAsync();
    Task<EmployeDetailDto> GetByMatriculeAsync(string matricule);
    Task<IEnumerable<EmployeListDto>> SearchAsync(string keyword);
    /// <summary>Génère un mot de passe, le stocke en clair, retourne la valeur en clair.</summary>
    Task<string> GeneratePasswordAsync(string matricule);
    Task<string> GenerateCTagAsync(string matricule);
    Task UpdateCTagAsync(string matricule, string ctag);
}
