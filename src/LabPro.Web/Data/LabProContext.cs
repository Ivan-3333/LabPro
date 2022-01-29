using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LabPro.Web.Data
{
    public class LabProContext : DbContext
    {
        private IDbContextTransaction _currentTransaction;

        public LabProContext(DbContextOptions<LabProContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
