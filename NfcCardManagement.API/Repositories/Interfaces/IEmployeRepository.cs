using NfcCardManagement.API.Models;

namespace NfcCardManagement.API.Repositories.Interfaces;

/// <summary>
/// Interface du repository d'accès aux données des employés/chauffeurs.
/// </summary>
public interface IEmployeRepository
{
    /// <summary>
    /// Retourne tous les employés enregistrés en base.
    /// </summary>
    Task<IEnumerable<GpEmploye>> GetAllAsync();

    /// <summary>
    /// Retourne l'employé correspondant au matricule spécifié, ou null s'il n'existe pas.
    /// </summary>
    /// <param name="matricule">Le matricule de l'employé à rechercher.</param>
    Task<GpEmploye?> GetByMatriculeAsync(string matricule);

    /// <summary>
    /// Recherche des employés dont le matricule ou la concaténation Nom+Prénom
    /// contient le mot-clé (insensible à la casse).
    /// </summary>
    /// <param name="keyword">Le mot-clé de recherche.</param>
    Task<IEnumerable<GpEmploye>> SearchAsync(string keyword);

    /// <summary>
    /// Met à jour le mot de passe (stocké en clair) d'un employé.
    /// </summary>
    /// <param name="matricule">Le matricule de l'employé.</param>
    /// <param name="password">Le mot de passe en clair à stocker.</param>
    Task UpdatePasswordAsync(string matricule, string password);

    /// <summary>
    /// Met à jour le CTag NFC d'un employé.
    /// </summary>
    /// <param name="matricule">Le matricule de l'employé.</param>
    /// <param name="ctag">La valeur du CTag à stocker.</param>
    Task UpdateCTagAsync(string matricule, string ctag);

    /// <summary>
    /// Vérifie si un CTag existe déjà dans la table GP_Employe.
    /// </summary>
    /// <param name="ctag">La valeur du CTag à vérifier.</param>
    /// <returns>True si le CTag est déjà utilisé, false sinon.</returns>
    Task<bool> CTagExistsAsync(string ctag);
}
