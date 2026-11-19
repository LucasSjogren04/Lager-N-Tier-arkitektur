namespace Lager_N_Tier_arkitektur.Dtos
{
    // =================================================================
    //  BUSINESS LAYER  -  DTOs (Data Transfer Objects)
    // -----------------------------------------------------------------
    //  The shapes of data that go in and out of the API as JSON.
    //  The controller works with these instead of the Students entity,
    //  so the database model is never exposed and the client cannot
    //  "over-post" fields it should not set (like Id).
    // =================================================================

    // Output: what GET returns.
    public record StudentDto(int Id, string Name, int Age);

    // Input: body of POST and PUT (no Id - it comes from the database / URL).
    public record SaveStudentDto(string Name, int Age);
}
