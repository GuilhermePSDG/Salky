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
           
            modelBuilder.ApplyConfiguration(new FriendConfiguration());
            modelBuilder.ApplyConfiguration(new MessageConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            //
            modelBuilder.ApplyConfiguration(new GroupRoleConfig());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new GroupMemberConfiguration());

            base.OnModelCreating(modelBuilder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            base.OnConfiguring(optionsBuilder);
        }


    }
}
