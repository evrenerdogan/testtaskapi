using Microsoft.EntityFrameworkCore;
using uploader.Data;

namespace uploader.Services
{
    public class FileDbContext : DbContext
    {
        public FileDbContext(DbContextOptions<FileDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<File>(entity =>
                {
                    entity.Property(e => e.Name).HasMaxLength(255);
                    entity.Property(e => e.Ext).HasMaxLength(20);
                    entity.Property(e => e.ContentType).HasMaxLength(100);
                    entity.Property(e => e.Link).HasMaxLength(255);
                }

            );

                
        }


        public virtual DbSet<File> Files { get; set; }
    }
}
