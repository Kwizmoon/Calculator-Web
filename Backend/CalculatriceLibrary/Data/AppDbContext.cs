using CalculatriceLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatriceLibrary.Data
{

    /// Contexte EF Core pour la base SQLite de la calculatrice.
    /// Gère la connexion à la BD et la structure des tables.

    public class AppDbContext : DbContext
    {
        // Représente la table CalculationLogs dans la base de données.
        public DbSet<CalculationLog> CalculationLogs { get; set; }

        // Chemin vers le fichier SQLite (AppData\Local\calculatrice_tp1.db).
        public string DbPath { get; }

        public AppDbContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "calculatrice_tp1.db");
        }

        // Indique à EF Core comment se connecter à la BD.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
        }

        // Indique à EF Core comment construire les tables et injecter les données.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // On délègue le seeding à une classe séparée pour garder le code propre.
            SeedData.Configure(modelBuilder);
        }
    }
}