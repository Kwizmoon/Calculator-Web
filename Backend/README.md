# Calculatrice Avancée — TP1

**Cours :** 420-246-AH · Programmation Microsoft  
**Professeur :** Steve Lévesque  
**Cégep Ahuntsic**

---

## Membres de l'équipe

| Prénom            | Nom       | Matricule
|-------------------|-----------|
| Nanou Ange Robert | Kouassi   |
| Mohamed Sabry     | Banwan    | 
| Bao Tran          | Bach      | 
| Mohammed Amine    | Lemgandez | 

---

## Description

Application console C# qui effectue des calculs mathématiques.  
Chaque calcul est sauvegardé dans une base de données SQLite (via Entity Framework Core) et dans un fichier JSON.


![Menu Principal](images/calculator_1.png)

---

## Structure du projet

```
TP1_Final/
├── CalculatriceLibrary/
│   ├── Calculator.cs
│   ├── Models/
│   │   └── CalculationLog.cs
│   ├── Data/
│   │   ├── AppDbContext.cs
│   │   └── SeedData.cs
│   └── Migrations/
│       ├── InitialCreate.cs
│       └── AppDbContextModelSnapshot.cs
├── CalculatriceTP1/
│   ├── Program.cs
│   └── UI/
│       └── ConsoleUI.cs
└── README.md
```

---

## Fonctionnalités

| Option | Fonctionnalité                                       |
|--------|------------------------------------------------------|
| `1`    | Addition                                             |
| `2`    | Soustraction                                         |
| `3`    | Multiplication                                       |
| `4`    | Division                                             |
| `5`    | Exposant 2 (x²)                                      |
| `6`    | Exposant N (xⁿ)                                      |
| `7`    | Racine carrée (√x)                                   |
| `8`    | Expression longue (précédence + parenthèses)         |
| `9`    | Historique des calculs depuis la BD                  |
| `10`   | Supprimer l'historique mais garde les exemples       |
| `0`    | Quitter                                              |

---

## Installation et utilisation

### Prérequis
- Visual Studio 2026
- .NET 8.0 SDK

### Ouvrir le projet
1. Extraire le `.zip`
2. Ouvrir `TP1_Final.sln` dans Visual Studio
3. Clic droit sur `CalculatriceTP1` → **Set as Startup Project**
4. Clic droit sur la Solution → **Restore NuGet Packages**

---

## Migrations EF Core

Ouvrir le **Package Manager Console** : `Tools → NuGet Package Manager → Package Manager Console`

```powershell
# Créer la migration
Add-Migration InitialCreate -Project CalculatriceLibrary -StartupProject CalculatriceTP1

# Créer la base de données
Update-Database -Project CalculatriceLibrary -StartupProject CalculatriceTP1
```

> La migration s'applique aussi automatiquement au premier lancement grâce à `dbContext.Database.Migrate()` dans `Program.cs`.

---

## Base de données

**Fichier :** `C:\Users\<nom>\AppData\Local\calculatrice_tp1.db`

### Table `CalculationLogs`

| Colonne      | Type    | Description                                 |
|--------------|---------|---------------------------------------------|
| `Id`         | INTEGER | Clé primaire auto-incrémentée               |
| `Expression` | TEXT    | Expression calculée                         |
| `Operand1`   | REAL    | Premier opérande (nullable)                 |
| `Operand2`   | REAL    | Deuxième opérande (nullable)                |
| `Operator`   | TEXT    | `+` `-` `*` `/` `pow2` `powN` `sqrt` `expr` |
| `Result`     | REAL    | Résultat                                    |
| `CreatedAt`  | TEXT    | Date et heure                               |

### Données initiales (seeding)

| Expression | Opérateur | Résultat |
|------------|-----------|----------|
| 10+5       | +         | 15       |
| 100-37     | -         | 63       |
| 7*8        | *         | 56       |
| 144/12     | /         | 12       |
| 9^2        | pow2      | 81       |
| 2^10       | powN      | 1024     |
| sqrt(256)  | sqrt      | 16       |

---

## Fichiers générés

| Fichier                      | Emplacement      |
|------------------------------|------------------|
| `calculatrice_tp1.db`        | `AppData\Local\` |
| `calculatrice_tp1_logs.json` | `AppData\Local\` |