using FluentValidation;
using NfcCardManagement.API.DTOs.Carte;

namespace NfcCardManagement.API.Validators;

/// <summary>Validator FluentValidation pour la requête de carte chauffeur.</summary>
public class CarteChaufeurRequestValidator : AbstractValidator<CarteChaufeurRequestDto>
{
    public CarteChaufeurRequestValidator()
    {
        RuleFor(x => x.Matricule)
            .NotNull().WithMessage("Le matricule est requis.")
            .NotEmpty().WithMessage("Le matricule ne peut pas être vide.");
    }
}
