using Moq;
using NfcCardManagement.API.Exceptions;
using NfcCardManagement.API.Models;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services;
using Xunit;

namespace NfcCardManagement.Tests.Services;

public class CarteServiceTests
{
    private readonly Mock<IEmployeRepository> _empRepo = new();
    private readonly Mock<IVehiculeRepository> _vehRepo = new();

    private CarteService CreateService() => new(_empRepo.Object, _vehRepo.Object);

    // ── GetCarteChaufeurAsync ─────────────────────────────────────────────────

    [Fact]
    public async Task GetCarteChaufeur_ValidChauffeur_ReturnsCorrectNfcData()
    {
        _empRepo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(new GpEmploye
            {
                Matricule = "M001", Nom = "A", Prenom = "B",
                TagC = "ABC123DEF456", MotDePasseC = "pass1234",
                TypeEmpC = "", Equipe = "", PosteEmployerC = "", SocieteC = "", SiteC = ""
            });

        var svc = CreateService();
        var result = await svc.GetCarteChaufeurAsync("M001");

        Assert.Equal("ABC123DEF456", result.Record1);
        Assert.Equal("pass1234", result.Record2);
    }

    [Fact]
    public async Task GetCarteChaufeur_NoTagC_Throws422()
    {
        _empRepo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(new GpEmploye
            {
                Matricule = "M001", Nom = "A", Prenom = "B",
                TagC = null, MotDePasseC = "pass1234",
                TypeEmpC = "", Equipe = "", PosteEmployerC = "", SocieteC = "", SiteC = ""
            });

        var svc = CreateService();
        await Assert.ThrowsAsync<UnprocessableEntityException>(() =>
            svc.GetCarteChaufeurAsync("M001"));
    }

    [Fact]
    public async Task GetCarteChaufeur_NoMotDePasse_Throws422()
    {
        _empRepo.Setup(r => r.GetByMatriculeAsync("M001"))
            .ReturnsAsync(new GpEmploye
            {
                Matricule = "M001", Nom = "A", Prenom = "B",
                TagC = "ABC123DEF456", MotDePasseC = null,
                TypeEmpC = "", Equipe = "", PosteEmployerC = "", SocieteC = "", SiteC = ""
            });

        var svc = CreateService();
        await Assert.ThrowsAsync<UnprocessableEntityException>(() =>
            svc.GetCarteChaufeurAsync("M001"));
    }

    [Fact]
    public async Task GetCarteChaufeur_NotFound_Throws404()
    {
        _empRepo.Setup(r => r.GetByMatriculeAsync("NOTEXIST"))
            .ReturnsAsync((GpEmploye?)null);

        var svc = CreateService();
        await Assert.ThrowsAsync<NotFoundException>(() =>
            svc.GetCarteChaufeurAsync("NOTEXIST"));
    }

    // ── GetCarteVehiculeAsync ────────────────────────────────────────────────

    [Fact]
    public async Task GetCarteVehicule_ValidVehicule_ReturnsRecord1AndNullRecord2()
    {
        _vehRepo.Setup(r => r.GetByIdAsync("V001"))
            .ReturnsAsync(new RefVehicule
            {
                CVehicule = "V001", LibVehicule = "Camion",
                NumeroSerie = "SN1", CTag = "AABBCC112233"
            });

        var svc = CreateService();
        var result = await svc.GetCarteVehiculeAsync("V001");

        Assert.Equal("AABBCC112233", result.Record1);
        Assert.Null(result.Record2);
    }

    [Fact]
    public async Task GetCarteVehicule_NoCTag_Throws422()
    {
        _vehRepo.Setup(r => r.GetByIdAsync("V001"))
            .ReturnsAsync(new RefVehicule
            {
                CVehicule = "V001", LibVehicule = "Camion",
                NumeroSerie = "SN1", CTag = null
            });

        var svc = CreateService();
        await Assert.ThrowsAsync<UnprocessableEntityException>(() =>
            svc.GetCarteVehiculeAsync("V001"));
    }

    [Fact]
    public async Task GetCarteVehicule_NotFound_Throws404()
    {
        _vehRepo.Setup(r => r.GetByIdAsync("NOTEXIST"))
            .ReturnsAsync((RefVehicule?)null);

        var svc = CreateService();
        await Assert.ThrowsAsync<NotFoundException>(() =>
            svc.GetCarteVehiculeAsync("NOTEXIST"));
    }
}
