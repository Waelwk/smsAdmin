namespace NfcCardManagement.API.DTOs.Carte;

/// <summary>DTO de requête pour la génération des données NFC d'un véhicule.</summary>
public class CarteVehiculeRequestDto
{
    /// <summary>Identifiant du véhicule (CVehicule).</summary>
    public string Id { get; set; } = null!;
}
