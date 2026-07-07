namespace NfcCardManagement.API.Services.Interfaces;

/// <summary>
/// Service de gestion et de vérification d'unicité globale des CTags NFC.
/// Un CTag doit être unique parmi toutes les entrées de GP_Employe.TagC et Ref_Vehicule.CTag.
/// </summary>
public interface ICTagService
{
    /// <summary>
    /// Génère un CTag unique garanti parmi les deux tables (GP_Employe et Ref_Vehicule).
    /// Effectue jusqu'à 10 tentatives ; lève une exception si aucun CTag unique n'est trouvé.
    /// </summary>
    /// <returns>Une chaîne de 12 caractères hexadécimaux en majuscules, unique globalement.</returns>
    /// <exception cref="InvalidOperationException">
    /// Levée si 10 tentatives successives produisent toutes des CTags déjà existants.
    /// </exception>
    Task<string> GenerateUniqueCTagAsync();

    /// <summary>
    /// Vérifie qu'un CTag donné est absent des deux tables (GP_Employe.TagC et Ref_Vehicule.CTag).
    /// </summary>
    /// <param name="ctag">La valeur du CTag à vérifier.</param>
    /// <returns><c>true</c> si le CTag n'existe dans aucune des deux tables, <c>false</c> sinon.</returns>
    Task<bool> IsCTagUniqueAsync(string ctag);
}
