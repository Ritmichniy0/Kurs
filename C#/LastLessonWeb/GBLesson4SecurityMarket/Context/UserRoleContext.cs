using Microsoft.EntityFrameworkCore;
using GBLesson4SecurityMarket.Model;
using System;
using System.Linq;

namespace GBLesson4SecurityMarket.Context
{
    public class UserRoleContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Role> Roles { get; set; }

        //private string _connectionString;

        public UserRoleContext(DbContextOptions<UserRoleContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public UserRoleContext()
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(ent =>
            {
                ent.HasKey(x => x.Id).HasName("users_key");
                ent.HasIndex(x => x.Email).IsUnique();

                ent.ToTable("users");

                ent.Property(e => e.Id).HasColumnName("id");

                ent.Property(e => e.Email).HasMaxLength(255).HasColumnName("name");

                ent.Property(e => e.Password).HasMaxLength(255).HasColumnName("password");

                ent.Property(e => e.Salt).HasColumnName("salt");

                ent.Property(e => e.RoleId).HasConversion<int>();

                ent.HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            });

            modelBuilder.Entity<Role>().Property(e => e.RoleId).HasConversion<int>();

            modelBuilder.Entity<Role>().HasData(Enum.GetValues(typeof(UserRoleType)).Cast<UserRoleType>().Select(u =>
                new Role()
                {
                    RoleId = u,
                    Name = u.ToString()
                }));
        }
    }
}

// в терминале для ChatBD  -  dotnet tool install --global dotnet-ef --version 8.0.4
// далее  -  dotnet ef migrations add InitialCreate. Получили два файла в каталоге Migrations.
// для изменений в БД  -  dotnet ef database update