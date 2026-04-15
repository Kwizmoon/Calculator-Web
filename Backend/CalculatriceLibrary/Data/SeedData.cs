using CalculatriceLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace CalculatriceLibrary.Data
{

    /// Classe séparée pour définir les données de départ (seeding).
    /// Ces données sont insérées automatiquement lors de la première migration.

    public static class SeedData
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CalculationLog>().HasData(

                new CalculationLog
                {
                    Id = 1,
                    Expression = "10+5",
                    Operand1 = 10,
                    Operand2 = 5,
                    Operator = "+",
                    Result = 15,
                    CreatedAt = new DateTime(2026, 3, 13, 9, 0, 0)

                },

                new CalculationLog
                {
                    Id = 2,
                    Expression = "100-37",
                    Operand1 = 100,
                    Operand2 = 37,
                    Operator = "-",
                    Result = 63,
                    CreatedAt = new DateTime(2025, 1, 1, 9, 1, 0)
                },

                new CalculationLog
                {
                    Id = 3,
                    Expression = "7*8",
                    Operand1 = 7,
                    Operand2 = 8,
                    Operator = "*",
                    Result = 56,
                    CreatedAt = new DateTime(2025, 1, 1, 9, 2, 0)
                },

                new CalculationLog
                {
                    Id = 4,
                    Expression = "144/12",
                    Operand1 = 144,
                    Operand2 = 12,
                    Operator = "/",
                    Result = 12,
                    CreatedAt = new DateTime(2025, 1, 1, 9, 3, 0)
                },

                new CalculationLog
                {
                    Id = 5,
                    Expression = "9^2",
                    Operand1 = 9,
                    Operand2 = null,
                    Operator = "pow2",
                    Result = 81,
                    CreatedAt = new DateTime(2025, 1, 1, 9, 4, 0)
                },

                new CalculationLog
                {
                    Id = 6,
                    Expression = "2^10",
                    Operand1 = 2,
                    Operand2 = 10,
                    Operator = "powN",
                    Result = 1024,
                    CreatedAt = new DateTime(2024, 2, 1, 9, 5, 0)
                },

                new CalculationLog
                {
                    Id = 7,
                    Expression = "sqrt(256)",
                    Operand1 = 256,
                    Operand2 = null,
                    Operator = "sqrt",
                    Result = 16,
                    CreatedAt = new DateTime(2024, 5, 20, 9, 6, 0)
                }
            );
        }
    }
}