namespace NfcCardManagement.API.Helpers;

/// <summary>
/// Fournit des utilitaires de génération d'identifiants CTag pour les chauffeurs et véhicules.
/// </summary>
public static class CTagHelper
{
    /// <summary>
    /// Génère un identifiant CTag court basé sur un GUID.
    /// L'identifiant correspond aux 12 premiers caractères hexadécimaux du GUID (sans tirets), en majuscules.
    /// Exemple : <c>A3F2B1C4D5E6</c>
    /// </summary>
    /// <returns>
    /// Une chaîne de 12 caractères hexadécimaux en majuscules.
    /// </returns>
    public static string Generate()
    {
        string guidWithoutDashes = Guid.NewGuid().ToString("N").ToUpperInvariant();
        return guidWithoutDashes[..12];
    }
}
