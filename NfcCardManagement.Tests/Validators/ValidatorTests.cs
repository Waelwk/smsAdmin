using FluentValidation.TestHelper;
using NfcCardManagement.API.DTOs.Carte;
using NfcCardManagement.API.DTOs.Employe;
using NfcCardManagement.API.Validators;
using Xunit;

namespace NfcCardManagement.Tests.Validators;

public class CTagUpdateValidatorTests
{
    private readonly CTagUpdateValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void CTag_InvalidValues_ShouldHaveError(string? value)
    {
        var result = _validator.TestValidate(new CTagUpdateDto { CTag = value! });
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void CTag_ValidValue_ShouldNotHaveError()
    {
        var result = _validator.TestValidate(new CTagUpdateDto { CTag = "ABC123DEF456" });
        result.ShouldNotHaveAnyValidationErrors();
    }
}

public class CarteChaufeurRequestValidatorTests
{
    private readonly CarteChaufeurRequestValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Matricule_InvalidValues_ShouldHaveError(string? value)
    {
        var result = _validator.TestValidate(new CarteChaufeurRequestDto { Matricule = value! });
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Matricule_ValidValue_ShouldNotHaveError()
    {
        var result = _validator.TestValidate(new CarteChaufeurRequestDto { Matricule = "M001" });
        result.ShouldNotHaveAnyValidationErrors();
    }
}

public class CarteVehiculeRequestValidatorTests
{
    private readonly CarteVehiculeRequestValidator _validator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Id_InvalidValues_ShouldHaveError(string? value)
    {
        var result = _validator.TestValidate(new CarteVehiculeRequestDto { Id = value! });
        result.ShouldHaveAnyValidationError();
    }

    [Fact]
    public void Id_ValidValue_ShouldNotHaveError()
    {
        var result = _validator.TestValidate(new CarteVehiculeRequestDto { Id = "V001" });
        result.ShouldNotHaveAnyValidationErrors();
    }
}
