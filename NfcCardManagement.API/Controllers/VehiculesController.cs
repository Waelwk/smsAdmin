using Microsoft.AspNetCore.Mvc;
using NfcCardManagement.API.DTOs.Common;
using NfcCardManagement.API.DTOs.Vehicule;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Controllers;

[ApiController]
[Route("api/vehicules")]
[Produces("application/json")]
[Tags("Véhicules")]
public class VehiculesController : ControllerBase
{
    private readonly IVehiculeService _service;

    public VehiculesController(IVehiculeService service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var data = await _service.GetAllAsync();
        return Ok(ApiResponse<IEnumerable<VehiculeListDto>>.Ok(data, "Liste des véhicules récupérée."));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var data = await _service.GetByIdAsync(id);
        return Ok(ApiResponse<VehiculeDetailDto>.Ok(data));
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string keyword = "")
    {
        var data = await _service.SearchAsync(keyword);
        return Ok(ApiResponse<IEnumerable<VehiculeListDto>>.Ok(data, "Recherche effectuée."));
    }

    [HttpPost("{id}/generate-ctag")]
    public async Task<IActionResult> GenerateCTag(string id)
    {
        var ctag = await _service.GenerateCTagAsync(id);
        return Ok(ApiResponse<string>.Ok(ctag, "CTag généré avec succès."));
    }

    [HttpPut("{id}/ctag")]
    public async Task<IActionResult> UpdateCTag(string id, [FromBody] CTagUpdateDto dto)
    {
        await _service.UpdateCTagAsync(id, dto.CTag);
        return Ok(ApiResponse.OkNoData("CTag mis à jour avec succès."));
    }
}
