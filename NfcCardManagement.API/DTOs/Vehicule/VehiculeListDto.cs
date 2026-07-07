namespace NfcCardManagement.API.DTOs.Vehicule;

public class VehiculeListDto
{
    public string CVehicule { get; set; } = null!;
    public string? LibVehicule { get; set; }
    public string? NumeroSerie { get; set; }
    public bool? BActif { get; set; }
    public bool HasCTag { get; set; }
}
