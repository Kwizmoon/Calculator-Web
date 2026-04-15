namespace CalculatriceLibrary.Models
{
    /// Représente un calcul effectué et enregistré en base de données.
    /// Chaque instance = une ligne dans la table CalculationLogs.
    public class CalculationLog
    {
        // Clé primaire (Id auto-incrémenté par SQLite).
        public int Id { get; set; }

        // Expression saisie ou formatée (ex : "2+3", "sqrt(9)", "(2+3)*4").
        public string? Expression { get; set; }

        // Premier opérande (ex : 2 dans "2+3").
        public double? Operand1 { get; set; }

        // Deuxième opérande — nullable pour racine carrée et exposant 2 (un seul opérande).
        public double? Operand2 { get; set; }

        // Symbole de l'opération : "+", "-", "*", "/", "pow2", "powN", "sqrt", "expr".
        public string Operator { get; set; } = string.Empty;

        // Résultat du calcul.
        public double Result { get; set; }

        // Date et heure du calcul — enregistrée automatiquement.
        public DateTime CreatedAt { get; set; }

        public int? UserId { get; set; }

        public User?  User { get; set; }


    }
}