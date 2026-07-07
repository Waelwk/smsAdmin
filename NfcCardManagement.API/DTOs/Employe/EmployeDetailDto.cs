namespace NfcCardManagement.API.DTOs.Employe;

/// <summary>DTO détail pour un employé/chauffeur (MotDePasse jamais exposé).</summary>
public class EmployeDetailDto
{
    public string Matricule { get; set; } = null!;
    public string? NomPrenom { get; set; }
    public decimal? ChargeParHeure { get; set; }
    public bool? BActif { get; set; }
    public string? TypeEmp { get; set; }
    public bool? BResponsable { get; set; }
    public string? CEquipe { get; set; }
    public string? TagC { get; set; }
    public string? CPosteEmployer { get; set; }
    public string? CSociete { get; set; }
    public string? CSite { get; set; }
    public bool? Banned { get; set; }
    public bool HasPassword { get; set; }
    public bool HasCTag { get; set; }
}
