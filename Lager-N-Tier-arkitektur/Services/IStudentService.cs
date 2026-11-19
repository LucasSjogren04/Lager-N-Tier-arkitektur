using Lager_N_Tier_arkitektur.Dtos;

namespace Lager_N_Tier_arkitektur.Services
{
    // =================================================================
    //  BUSINESS LAYER (Affärslogiklager)  -  SERVICE INTERFACE
    // -----------------------------------------------------------------
    //  The contract the controller depends on. It only speaks in DTOs,
    //  so the controller never sees the entity, repository or DbContext.
    // =================================================================
    public interface IStudentService
    {
        Task<List<StudentDto>> GetAllAsync();
        Task<StudentDto?> GetByIdAsync(int id);
        Task<StudentDto> CreateAsync(SaveStudentDto dto);
        Task<bool> UpdateAsync(int id, SaveStudentDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
