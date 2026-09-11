# Max-Power : Puissance 4

Bienvenue dans le dépôt du projet **Max-Power**, une implémentation complète et moderne du célèbre jeu de plateau **Puissance 4** développée en **C# (.NET)**.

## Fonctionnalités

*   **Deux interfaces utilisateur :**
    *   **Mode Console** (`Connect4.Console`) : Jouez directement dans votre terminal avec un affichage ASCII.
    *   **Mode Graphique** (`Connect4.GUI`) : Profitez d'une interface visuelle développée avec **WPF** (Windows Presentation Foundation) pour une expérience fluide à la souris.
*   **Deux modes de jeu :**
    *   **Joueur contre Joueur (Local)** : Affrontez un ami sur la même machine.
    *   **Joueur contre Ordinateur (IA)** : Jouez contre une intelligence artificielle utilisant l'algorithme *Minimax*.
*   **Architecture propre :** Le projet sépare la logique métier de l'interface utilisateur pour une meilleure évolutivité.

## Structure du Projet

Le projet est divisé en trois sous-projets, réunis au sein de la solution `Connect4.slnx` :

1.  **`Connect4.Core`** : Une bibliothèque de classes contenant toute la logique du jeu. Elle gère la grille de jeu, détecte les conditions de victoire (horizontale, verticale, diagonale), gère les tours, et inclut l'intelligence artificielle.
2.  **`Connect4.Console`** : L'application console qui utilise la logique de `Connect4.Core` pour offrir une interface textuelle interactive.
3.  **`Connect4.GUI`** : L'application graphique (WPF) qui s'appuie également sur `Connect4.Core` et fournit une fenêtre dynamique cliquable avec des jetons colorés.

## Comment lancer le jeu ?

Assurez-vous d'avoir le **SDK .NET** d'installé sur votre machine.

### Lancer l'interface graphique (WPF)
Ouvrez un terminal à la racine du projet et exécutez :
```bash
dotnet run --project Connect4.GUI
```
*(Note : L'interface WPF est uniquement compatible avec Windows).*

### Lancer le mode console
Dans votre terminal, exécutez :
```bash
dotnet run --project Connect4.Console
```

## Règles du jeu

Le but du Puissance 4 est d'aligner une suite de **4 jetons de même couleur** sur une grille comptant 6 rangées et 7 colonnes. Tour à tour, les deux joueurs placent un jeton dans la colonne de leur choix, le jeton tombe alors au niveau le plus bas possible.
Le gagnant est le joueur qui réalise le premier un alignement (horizontal, vertical ou diagonal) consécutif d'au moins quatre jetons de sa couleur. Si toutes les cases de la grille sont remplies et qu'aucun des deux joueurs n'a réalisé un tel alignement, la partie est déclarée nulle.
