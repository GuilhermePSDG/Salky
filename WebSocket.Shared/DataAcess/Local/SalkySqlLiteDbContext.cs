using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"DataSource={Path.Combine(FolderPath,DbName)};");
        }
    }


}

