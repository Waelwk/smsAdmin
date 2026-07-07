using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NfcCardManagement.API.Models;

/// <summary>Représente un chauffeur/employé — table GP_Employe.</summary>
[Table("GP_Employe")]
public class GpEmploye
{
    [Key]
    [Column("Matricule")]
    public string Matricule { get; set; } = null!;          // varchar

    [Column("NomPrenom")]
    public string? NomPrenom { get; set; }                  // varchar

    [Column("ChargeParHeure", TypeName = "decimal(18,2)")]
    public decimal? ChargeParHeure { get; set; }            // decimal

    [Column("BActif")]
    public bool? BActif { get; set; }                       // bit

    [Column("TypeEmp")]
    public string? TypeEmp { get; set; }                    // varchar

    [Column("CreePar")]
    public int? CreePar { get; set; }                       // int

    [Column("ModifiePar")]
    public int? ModifiePar { get; set; }                    // int

    [Column("DateInsertion")]
    public DateTime? DateInsertion { get; set; }            // datetime

    [Column("DateModification")]
    public DateTime? DateModification { get; set; }         // datetime

    [Column("PCInsertion")]
    public string? PCInsertion { get; set; }                // varchar

    [Column("PCModification")]
    public string? PCModification { get; set; }             // varchar

    [Column("BResponsable")]
    public bool? BResponsable { get; set; }                 // bit

    [Column("CEquipe")]
    public string? CEquipe { get; set; }                    // varchar

    /// <summary>Mot de passe en clair. NULL si non généré. Ne jamais logger.</summary>
    [Column("MotDePasse")]
    public string? MotDePasseC { get; set; }                // nvarchar

    /// <summary>Identifiant NFC. NULL si aucune carte assignée.</summary>
    [Column("CTag")]
    public string? TagC { get; set; }                       // varchar

    [Column("CPosteEmployer")]
    public string? CPosteEmployer { get; set; }             // varchar

    [Column("CSociete")]
    public string? CSociete { get; set; }                   // varchar

    [Column("CSite")]
    public string? CSite { get; set; }                      // varchar

    [Column("Banned")]
    public bool? Banned { get; set; }                       // bit
}
