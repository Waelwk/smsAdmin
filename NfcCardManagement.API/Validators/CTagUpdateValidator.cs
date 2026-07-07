using FluentValidation;
using NfcCardManagement.API.DTOs.Employe;

namespace NfcCardManagement.API.Validators;

/// <summary>Validator FluentValidation pour la mise à jour manuelle du CTag.</summary>
public class CTagUpdateValidator : AbstractValidator<CTagUpdateDto>
{
    public CTagUpdateValidator()
    {
        RuleFor(x => x.CTag)
            .NotNull().WithMessage("Le CTag est requis.")
            .NotEmpty().WithMessage("Le CTag ne peut pas être vide.")
            .Must(v => !string.IsNullOrWhiteSpace(v)).WithMessage("Le CTag ne peut pas être composé uniquement d'espaces.");
    }
}

/// <summary>Validator FluentValidation pour la mise à jour manuelle du CTag d'un véhicule.</summary>
public class VehiculeCTagUpdateValidator : AbstractValidator<DTOs.Vehicule.CTagUpdateDto>
{
    public VehiculeCTagUpdateValidator()
    {
        RuleFor(x => x.CTag)
            .NotNull().WithMessage("Le CTag est requis.")
            .NotEmpty().WithMessage("Le CTag ne peut pas être vide.")
            .Must(v => !string.IsNullOrWhiteSpace(v)).WithMessage("Le CTag ne peut pas être composé uniquement d'espaces.");
    }
}
