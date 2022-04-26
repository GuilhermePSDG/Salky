using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Salky.Domain;
using Salky.Domain.Salky.Domain;
using Salky.Persistence.ModelsConfigure;


namespace Salky.Persistence.Contexts
{
    public class SalkyDbContext : IdentityDbContext<User, Role, Guid,IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public SalkyDbContext(DbContextOptions<SalkyDbContext> options) : base(options) {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Contato> Contacts { get; set; }
        public DbSet<Message> Messages { get; set; }
      

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());


            modelBuilder.Entity<UserRole>(userRole =>
            {
                modelBuilder.Entity<User>()
                .Ignore(e => e.Contatos)
                .Ignore(e => e.FriendRequests);

                modelBuilder.Entity<Contato>()
                    .HasOne(s => s.UserOwner)
                    .WithMany(f => f.UserOwnerList)
                    .HasForeignKey(s => s.UserOwnerId);

                modelBuilder.Entity<Contato>()
                    .HasOne(s => s.UserContact)
                    .WithMany(f => f.UserContactList)
                    .HasForeignKey(s => s.UserContactId);


                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            }
            );
            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }
    }
}
