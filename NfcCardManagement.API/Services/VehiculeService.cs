using AutoMapper;
using NfcCardManagement.API.DTOs.Vehicule;
using NfcCardManagement.API.Exceptions;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Services;

public class VehiculeService : IVehiculeService
{
    private readonly IVehiculeRepository _repo;
    private readonly ICTagService _ctagService;
    private readonly IMapper _mapper;
    private readonly ILogger<VehiculeService> _logger;

    public VehiculeService(
        IVehiculeRepository repo, ICTagService ctagService,
        IMapper mapper, ILogger<VehiculeService> logger)
    {
        _repo = repo; _ctagService = ctagService;
        _mapper = mapper; _logger = logger;
    }

    public async Task<IEnumerable<VehiculeListDto>> GetAllAsync()
        => _mapper.Map<IEnumerable<VehiculeListDto>>(await _repo.GetAllAsync());

    public async Task<VehiculeDetailDto> GetByIdAsync(string id)
    {
        var v = await _repo.GetByIdAsync(id) ?? throw new NotFoundException($"Véhicule '{id}' non trouvé.");
        return _mapper.Map<VehiculeDetailDto>(v);
    }

    public async Task<IEnumerable<VehiculeListDto>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return await GetAllAsync();
        return _mapper.Map<IEnumerable<VehiculeListDto>>(await _repo.SearchAsync(keyword));
    }

    public async Task<string> GenerateCTagAsync(string id)
    {
        var v = await _repo.GetByIdAsync(id) ?? throw new NotFoundException($"Véhicule '{id}' non trouvé.");
        if (v.CTag != null) throw new ConflictException("Un CTag existe déjà pour ce véhicule.");
        var ctag = await _ctagService.GenerateUniqueCTagAsync();
        await _repo.UpdateCTagAsync(id, ctag);
        _logger.LogInformation("CTag généré pour le véhicule {Id}.", id);
        return ctag;
    }

    public async Task UpdateCTagAsync(string id, string ctag)
    {
        var v = await _repo.GetByIdAsync(id) ?? throw new NotFoundException($"Véhicule '{id}' non trouvé.");
        if (v.CTag != null) throw new ConflictException("Un CTag existe déjà pour ce véhicule.");
        await _repo.UpdateCTagAsync(id, ctag);
        _logger.LogInformation("CTag mis à jour pour le véhicule {Id}.", id);
    }
}
