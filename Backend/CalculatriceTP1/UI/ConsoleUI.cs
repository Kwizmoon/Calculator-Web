// Projet : CalculatriceTP1
// Dossier : UI

using CalculatriceLibrary;
using CalculatriceLibrary.Data;
using CalculatriceLibrary.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace CalculatriceTP1.UI
{

    /// Classe qui gère l'interface console de la calculatrice.
    /// Sépare l'affichage de la logique métier (Calculator).

    public class ConsoleUI
    {
        private readonly Calculator _calculator;
        private readonly AppDbContext _dbContext;
        private readonly string _jsonLogPath;


        // Dernier résultat affiché sur l'écran
        private string _ecran = "0";

        public ConsoleUI(Calculator calculator, AppDbContext dbContext)
        {
            _calculator = calculator;
            _dbContext = dbContext;

            _jsonLogPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "calculatrice_tp1_logs.json");
        }

        // ── Méthode principale — boucle du menu ──────────────────

        public void Run()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool quitter = false;
            while (!quitter)
            {
                AfficherMenu();

                Console.Write("  Votre choix : ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string? choix = Console.ReadLine()?.Trim();
                Console.ResetColor();

                switch (choix)
                {
                    case "1": EffectuerAddition(); break;
                    case "2": EffectuerSoustraction(); break;
                    case "3": EffectuerMultiplication(); break;
                    case "4": EffectuerDivision(); break;
                    case "5": EffectuerExposantDeux(); break;
                    case "6": EffectuerExposantN(); break;
                    case "7": EffectuerRacineCarree(); break;
                    case "8": EffectuerExpressionLongue(); break;
                    case "9": AfficherHistorique(); break;
                    case "10":
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("\n  Êtes-vous sûr de vouloir supprimer l’historique ?(Y/N):  ");
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        string? reponse = Console.ReadLine()?.ToUpper();

                        if (reponse != "Y" & reponse != "N")
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("  Entrer Y pour oui ou N pour non.");
                            Pause();
                        }
                        else if (reponse == "N")
                            Pause();
                        else if (reponse == "Y")
                            EffacerHistorique();
                        break;
                        
                    case "0": quitter = true; break;
                    default:
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("\n  CHOIX INVALIDE");
                        Pause();
                        break;
                }
            }

            AfficherAdieu();
        }

        // ════════════════════════════════════════════════════════
        // MENU PRINCIPAL
        // Largeur interne : 58 caractères (entre les │ )
        // ════════════════════════════════════════════════════════

        private void AfficherMenu()
        {
            Console.Clear();

            // W = largeur interne entre les bordures
            const int W = 58;

            // ── Titre ─────────────────────────────────────────────
            Bord("┌" + H('─', W) + "┐");
            LigneTexte(W, "                 CALCULATRICE  TP1  —  C#", ConsoleColor.Cyan);
            Bord("├" + H('─', W) + "┤");

            // ── Écran ─────────────────────────────────────────────
            // Résultat aligné à droite, comme une vraie calculatrice
            string ecranTexte = _ecran.Length > W - 2 ? _ecran[^(W - 2)..] : _ecran;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  │");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write(ecranTexte.PadLeft(W - 1));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" │");
            Bord("├" + H('─', W) + "┤");

            // ── Opérations de base (2 colonnes de 29 chacune) ────
            // Séparateur vertical au milieu
            LigneDeux(W, "[1]", "+", "Addition", "[2]", "-", "Soustraction", ConsoleColor.White);
            Bord("├" + H('─', 29) + H('─', W - 29) + "┤");
            LigneDeux(W, "[3]", "×", "Multiplication", "[4]", "÷", "Division", ConsoleColor.White);
            Bord("├" + H('─', W) + "┤");

            // ── Espace + fonctions avancées ───────────────────────

            LigneDeux(W, "[5]", "x²", "Exposant 2", "[6]", "xⁿ", "Exposant N", ConsoleColor.White);
            Bord("├" + H('─', 29) + H('─', W - 29) + "┤");
            LigneDeux(W, "[7]", "√x", "Racine carrée", "[8]", "f(x)", "Expression", ConsoleColor.White);
            Bord("├" + H('─', W) + "┤");


            // ── Historique + Quitter ──────────────────────────────

            LigneDeux(W, "[9]", "◉", "Historique", "[10]", "", "Supprimer historique", ConsoleColor.White);
            Bord("├" + H('─', 29) + H('─', W - 29) + "┤");
            LigneUn(W, "[0]", "->", "Quittez", ConsoleColor.White);

            // ── Pied ──────────────────────────────────────────────
            Bord("└" + H('─', W) + "┘");
            Console.ResetColor();
            Console.WriteLine();
        }

        // ── Helpers de dessin ────────────────────────────────────

        // Ligne de bordure simple (haut, bas, séparateur)
        private void Bord(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  " + s);
            Console.ResetColor();
        }

        // Répète un caractère n fois
        private string H(char c, int n) => new string(c, n);


        // Ligne de texte centré dans le cadre
        private void LigneTexte(int w, string texte, ConsoleColor couleur)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  │");
            Console.ForegroundColor = couleur;
            Console.Write(texte.PadRight(w));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("│");
            Console.ResetColor();
        }

        // Ligne avec 2 options côte à côte séparées par │ au milieu
        private void LigneDeux(int w,
                                string k1, string sym1, string label1,
                                string k2, string sym2, string label2,
                                ConsoleColor couleur)
        {
            int col = w / 2 - 1;
            string gauche = $" {k1} {sym1}  {label1}";
            string droite = $" {k2} {sym2}  {label2}";
            if (gauche.Length > col) gauche = gauche[..col];
            if (droite.Length > col) droite = droite[..col];

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  │");
            Console.ForegroundColor = couleur;
            Console.Write(gauche.PadRight(col));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("│");
            Console.ForegroundColor = couleur;
            Console.Write(droite.PadRight(col + 1));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("│");
            Console.ResetColor();
        }

        // Ligne avec 1 option 
        private void LigneUn(int w,
                         string k2, string sym2, string label2,
                         ConsoleColor couleur)
        {
            int col = w - 22;

            string milieu = $" {k2} {sym2} {label2}";

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  │");
            Console.ForegroundColor = couleur;
            Console.Write("\t" + "\t" + "\t" + milieu.PadRight(col));
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.ForegroundColor = couleur;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(" │");
            Console.ResetColor();
        }

        // ════════════════════════════════════════════════════════
        // TITRE DE SECTION
        // ════════════════════════════════════════════════════════

        private void AfficherTitre(string titre)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("  │  ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{titre.PadRight(56)}");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("│");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
            Console.ResetColor();
            Console.WriteLine();
        }

        // ════════════════════════════════════════════════════════
        // RÉSULTAT ET ERREUR
        // ════════════════════════════════════════════════════════

        private void AfficherResultat(string expression, double result)
        {
            _ecran = result.ToString("G10");
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  {expression}");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"   = {_ecran}");
            Console.ResetColor();
        }

        private void AfficherErreur(string message)
        {
            _ecran = "ERREUR";
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"   {message}\n");
            Console.ResetColor();
        }

        private void Pause()
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("   Appuyez sur une touche pour continuer...");
            Console.ResetColor();
            Console.ReadKey(true);
        }

        // Lit un nombre valide — boucle jusqu'à saisie correcte
        private double LireNombre(string message)
        {
            while (true)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write($"  {message}");
                Console.ForegroundColor = ConsoleColor.Yellow;
                string? entree = Console.ReadLine();
                Console.ResetColor();

                entree = entree?.Replace(',', '.');

                if (double.TryParse(entree,
                        System.Globalization.NumberStyles.Any,
                        System.Globalization.CultureInfo.InvariantCulture,
                        out double valeur))
                {
                    return valeur;
                }

                AfficherErreur("Entrée invalide — entrez un nombre.");
            }
        }

        private void AfficherAdieu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  ┌──────────────────────────────────────────────────────────┐");

            Console.Write("  │  ");

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("       Merci d'avoir utilisé CALCULATRICE-TP1 !       ");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  │");

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("  │                                                          │");
            Console.WriteLine("  │          Logs sauvegardés dans AppData\\Local\\            │");
            Console.WriteLine("  └──────────────────────────────────────────────────────────┘");
            Console.ResetColor();
            Console.WriteLine();
        }


        // ════════════════════════════════════════════════════════
        // LOGS JSON + SAUVEGARDE BD 
        // ════════════════════════════════════════════════════════

        //
        private void SauvegarderOperation(string expression, double? op1, double? op2,
                                           string opSymbol, double result)
        {
            var log = new CalculationLog
            {
                Expression = expression,
                Operand1 = op1,
                Operand2 = op2,
                Operator = opSymbol,
                Result = result,
                CreatedAt = new DateTime(
                            DateTime.Now.Year,
                            DateTime.Now.Month,
                            DateTime.Now.Day,
                            DateTime.Now.Hour,
                            DateTime.Now.Minute,
                            DateTime.Now.Second)
            };

            // 1. Enregistrement dans la base de données SQLite
            _dbContext.CalculationLogs.Add(log);
            _dbContext.SaveChanges();

            // 2. Synchronisation complète du fichier JSON
            // On récupère TOUTE l'histoire pour réparer le JSON s'il manquait des lignes
            var fullHistory = _dbContext.CalculationLogs.ToList();
            SyncJson(fullHistory);
        }

        private void SyncJson(List<CalculationLog> logs)
        {
            try
            {
                var options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    // Dit au serializer de ne pas échapper les caractères spéciaux comme +, -, *, /, etc.
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                };
                string jsonContent = JsonSerializer.Serialize(logs, options);
                File.WriteAllText(_jsonLogPath, jsonContent);
            }
            catch (Exception ex)
            {
                // On affiche l'erreur en rouge sans bloquer l'utilisateur
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Erreur Synchro JSON : {ex.Message}");
                Console.ResetColor();
            }
        }



        // ════════════════════════════════════════════════════════
        // OPÉRATIONS
        // ════════════════════════════════════════════════════════

        private void EffectuerAddition()
        {
            AfficherTitre("Addition  [ + ]");
            double a = LireNombre(" Premier nombre  : ");
            double b = LireNombre(" Deuxième nombre : ");
            double result = _calculator.Add(a, b);
            AfficherResultat($" {a} + {b}", result);
            SauvegarderOperation($"{a}+{b}", a, b, "+", result);
            Pause();
        }

        private void EffectuerSoustraction()
        {
            AfficherTitre("Soustraction  [ − ]");
            double a = LireNombre(" Premier nombre  : ");
            double b = LireNombre(" Deuxième nombre : ");
            double result = _calculator.Subtract(a, b);
            AfficherResultat($" {a} - {b}", result);
            SauvegarderOperation($"{a}-{b}", a, b, "-", result);
            Pause();
        }

        private void EffectuerMultiplication()
        {
            AfficherTitre("Multiplication  [ × ]");
            double a = LireNombre(" Premier nombre  : ");
            double b = LireNombre(" Deuxième nombre : ");
            double result = _calculator.Multiply(a, b);
            AfficherResultat($" {a} × {b}", result);
            SauvegarderOperation($"{a}*{b}", a, b, "*", result);
            Pause();
        }

        private void EffectuerDivision()
        {
            AfficherTitre("Division  [ ÷ ]");
            double a = LireNombre(" Dividende : ");
            double b = LireNombre(" Diviseur  : ");
            try
            {
                double result = _calculator.Divide(a, b);
                AfficherResultat($" {a} ÷ {b}", result);
                SauvegarderOperation($"{a}/{b}", a, b, "/", result);
            }
            catch (DivideByZeroException ex) { AfficherErreur(ex.Message); }
            Pause();
        }

        private void EffectuerExposantDeux()
        {
            AfficherTitre("Exposant 2  [ x² ]");
            double a = LireNombre(" Nombre : ");
            double result = _calculator.Square(a);
            AfficherResultat($" {a}²", result);
            SauvegarderOperation($"{a}^2", a, null, "pow2", result);
            Pause();
        }

        private void EffectuerExposantN()
        {
            AfficherTitre("Exposant N  [ xⁿ ]");
            double a = LireNombre(" Base     : ");
            double n = LireNombre(" Exposant : ");
            double result = _calculator.Power(a, n);
            AfficherResultat($" {a}^{n}", result);
            SauvegarderOperation($"{a}^{n}", a, n, "powN", result);
            Pause();
        }

        private void EffectuerRacineCarree()
        {
            AfficherTitre("Racine carrée  [ √x ]");
            double a = LireNombre(" Nombre (>= 0) : ");
            try
            {
                double result = _calculator.SquareRoot(a);
                AfficherResultat($" √{a}", result);
                SauvegarderOperation($"sqrt({a})", a, null, "sqrt", result);
            }
            catch (ArgumentException ex) { AfficherErreur(ex.Message); }
            Pause();
        }

        private void EffectuerExpressionLongue()
        {
            AfficherTitre("Expression longue  [ BONUS ]");
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("   Exemples : (3+4)*2   sqrt(16)+2^3   10/(2+3)");
            Console.WriteLine();
            Console.Write("   Expression : ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            string? expr = Console.ReadLine();
            Console.ResetColor();

            if (string.IsNullOrWhiteSpace(expr))
            {
                AfficherErreur("Expression vide.");
                Pause();
                return;
            }

            try
            {
                double result = _calculator.EvaluerExpression(expr);
                AfficherResultat(" " + expr, result);
                SauvegarderOperation(expr, null, null, "expr", result);
            }
            catch (DivideByZeroException ex) { AfficherErreur(ex.Message); }
            catch (InvalidOperationException ex) { AfficherErreur(ex.Message); }
            catch (Exception ex) { AfficherErreur($"Invalide : {ex.Message}"); }
            Pause();
        }

        // ════════════════════════════════════════════════════════
        // HISTORIQUE
        // ════════════════════════════════════════════════════════

        private void AfficherHistorique()
        {
            Console.Clear();
            AfficherTitre("HISTORIQUE DES OPÉRATIONS");

            // LINQ : Tri décroissant
            var logs = _dbContext.CalculationLogs
                .OrderByDescending(l => l.CreatedAt)
                .ToList();

            if (logs.Count == 0)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.WriteLine("  Aucune opération enregistrée.");
                Pause();
            }
            else
            {
                // LINQ : Groupement par date
                var groupesParJour = logs.GroupBy(l => l.CreatedAt.Date);
                const int largeur = 50;

                foreach (var groupe in groupesParJour)
                {
                    // --- AFFICHAGE DE L'ENTÊTE DU JOUR ---
                    Console.WriteLine();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    string titreDate = GetDate(groupe.Key);
                    // On centre ou on aligne à gauche le titre dans le cadre
                    Console.WriteLine($"   {titreDate.PadRight(largeur)}");

                    // Ligne de séparation sous le titre 
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.WriteLine("   " + new string('─', largeur) + "┐");

                    foreach (var log in groupe)
                    {

                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.Write($"  {" ".PadRight(largeur)}");
                        Console.WriteLine(" │");


                        Console.Write("   ");
                        Console.ForegroundColor = ConsoleColor.Gray;
                        Console.Write(log.Expression.PadRight(largeur));
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("│");


                        Console.Write("   ");
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(log.Result.ToString().PadRight(largeur));
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine("│");


                        Console.Write($"  {" ".PadRight(largeur)}");
                        Console.WriteLine(" │");

                        // Ligne de séparation entre les calculs
                        Console.WriteLine("   " + new string('─', largeur) + "┤");
                    }
                }

                Console.ResetColor();
                Pause();
            }
        }


        // Helper pour afficher "Aujourd'hui" ou "Hier" au lieu de la date brute
        private string GetDate(DateTime date)
        {
            if (date == DateTime.Today) return "Aujourd'hui";
            if (date == DateTime.Today.AddDays(-1)) return "Hier";
            return date.ToString("dd MMMM yyyy"); 
        }


        public void EffacerHistorique()
        {
            try
            {
                // On cible uniquement les calculs qui ne font pas partie du seeding (ID > 7)
                // Pour éviter de supprimer les exemples préenregistrés dans la base de données
                var logsUtilisateur = _dbContext.CalculationLogs.Where(l => l.Id > 7);

                if (logsUtilisateur.Any())  
                {
                    _dbContext.CalculationLogs.RemoveRange(logsUtilisateur);
                    _dbContext.SaveChanges();
                }

                if (File.Exists(_jsonLogPath))
                {
                    File.Delete(_jsonLogPath);
                }

                _ecran = "0";
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n  [*] Historique effacé. Exemples conservés.");
                Console.ResetColor();
                Pause();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n  [!] Erreur lors de la suppression : {ex.Message}");
                Console.ResetColor();

                Pause();
            }
        }
    }

}