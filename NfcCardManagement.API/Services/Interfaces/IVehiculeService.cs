using NfcCardManagement.API.DTOs.Vehicule;

namespace NfcCardManagement.API.Services.Interfaces;

public interface IVehiculeService
{
    Task<IEnumerable<VehiculeListDto>> GetAllAsync();
    Task<VehiculeDetailDto> GetByIdAsync(string id);
    Task<IEnumerable<VehiculeListDto>> SearchAsync(string keyword);
    Task<string> GenerateCTagAsync(string id);
    Task UpdateCTagAsync(string id, string ctag);
}
