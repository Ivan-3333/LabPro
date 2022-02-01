using LabPro.Web.Models;
using LabPro.Web.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace LabPro.Web.Data
{
    public class LabProContext : DbContext
    {

        public DbSet<Company> Companies { get; set; }
        public DbSet<ContactPerson> ContactPersons { get; set; }
        public DbSet<Location> Locations { get; set; }


        private readonly SecurityService _securityService;

        public LabProContext(DbContextOptions<LabProContext> options, SecurityService securityService) : base(options)
        {
            _securityService = securityService;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>().ToTable("Company");
            modelBuilder.Entity<ContactPerson>().ToTable("ContactPerson");
            modelBuilder.Entity<Location>().ToTable("Location");
            modelBuilder.Entity<Note>().ToTable("Note");
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            UpdateAuditProperties();

            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }

        public override int SaveChanges()
        {
            UpdateAuditProperties();

            var result = base.SaveChanges();

            return result;
        }

        private void UpdateAuditProperties()
        {
            foreach (var entry in ChangeTracker.Entries<DatabaseEntity>())
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
        }
    }
}
