using CalculatriceLibrary;
using CalculatriceLibrary.Data;
using CalculatriceTP1.UI;
using Microsoft.EntityFrameworkCore;

namespace CalculatriceTP1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Calculatrice TP1";
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // On crée les objets dont l'UI a besoin.
            // "using" garantit que la BD est fermée proprement à la fin.
            using AppDbContext dbContext = new AppDbContext();

            // Appliquer les migrations automatiquement au lancement.
            // Équivalent de "Update-Database" mais sans avoir à le faire manuellement.
            dbContext.Database.Migrate();

            Calculator calculator = new Calculator();
            ConsoleUI ui = new ConsoleUI(calculator, dbContext);

            // On lance la boucle de la calculatrice.
            ui.Run();
        }
    }
}