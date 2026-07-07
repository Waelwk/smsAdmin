namespace NfcCardManagement.API.DTOs.Carte;

/// <summary>Données NFC à écrire sur une carte physique.</summary>
public class NfcDataDto
{
    /// <summary>Record 1 : CTag NFC.</summary>
    public string Record1 { get; set; } = null!;

    /// <summary>Record 2 : Mot de passe en clair (chauffeur uniquement, null pour véhicule).</summary>
    public string? Record2 { get; set; }
}
