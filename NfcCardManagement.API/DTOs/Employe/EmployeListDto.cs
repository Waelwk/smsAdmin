namespace NfcCardManagement.API.DTOs.Employe;

/// <summary>DTO de liste pour un employé/chauffeur.</summary>
public class EmployeListDto
{
    public string Matricule { get; set; } = null!;
    public string? NomPrenom { get; set; }
    public bool? BActif { get; set; }
    /// <summary>True si MotDePasse est non null.</summary>
    public bool HasPassword { get; set; }
    /// <summary>True si CTag est non null.</summary>
    public bool HasCTag { get; set; }
}
