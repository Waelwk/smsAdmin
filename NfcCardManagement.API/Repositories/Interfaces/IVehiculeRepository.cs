using NfcCardManagement.API.Models;

namespace NfcCardManagement.API.Repositories.Interfaces;

public interface IVehiculeRepository
{
    Task<IEnumerable<RefVehicule>> GetAllAsync();
    Task<RefVehicule?> GetByIdAsync(string id);
    Task<IEnumerable<RefVehicule>> SearchAsync(string keyword);
    Task UpdateCTagAsync(string id, string ctag);
    Task<bool> CTagExistsAsync(string ctag);
}
