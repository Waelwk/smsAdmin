using Moq;
using NfcCardManagement.API.Repositories.Interfaces;
using NfcCardManagement.API.Services;
using Xunit;

namespace NfcCardManagement.Tests.Services;

public class CTagServiceTests
{
    private readonly Mock<IEmployeRepository> _empRepo = new();
    private readonly Mock<IVehiculeRepository> _vehRepo = new();

    private CTagService CreateService() => new(_empRepo.Object, _vehRepo.Object);

    [Fact]
    public async Task IsCTagUnique_NotInEitherTable_ReturnsTrue()
    {
        _empRepo.Setup(r => r.CTagExistsAsync("NEWTAG123456")).ReturnsAsync(false);
        _vehRepo.Setup(r => r.CTagExistsAsync("NEWTAG123456")).ReturnsAsync(false);

        var result = await CreateService().IsCTagUniqueAsync("NEWTAG123456");

        Assert.True(result);
    }

    [Fact]
    public async Task IsCTagUnique_ExistsInEmployeTable_ReturnsFalse()
    {
        _empRepo.Setup(r => r.CTagExistsAsync("EXISTTAG1234")).ReturnsAsync(true);

        var result = await CreateService().IsCTagUniqueAsync("EXISTTAG1234");

        Assert.False(result);
    }

    [Fact]
    public async Task IsCTagUnique_ExistsInVehiculeTable_ReturnsFalse()
    {
        _empRepo.Setup(r => r.CTagExistsAsync("EXISTTAG1234")).ReturnsAsync(false);
        _vehRepo.Setup(r => r.CTagExistsAsync("EXISTTAG1234")).ReturnsAsync(true);

        var result = await CreateService().IsCTagUniqueAsync("EXISTTAG1234");

        Assert.False(result);
    }

    [Fact]
    public async Task GenerateUniqueCTag_WhenFirstAttemptIsUnique_ReturnsValue()
    {
        // Tous les CTags sont uniques dès le premier appel
        _empRepo.Setup(r => r.CTagExistsAsync(It.IsAny<string>())).ReturnsAsync(false);
        _vehRepo.Setup(r => r.CTagExistsAsync(It.IsAny<string>())).ReturnsAsync(false);

        var ctag = await CreateService().GenerateUniqueCTagAsync();

        Assert.NotNull(ctag);
        Assert.Equal(12, ctag.Length);
    }

    [Fact]
    public async Task GenerateUniqueCTag_AllAttemptsConflict_ThrowsInvalidOperation()
    {
        // Tous les CTags sont déjà pris
        _empRepo.Setup(r => r.CTagExistsAsync(It.IsAny<string>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CreateService().GenerateUniqueCTagAsync());
    }
}
