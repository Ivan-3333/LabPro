using LabPro.Web.Models;
using LabPro.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LabPro.Web.Data
{
    public class LabProContext : DbContext
    {

        private readonly SecurityService _securityService;

        public LabProContext(DbContextOptions<LabProContext> options, SecurityService securityService) : base(options)
        {
            _securityService = securityService;
        }


        public DbSet<Company> Companies { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Company");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _securityService.User.Name;
                        entry.Entity.Created = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _securityService.User.Name;
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        public override int SaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = _securityService.User.Name;
                        entry.Entity.Created = DateTime.Now;
                        entry.Entity.LastModifiedBy = _securityService.User.Name;
                        entry.Entity.LastModified = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedBy = _securityService.User.Name;
                        entry.Entity.LastModified = DateTime.Now;
                        break;
                }
            }

            var result = base.SaveChanges();

            return result;
        }
    }
}
