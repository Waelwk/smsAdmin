namespace NfcCardManagement.API.DTOs.Vehicule;

public class VehiculeDetailDto
{
    public string CVehicule { get; set; } = null!;
    public string? LibVehicule { get; set; }
    public string? NumeroSerie { get; set; }
    public bool? BActif { get; set; }
    public decimal? ChargeMax { get; set; }
    public bool? BDisponible { get; set; }
    public decimal? ChargeLibre { get; set; }
    public decimal? CoutparKM { get; set; }
    public string? CTag { get; set; }
    public bool HasCTag { get; set; }
}
