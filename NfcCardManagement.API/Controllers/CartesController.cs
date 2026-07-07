using Microsoft.AspNetCore.Mvc;
using NfcCardManagement.API.DTOs.Carte;
using NfcCardManagement.API.DTOs.Common;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Controllers;

/// <summary>
/// Génération des données NFC à écrire sur les cartes physiques.
/// </summary>
[ApiController]
[Route("api/cartes")]
[Produces("application/json")]
[Tags("Cartes NFC")]
public class CartesController : ControllerBase
{
    private readonly ICarteService _service;

    public CartesController(ICarteService service) => _service = service;

    /// <summary>Obtenir les données NFC d'une carte chauffeur</summary>
    /// <remarks>
    /// Retourne les deux records à écrire sur la carte NFC physique du chauffeur.
    /// Le chauffeur doit avoir un CTag **et** un mot de passe avant d'appeler cet endpoint.
    ///
    ///     POST /api/cartes/chauffeurs
    ///     {
    ///         "matricule": "M001"
    ///     }
    ///
    /// Réponse :
    ///
    ///     {
    ///         "success": true,
    ///         "message": "Données NFC chauffeur générées.",
    ///         "data": {
    ///             "record1": "ABC123DEF456",
    ///             "record2": "Pass1234"
    ///         },
    ///         "errors": []
    ///     }
    ///
    /// </remarks>
    /// <param name="dto">Corps de la requête contenant le matricule</param>
    /// <response code="200">Données NFC retournées — record1=CTag, record2=MotDePasse</response>
    /// <response code="400">Matricule vide ou invalide</response>
    /// <response code="404">Chauffeur non trouvé</response>
    /// <response code="422">CTag ou mot de passe manquant — génération requise avant</response>
    [HttpPost("chauffeurs")]
    [ProducesResponseType(typeof(ApiResponse<NfcDataDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 422)]
    public async Task<IActionResult> GetCarteChauffeur([FromBody] CarteChaufeurRequestDto dto)
    {
        var data = await _service.GetCarteChaufeurAsync(dto.Matricule);
        return Ok(ApiResponse<NfcDataDto>.Ok(data, "Données NFC chauffeur générées."));
    }

    /// <summary>Obtenir les données NFC d'une carte véhicule</summary>
    /// <remarks>
    /// Retourne le record à écrire sur la carte NFC physique du véhicule.
    /// Le véhicule doit avoir un CTag avant d'appeler cet endpoint.
    ///
    ///     POST /api/cartes/vehicules
    ///     {
    ///         "id": "V001"
    ///     }
    ///
    /// Réponse :
    ///
    ///     {
    ///         "success": true,
    ///         "message": "Données NFC véhicule générées.",
    ///         "data": {
    ///             "record1": "AABBCC112233",
    ///             "record2": null
    ///         },
    ///         "errors": []
    ///     }
    ///
    /// </remarks>
    /// <param name="dto">Corps de la requête contenant l'identifiant véhicule</param>
    /// <response code="200">Données NFC retournées — record1=CTag, record2=null</response>
    /// <response code="400">Identifiant vide ou invalide</response>
    /// <response code="404">Véhicule non trouvé</response>
    /// <response code="422">CTag manquant — génération requise avant</response>
    [HttpPost("vehicules")]
    [ProducesResponseType(typeof(ApiResponse<NfcDataDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 422)]
    public async Task<IActionResult> GetCarteVehicule([FromBody] CarteVehiculeRequestDto dto)
    {
        var data = await _service.GetCarteVehiculeAsync(dto.Id);
        return Ok(ApiResponse<NfcDataDto>.Ok(data, "Données NFC véhicule générées."));
    }
}
