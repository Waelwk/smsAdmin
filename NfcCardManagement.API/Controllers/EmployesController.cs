using Microsoft.AspNetCore.Mvc;
using NfcCardManagement.API.DTOs.Common;
using NfcCardManagement.API.DTOs.Employe;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Controllers;

/// <summary>
/// Gestion des chauffeurs — consultation, mot de passe, CTag NFC.
/// </summary>
[ApiController]
[Route("api/employes")]
[Produces("application/json")]
[Tags("Chauffeurs")]
public class EmployesController : ControllerBase
{
    private readonly IEmployeService _service;

    public EmployesController(IEmployeService service) => _service = service;

    /// <summary>Liste tous les chauffeurs</summary>
    /// <remarks>
    /// Retourne l'ensemble des chauffeurs de la table GP_Employe avec les indicateurs
    /// <c>hasPassword</c> et <c>hasCTag</c>.
    ///
    ///     GET /api/employes
    ///
    /// </remarks>
    /// <response code="200">Liste retournée avec succès</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeListDto>>), 200)]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<EmployeListDto>>.Ok(data, "Liste des chauffeurs récupérée."));
    }

    /// <summary>Détails d'un chauffeur par matricule</summary>
    /// <remarks>
    ///     GET /api/employes/M001
    /// </remarks>
    /// <param name="matricule">Matricule du chauffeur (ex: M001)</param>
    /// <response code="200">Chauffeur trouvé</response>
    /// <response code="404">Matricule inexistant</response>
    [HttpGet("{matricule}")]
    [ProducesResponseType(typeof(ApiResponse<EmployeDetailDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<IActionResult> GetById(string matricule)
    {
        var data = await _service.GetByMatriculeAsync(matricule);
        return Ok(ApiResponse<EmployeDetailDto>.Ok(data));
    }

    /// <summary>Rechercher des chauffeurs par mot-clé</summary>
    /// <remarks>
    /// Filtre sur Matricule et Nom+Prénom (insensible à la casse).
    /// Un mot-clé vide retourne la liste complète.
    ///
    ///     GET /api/employes/search?keyword=dupont
    ///
    /// </remarks>
    /// <param name="keyword">Mot-clé de recherche (optionnel)</param>
    /// <response code="200">Résultats de recherche</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EmployeListDto>>), 200)]
    public async Task<IActionResult> Search([FromQuery] string keyword = "")
    {
        var data = await _service.SearchAsync(keyword);
        return Ok(ApiResponse<IEnumerable<EmployeListDto>>.Ok(data, "Recherche effectuée."));
    }

    /// <summary>Générer un mot de passe pour un chauffeur</summary>
    /// <remarks>
    /// Génère un mot de passe alphanumérique aléatoire (8–12 caractères) et le stocke en clair.
    /// Retourne le mot de passe en clair dans <c>data</c>.
    ///
    ///     POST /api/employes/M001/generate-password
    ///
    /// </remarks>
    /// <param name="matricule">Matricule du chauffeur</param>
    /// <response code="200">Mot de passe généré — retourné dans data</response>
    /// <response code="404">Matricule inexistant</response>
    /// <response code="409">Un mot de passe existe déjà pour ce chauffeur</response>
    [HttpPost("{matricule}/generate-password")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 409)]
    public async Task<IActionResult> GeneratePassword(string matricule)
    {
        var password = await _service.GeneratePasswordAsync(matricule);
        return Ok(ApiResponse<string>.Ok(password, "Mot de passe généré avec succès."));
    }

    /// <summary>Générer automatiquement un CTag pour un chauffeur</summary>
    /// <remarks>
    /// Génère un identifiant NFC unique (12 caractères hex) et le stocke dans GP_Employe.TagC.
    /// L'unicité est garantie sur GP_Employe.TagC **et** Ref_Vehicule.CTag.
    ///
    ///     POST /api/employes/M001/generate-ctag
    ///
    /// </remarks>
    /// <param name="matricule">Matricule du chauffeur</param>
    /// <response code="200">CTag généré — retourné dans data</response>
    /// <response code="404">Matricule inexistant</response>
    /// <response code="409">Un CTag existe déjà pour ce chauffeur</response>
    [HttpPost("{matricule}/generate-ctag")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 409)]
    public async Task<IActionResult> GenerateCTag(string matricule)
    {
        var ctag = await _service.GenerateCTagAsync(matricule);
        return Ok(ApiResponse<string>.Ok(ctag, "CTag généré avec succès."));
    }

    /// <summary>Assigner manuellement un CTag à un chauffeur</summary>
    /// <remarks>
    /// Permet de saisir un CTag existant (lu depuis une carte physique par exemple).
    ///
    ///     PUT /api/employes/M001/ctag
    ///     {
    ///         "ctag": "ABC123DEF456"
    ///     }
    ///
    /// </remarks>
    /// <param name="matricule">Matricule du chauffeur</param>
    /// <param name="dto">Corps de la requête contenant le CTag</param>
    /// <response code="200">CTag mis à jour</response>
    /// <response code="400">CTag vide ou invalide</response>
    /// <response code="404">Matricule inexistant</response>
    /// <response code="409">Un CTag existe déjà pour ce chauffeur</response>
    [HttpPut("{matricule}/ctag")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    [ProducesResponseType(typeof(ApiResponse<object>), 409)]
    public async Task<IActionResult> UpdateCTag(string matricule, [FromBody] CTagUpdateDto dto)
    {
        await _service.UpdateCTagAsync(matricule, dto.CTag);
        return Ok(ApiResponse.OkNoData("CTag mis à jour avec succès."));
    }
}
