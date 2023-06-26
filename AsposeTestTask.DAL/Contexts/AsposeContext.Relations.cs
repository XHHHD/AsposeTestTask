using AsposeTestTask.Entities;
using Microsoft.EntityFrameworkCore;

namespace AsposeTestTask.DAL.Data
{
    public partial class AsposeContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Company>(c =>
            {
                c.HasMany(c => c.Members)
                    .WithOne(p => p.Company)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Person>(p =>
            {
                p.HasOne(p => p.Company)
                    .WithMany(c => c.Members)
                    .HasForeignKey(p => p.CompanyId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}
