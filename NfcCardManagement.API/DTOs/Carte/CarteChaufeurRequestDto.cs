namespace NfcCardManagement.API.DTOs.Carte;

/// <summary>DTO de requête pour la génération des données NFC d'un chauffeur.</summary>
public class CarteChaufeurRequestDto
{
    /// <summary>Matricule du chauffeur.</summary>
    public string Matricule { get; set; } = null!;
}
