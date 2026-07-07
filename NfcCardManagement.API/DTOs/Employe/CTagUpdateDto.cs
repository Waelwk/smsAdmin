namespace NfcCardManagement.API.DTOs.Employe;

/// <summary>DTO pour la mise à jour manuelle du CTag d'un employé.</summary>
public class CTagUpdateDto
{
    /// <summary>Valeur du CTag NFC (non vide).</summary>
    public string CTag { get; set; } = null!;
}
