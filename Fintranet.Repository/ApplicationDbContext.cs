using Microsoft.EntityFrameworkCore;
using Fintranet.Domain.Models;

namespace Repository;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<CongestionTaxTransaction> CongestionTaxTransactions { get; set; }
}