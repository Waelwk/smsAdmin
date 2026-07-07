using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NfcCardManagement.API.Exceptions;
using NfcCardManagement.API.Mapping;
using NfcCardManagement.API.Models;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services;
using NfcCardManagement.API.Services.Interfaces;
using Xunit;

namespace NfcCardManagement.Tests.Services;

public class EmployeServiceTests
{
    private readonly Mock<IEmployeRepository> _repo = new();
    private readonly Mock<ICTagService> _ctagSvc = new();
    private readonly IMapper _mapper;
    private readonly NullLogger<EmployeService> _logger = new();

    public EmployeServiceTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    private EmployeService CreateService() =>
        new(_repo.Object, _ctagSvc.Object, _mapper, _logger);

    private static GpEmploye BuildEmploye(string matricule, string? motDePasse = null, string? tagC = null) =>
        new()
        {
            Matricule = matricule, Nom = "Dupont", Prenom = "Jean",
            MotDePasseC = motDePasse, TagC = tagC,
            TypeEmpC = "", Equipe = "", PosteEmployerC = "", SocieteC = "", SiteC = ""
        };

    // ── GetAllAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAllAsync_ReturnsMappedList()
    {
        _repo.Setup(r => r.GetAllAsync())
            .ReturnsAsync(new[] { BuildEmploye("M001"), BuildEmploye("M002") });

        var result = (await CreateService().GetAllAsync()).ToList();

        Assert.Equal(2, result.Count);
        Assert.Equal("M001", result[0].Matricule);
    }

    // ── GetByMatriculeAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetByMatricule_NotFound_Throws404()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("NOTEXIST")).ReturnsAsync((GpEmploye?)null);
        await Assert.ThrowsAsync<NotFoundException>(() =>
            CreateService().GetByMatriculeAsync("NOTEXIST"));
    }

    [Fact]
    public async Task GetByMatricule_Found_ReturnsDto()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("M001")).ReturnsAsync(BuildEmploye("M001"));
        var dto = await CreateService().GetByMatriculeAsync("M001");
        Assert.Equal("M001", dto.Matricule);
    }

    // ── SearchAsync ──────────────────────────────────────────────────────────

    [Fact]
    public async Task SearchAsync_EmptyKeyword_CallsGetAll()
    {
        _repo.Setup(r => r.GetAllAsync()).ReturnsAsync(Array.Empty<GpEmploye>());
        await CreateService().SearchAsync("");
        _repo.Verify(r => r.GetAllAsync(), Times.Once);
        _repo.Verify(r => r.SearchAsync(It.IsAny<string>()), Times.Never);
    }

    // ── GeneratePasswordAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GeneratePassword_AlreadyHasPassword_Throws409()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(BuildEmploye("M001", motDePasse: "existing"));

        await Assert.ThrowsAsync<ConflictException>(() =>
            CreateService().GeneratePasswordAsync("M001"));
    }

    [Fact]
    public async Task GeneratePassword_NoPassword_ReturnsAlphanumericString()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(BuildEmploye("M001", motDePasse: null));
        _repo.Setup(r => r.UpdatePasswordAsync(It.IsAny<string>(), It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        var pwd = await CreateService().GeneratePasswordAsync("M001");

        Assert.InRange(pwd.Length, 8, 12);
        Assert.All(pwd, c => Assert.True(char.IsLetterOrDigit(c)));
    }

    // ── GenerateCTagAsync ────────────────────────────────────────────────────

    [Fact]
    public async Task GenerateCTag_AlreadyHasCTag_Throws409()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(BuildEmploye("M001", tagC: "EXISTINGTAG1"));

        await Assert.ThrowsAsync<ConflictException>(() =>
            CreateService().GenerateCTagAsync("M001"));
    }

    [Fact]
    public async Task GenerateCTag_NoCTag_StoresAndReturnsCtag()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(BuildEmploye("M001", tagC: null));
        _ctagSvc.Setup(s => s.GenerateUniqueCTagAsync()).ReturnsAsync("NEWCTAG12345");
        _repo.Setup(r => r.UpdateCTagAsync("M001", "NEWCTAG12345")).Returns(Task.CompletedTask);

        var ctag = await CreateService().GenerateCTagAsync("M001");

        Assert.Equal("NEWCTAG12345", ctag);
    }

    // ── UpdateCTagAsync ──────────────────────────────────────────────────────

    [Fact]
    public async Task UpdateCTag_AlreadyHasCTag_Throws409()
    {
        _repo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(BuildEmploye("M001", tagC: "EXISTINGTAG1"));

        await Assert.ThrowsAsync<ConflictException>(() =>
            CreateService().UpdateCTagAsync("M001", "NEWVALUE1234"));
    }
}
