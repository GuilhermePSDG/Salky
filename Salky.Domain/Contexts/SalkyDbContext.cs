using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Salky.Domain.Models.FriendModels;
using Salky.Domain.Models.GenericsModels;
using Salky.Domain.Models.GroupModels;
using Salky.Domain.Models.UserModels;
using Salky.Domain.ModelsConfigure;
using Salky.Domain.Salky.Domain;


namespace Salky.Domain.Contexts
{
    public class SalkyDbContext : DbContext
    {
        public SalkyDbContext(DbContextOptions<SalkyDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<MessageGroup> MessagesGroup { get; set; }
        public DbSet<FriendMessage> MessagesFriend { get; set; }
        
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupRole> GroupsRoles { get; set; }
        public DbSet<GroupMember> GroupsUsers { get; set; }
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


            modelBuilder.Entity<GroupMember>(member =>
            {
                member
                .HasOne(f => f.Group)
                .WithMany(f => f.Members)
                ;
                member
                .HasOne(f => f.User)
                .WithMany(f => f.Groups)
                ;
                member
                .HasOne(x => x.Role)
                .WithMany(x => x.MemberWithRoles)
                .OnDelete(DeleteBehavior.NoAction)
                ;
            });

            modelBuilder.Entity<Group>(group =>
            {
                group
                .HasMany(x => x.Members)
                .WithOne(x => x.Group);

                group
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnerGroups);

                group
                .HasMany(x => x.GroupRoles)
                .WithOne(x => x.Group);


            });

            modelBuilder.Entity<GroupRole>(groupRole =>
            {

                groupRole
                .HasOne(x => x.Group)
                .WithMany(x => x.GroupRoles)
                ;

                groupRole
                .HasMany(x => x.MemberWithRoles)
                .WithOne(x => x.Role);

                groupRole.HasIndex(x => new { x.RoleName, x.GroupId }).IsUnique();
            });

        


            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());


            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }


    }
}
