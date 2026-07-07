using System.Security.Cryptography;

namespace NfcCardManagement.API.Helpers;

/// <summary>
/// Fournit des utilitaires de génération de mots de passe pour les chauffeurs.
/// </summary>
public static class PasswordHelper
{
    private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private const int MinLength = 8;
    private const int MaxLength = 12;

    /// <summary>
    /// Génère un mot de passe alphanumérique aléatoire de longueur comprise entre 8 et 12 (inclus).
    /// Utilise <see cref="RandomNumberGenerator"/> pour garantir un tirage cryptographiquement sûr.
    /// Le mot de passe est retourné en clair et doit être stocké tel quel.
    /// </summary>
    /// <returns>
    /// Une chaîne alphanumérique aléatoire dont la longueur appartient à l'intervalle [8, 12].
    /// </returns>
    public static string Generate()
    {
        int length = RandomNumberGenerator.GetInt32(MinLength, MaxLength + 1);

        var chars = new char[length];
        for (int i = 0; i < length; i++)
        {
            chars[i] = Alphabet[RandomNumberGenerator.GetInt32(Alphabet.Length)];
        }

        return new string(chars);
    }
}
