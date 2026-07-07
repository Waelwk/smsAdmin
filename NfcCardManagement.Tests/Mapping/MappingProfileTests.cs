using AutoMapper;
using NfcCardManagement.API.Mapping;
using NfcCardManagement.API.Models;
using Xunit;

namespace NfcCardManagement.Tests.Mapping;

public class MappingProfileTests
{
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        var config = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
        _mapper = config.CreateMapper();
    }

    [Fact]
    public void Configuration_IsValid()
    {
        // Vérifie que tous les mappings AutoMapper sont configurés correctement
        _mapper.ConfigurationProvider.AssertConfigurationIsValid();
    }

    [Fact]
    public void GpEmploye_ToListDto_HasPassword_True_WhenMotDePasseNotNull()
    {
        var emp = BuildEmploye(motDePasse: "secret", tagC: null);
        var dto = _mapper.Map<API.DTOs.Employe.EmployeListDto>(emp);
        Assert.True(dto.HasPassword);
        Assert.False(dto.HasCTag);
    }

    [Fact]
    public void GpEmploye_ToListDto_HasPassword_False_WhenMotDePasseNull()
    {
        var emp = BuildEmploye(motDePasse: null, tagC: null);
        var dto = _mapper.Map<API.DTOs.Employe.EmployeListDto>(emp);
        Assert.False(dto.HasPassword);
    }

    [Fact]
    public void GpEmploye_ToListDto_HasCTag_True_WhenTagCNotNull()
    {
        var emp = BuildEmploye(motDePasse: null, tagC: "ABC123DEF456");
        var dto = _mapper.Map<API.DTOs.Employe.EmployeListDto>(emp);
        Assert.True(dto.HasCTag);
    }

    [Fact]
    public void GpEmploye_ToDetailDto_DoesNotExposeMotDePasse()
    {
        var emp = BuildEmploye(motDePasse: "secret123", tagC: "ABC123DEF456");
        var dto = _mapper.Map<API.DTOs.Employe.EmployeDetailDto>(emp);

        // MotDePasseC ne doit pas apparaître dans les champs du DTO
        var props = typeof(API.DTOs.Employe.EmployeDetailDto).GetProperties();
        foreach (var prop in props)
        {
            var val = prop.GetValue(dto)?.ToString();
            Assert.False(val == "secret123",
                $"MotDePasseC exposé dans la propriété {prop.Name} du DTO détail");
        }
    }

    [Fact]
    public void RefVehicule_ToListDto_HasCTag_True_WhenCTagNotNull()
    {
        var v = BuildVehicule(ctag: "AABBCC112233");
        var dto = _mapper.Map<API.DTOs.Vehicule.VehiculeListDto>(v);
        Assert.True(dto.HasCTag);
    }

    [Fact]
    public void RefVehicule_ToListDto_HasCTag_False_WhenCTagNull()
    {
        var v = BuildVehicule(ctag: null);
        var dto = _mapper.Map<API.DTOs.Vehicule.VehiculeListDto>(v);
        Assert.False(dto.HasCTag);
    }

    // ── helpers ──────────────────────────────────────────────────────────────

    private static GpEmploye BuildEmploye(string? motDePasse, string? tagC) => new()
    {
        Matricule = "M001",
        Nom = "Dupont",
        Prenom = "Jean",
        MotDePasseC = motDePasse,
        TagC = tagC,
        TypeEmpC = "T",
        Equipe = "E1",
        PosteEmployerC = "P1",
        SocieteC = "S1",
        SiteC = "ST1"
    };

    private static RefVehicule BuildVehicule(string? ctag) => new()
    {
        CVehicule = "V001",
        LibVehicule = "Camion",
        NumeroSerie = "SN001",
        CTag = ctag
    };
}
