using Lager_N_Tier_arkitektur.Models;
using Microsoft.EntityFrameworkCore;

namespace Lager_N_Tier_arkitektur.Repositories
{
    // =================================================================
    //  DATA LAYER  -  REPOSITORY IMPLEMENTATION
    // -----------------------------------------------------------------
    //  The ONLY class that uses the DbContext for students. All the
    //  _context code that used to be in StudentsController lives here.
    //
    //  Responsible for:     queries, Add/Update/Remove, SaveChangesAsync
    //  NOT responsible for: validation, business rules, HTTP, DTOs
    // =================================================================
    public class StudentRepository : IStudentRepository
    {
        private readonly Lager_N_Tier_arkitekturContext _context;

        public StudentRepository(Lager_N_Tier_arkitekturContext context)
        {
            _context = context;
        }

        public async Task<List<Students>> GetAllAsync()
        {
            return await _context.Students.AsNoTracking().ToListAsync();
        }

        public async Task<Students?> GetByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }

        public async Task<Students> AddAsync(Students student)
        {
            _context.Students.Add(student);
            await _context.SaveChangesAsync();
            return student; // Id has been set by the database
        }

        public async Task<bool> UpdateAsync(Students student)
        {
            var existing = await _context.Students.FindAsync(student.Id);
            if (existing == null)
            {
                return false;
            }

            // Copy the new values onto the tracked entity, then save.
            _context.Entry(existing).CurrentValues.SetValues(student);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return false;
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
