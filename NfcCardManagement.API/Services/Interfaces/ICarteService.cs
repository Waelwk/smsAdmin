using NfcCardManagement.API.DTOs.Carte;

namespace NfcCardManagement.API.Services.Interfaces;

/// <summary>Service de génération des données NFC pour les cartes chauffeur et véhicule.</summary>
public interface ICarteService
{
    /// <summary>Retourne les données NFC (Record1=CTag, Record2=MotDePasse) pour un chauffeur.</summary>
    Task<NfcDataDto> GetCarteChaufeurAsync(string matricule);

    /// <summary>Retourne les données NFC (Record1=CTag) pour un véhicule.</summary>
    Task<NfcDataDto> GetCarteVehiculeAsync(string id);
}
