using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NfcCardManagement.API.Models;

/// <summary>Représente un véhicule — table Ref_Vehicule.</summary>
[Table("Ref_Vehicule")]
public class RefVehicule
{
    [Key]
    [Column("CVehicule")]
    [StringLength(20)]
    public string CVehicule { get; set; } = null!;

    [Column("LibVehicule")]
    [StringLength(50)]
    public string? LibVehicule { get; set; }

    [Column("NumeroSerie")]
    [StringLength(20)]
    public string? NumeroSerie { get; set; }

    [Column("BActif")]
    public bool? BActif { get; set; }

    [Column("ChargeMax", TypeName = "decimal(18,2)")]
    public decimal? ChargeMax { get; set; }

    [Column("BDisponible")]
    public bool? BDisponible { get; set; }

    [Column("ChargeLibre", TypeName = "decimal(18,2)")]
    public decimal? ChargeLibre { get; set; }

    // int en base (référence vers un employé)
    [Column("CreePar")]
    public int? CreePar { get; set; }

    [Column("ModifiePar")]
    public int? ModifiePar { get; set; }

    [Column("DateInsertion")]
    public DateTime? DateInsertion { get; set; }

    [Column("DateModification")]
    public DateTime? DateModification { get; set; }

    [Column("PCInsertion")]
    [StringLength(100)]
    public string? PCInsertion { get; set; }

    [Column("PCModification")]
    [StringLength(100)]
    public string? PCModification { get; set; }

    [Column("CoutparKM", TypeName = "decimal(18,4)")]
    public decimal? CoutparKM { get; set; }

    /// <summary>Identifiant NFC (CTag). NULL si aucune carte assignée.</summary>
    [Column("CTag")]
    [StringLength(20)]
    public string? CTag { get; set; }
}
