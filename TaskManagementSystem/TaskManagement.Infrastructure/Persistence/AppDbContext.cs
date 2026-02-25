using Microsoft.EntityFrameworkCore;
using TaskManagement.Infrastructure.Persistence.Models;
using Task = TaskManagement.Infrastructure.Persistence.Models.Task;
using TaskStatus = TaskManagement.Infrastructure.Persistence.Models.TaskStatus;

namespace TaskManagement.Infrastructure.Persistence
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Task> Tasks { get; set; }

        public virtual DbSet<TaskHistory> TaskHistories { get; set; }

        public virtual DbSet<TaskStatus> TaskStatuses { get; set; }

        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Task>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Tasks__3214EC070E1CAEC5");

                entity.HasOne(d => d.Status).WithMany(p => p.Tasks)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Tasks__StatusId__5070F446");
            });

            modelBuilder.Entity<TaskHistory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TaskHist__3214EC076BE1C885");

                entity.Property(e => e.ActionDate).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Task).WithMany(p => p.TaskHistories)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TaskHisto__TaskI__5535A963");

                entity.HasOne(d => d.User).WithMany(p => p.TaskHistories)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TaskHisto__UserI__5629CD9C");
            });

            modelBuilder.Entity<TaskStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__TaskStat__3214EC076A0D30B8");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07FC595E16");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}