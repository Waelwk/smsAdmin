using FluentValidation;
using NfcCardManagement.API.DTOs.Carte;

namespace NfcCardManagement.API.Validators;

/// <summary>Validator FluentValidation pour la requête de carte véhicule.</summary>
public class CarteVehiculeRequestValidator : AbstractValidator<CarteVehiculeRequestDto>
{
    public CarteVehiculeRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotNull().WithMessage("L'identifiant véhicule est requis.")
            .NotEmpty().WithMessage("L'identifiant véhicule ne peut pas être vide.");
    }
}
