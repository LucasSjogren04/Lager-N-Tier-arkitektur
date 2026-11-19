using Microsoft.EntityFrameworkCore;

public class Lager_N_Tier_arkitekturContext(DbContextOptions<Lager_N_Tier_arkitekturContext> options) : DbContext(options)
{
    public DbSet<Lager_N_Tier_arkitektur.Models.Students> Students { get; set; } = default!;
}
