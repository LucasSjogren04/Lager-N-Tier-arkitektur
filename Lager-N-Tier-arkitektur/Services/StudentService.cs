using Lager_N_Tier_arkitektur.Dtos;
using Lager_N_Tier_arkitektur.Models;
using Lager_N_Tier_arkitektur.Repositories;

namespace Lager_N_Tier_arkitektur.Services
{
    // =================================================================
    //  BUSINESS LAYER  -  SERVICE
    // -----------------------------------------------------------------
    //  The "brain" of the application:
    //    - business rules / validation
    //    - mapping between the entity (Students) and DTOs
    //
    //  Knows nothing about HTTP (no status codes) and nothing about the
    //  database (no DbContext). It only uses the layer below:
    //  IStudentRepository.
    //
    //  Dependency direction:  Controller -> Service -> Repository -> DbContext
    // =================================================================
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<StudentDto>> GetAllAsync()
        {
            var students = await _repository.GetAllAsync();
            return students.Select(ToDto).ToList();
        }

        public async Task<StudentDto?> GetByIdAsync(int id)
        {
            var student = await _repository.GetByIdAsync(id);
            return student == null ? null : ToDto(student);
        }

        public async Task<StudentDto> CreateAsync(SaveStudentDto dto)
        {
            Validate(dto);

            var student = new Students { Name = dto.Name.Trim(), Age = dto.Age };
            var saved = await _repository.AddAsync(student);
            return ToDto(saved);
        }

        public async Task<bool> UpdateAsync(int id, SaveStudentDto dto)
        {
            Validate(dto);

            var student = new Students { Id = id, Name = dto.Name.Trim(), Age = dto.Age };
            return await _repository.UpdateAsync(student);
        }

        public Task<bool> DeleteAsync(int id)
        {
            return _repository.DeleteAsync(id);
        }

        // ---- Business rules ------------------------------------------
        // Broken rules throw ArgumentException. ExceptionHandlingMiddleware
        // (Presentation layer) turns that into 400 Bad Request.
        private static void Validate(SaveStudentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                throw new ArgumentException("Name is required.");
            }

            if (dto.Age < 15 || dto.Age > 120)
            {
                throw new ArgumentException("Age must be between 15 and 120.");
            }
        }

        // ---- Mapping entity -> DTO -----------------------------------
        private static StudentDto ToDto(Students s)
        {
            return new StudentDto(s.Id, s.Name, s.Age);
        }
    }
}
