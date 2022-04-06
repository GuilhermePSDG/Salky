using Microsoft.EntityFrameworkCore;
using WebSocket.Shared.DataAcess.Models;

namespace WebSocket.Shared.DataAcess.Local
{
    public class SalkySqlLiteDbContext : DbContext
    {

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Contato> Contatos { get; set; }
        public DbSet<Message> Mensagens { get; set; }
        public string FolderPath { get; }
        public string DbName { get; }

        public SalkySqlLiteDbContext()
        {
            this.FolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Salky");
            this.DbName = "SalkyDb.db";
            Directory.CreateDirectory(FolderPath);
            //Database.EnsureCreated();
            this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.UseSqlite($"DataSource={Path.Combine(FolderPath,DbName)};")
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            ;
        }
    }


}

