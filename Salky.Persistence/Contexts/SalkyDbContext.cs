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
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupsUsers { get; set; }
        public DbSet<Friend> Friend { get; set; }
        public DbSet<Notification> Notifications { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Friend>()
                .HasOne(a => a.RequestedBy)
                .WithMany(b => b.SentFriendRequests)
                .HasForeignKey(c => c.RequestedById);

            modelBuilder.Entity<Friend>()
                .HasOne(a => a.RequestedTo)
                .WithMany(b => b.ReceievedFriendRequests)
                .HasForeignKey(c => c.RequestedToId);


            modelBuilder.Entity<GroupUser>(group =>
            {
                //group
                //.HasKey(bc => new { bc.UserId,bc.GroupId });
                group
                .HasOne(f => f.Group)
                .WithMany(f => f.Members);
                group
                .HasOne(f => f.User)
                .WithMany(f => f.Groups);

            });


            modelBuilder.Entity<Group>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnerGroups);


            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());


            modelBuilder.Entity<UserRole>(userRole =>
            {
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
