using AutoMapper;
using NfcCardManagement.API.DTOs.Employe;
using NfcCardManagement.API.Exceptions;
using NfcCardManagement.API.Helpers;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services.Interfaces;

namespace NfcCardManagement.API.Services;

/// <summary>Implémentation du service métier pour les chauffeurs.</summary>
public class EmployeService : IEmployeService
{
    private readonly IEmployeRepository _repo;
    private readonly ICTagService _ctagService;
    private readonly IMapper _mapper;
    private readonly ILogger<EmployeService> _logger;

    public EmployeService(
        IEmployeRepository repo,
        ICTagService ctagService,
        IMapper mapper,
        ILogger<EmployeService> logger)
    {
        _repo = repo;
        _ctagService = ctagService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<IEnumerable<EmployeListDto>> GetAllAsync()
    {
        var employes = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<EmployeListDto>>(employes);
    }

    public async Task<EmployeDetailDto> GetByMatriculeAsync(string matricule)
    {
        var employe = await _repo.GetByMatriculeAsync(matricule)
            ?? throw new NotFoundException($"Chauffeur '{matricule}' non trouvé.");
        return _mapper.Map<EmployeDetailDto>(employe);
    }

    public async Task<IEnumerable<EmployeListDto>> SearchAsync(string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword))
            return await GetAllAsync();

        var employes = await _repo.SearchAsync(keyword);
        return _mapper.Map<IEnumerable<EmployeListDto>>(employes);
    }

    public async Task<string> GeneratePasswordAsync(string matricule)
    {
        var employe = await _repo.GetByMatriculeAsync(matricule)
            ?? throw new NotFoundException($"Chauffeur '{matricule}' non trouvé.");

        if (employe.MotDePasseC != null)
            throw new ConflictException("Un mot de passe existe déjà pour ce chauffeur.");

        var password = PasswordHelper.Generate();
        await _repo.UpdatePasswordAsync(matricule, password);

        _logger.LogInformation("Mot de passe généré pour le chauffeur {Matricule} à {Time}.",
            matricule, DateTime.UtcNow);

        return password;
    }

    public async Task<string> GenerateCTagAsync(string matricule)
    {
        var employe = await _repo.GetByMatriculeAsync(matricule)
            ?? throw new NotFoundException($"Chauffeur '{matricule}' non trouvé.");

        if (employe.TagC != null)
            throw new ConflictException("Un CTag existe déjà pour ce chauffeur.");

        var ctag = await _ctagService.GenerateUniqueCTagAsync();
        await _repo.UpdateCTagAsync(matricule, ctag);

        _logger.LogInformation("CTag généré pour le chauffeur {Matricule} à {Time}.",
            matricule, DateTime.UtcNow);

        return ctag;
    }

    public async Task UpdateCTagAsync(string matricule, string ctag)
    {
        var employe = await _repo.GetByMatriculeAsync(matricule)
            ?? throw new NotFoundException($"Chauffeur '{matricule}' non trouvé.");

        if (employe.TagC != null)
            throw new ConflictException("Un CTag existe déjà pour ce chauffeur.");

        await _repo.UpdateCTagAsync(matricule, ctag);

        _logger.LogInformation("CTag mis à jour manuellement pour le chauffeur {Matricule} à {Time}.",
            matricule, DateTime.UtcNow);
    }
}
