using Lager_N_Tier_arkitektur.Models;

namespace Lager_N_Tier_arkitektur.Repositories
{
    // =================================================================
    //  DATA LAYER (Dataåtkomstlager)  -  REPOSITORY INTERFACE
    // -----------------------------------------------------------------
    //  The contract the Business layer uses to read/write students.
    //  It says WHAT can be done with the data, not HOW (EF Core, SQL...).
    //  Only CRUD here - no business rules.
    // =================================================================
    public interface IStudentRepository
    {
        Task<List<Students>> GetAllAsync();
        Task<Students?> GetByIdAsync(int id);
        Task<Students> AddAsync(Students student);
        Task<bool> UpdateAsync(Students student);
        Task<bool> DeleteAsync(int id);
    }
}
