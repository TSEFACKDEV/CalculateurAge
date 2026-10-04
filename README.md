# CalculateurAge

Application mobile multiplateforme permettant de calculer l'âge d'une personne à
partir de son nom et de sa date de naissance, ainsi que sa catégorie d'âge
(majeur ou mineur) et le nombre de jours restant avant son prochain anniversaire.

## Fonctionnalités

- Saisie du nom et de la date de naissance.
- Calcul de l'âge exact, gestion des anniversaires non encore passés dans
  l'année en cours.
- Affichage de la catégorie : majeur (18 ans et plus) ou mineur.
- Calcul du nombre de jours restant avant le prochain anniversaire.
- Validation des dates de naissance futures, avec message d'erreur dédié.
- Historique des calculs effectués durant la session, du plus récent au plus
  ancien.
- Réinitialisation des champs de saisie via la commande « Effacer ».
- Page de résultat détaillé, accessible depuis l'écran principal.

## Prérequis

- .NET SDK 10.0
- SDK Android et un appareil ou un émulateur Android (pour le développement et
  l'exécution sur Android)
- workloads .NET MAUI correspondant aux plateformes ciblées

Installation des workloads :

```bash
dotnet workload install maui-android
```

Sous Windows, le workload `maui` installe l'ensemble des plateformes
(Android, iOS, Mac Catalyst, Windows). Sous macOS, utiliser `maui`.

## Compilation et exécution

```bash
# Restauration des dépendances
dotnet restore

# Compilation
dotnet build

# Exécution sur Android
dotnet build -t:Run -f net10.0-android

# Exécution sur Windows (cible Windows uniquement)
dotnet build -t:Run -f net10.0-windows10.0.19041.0
```

## Plateformes ciblées

Les cibles sont définies dans `CalculateurAge.csproj` :

| Plateforme | Condition                       | Version minimale |
|------------|---------------------------------|------------------|
| Android    | Toutes                          | 21.0 (API 21)    |
| iOS        | macOS uniquement                | 15.0             |
| Mac Catalyst| macOS uniquement               | 15.0             |
| Windows    | Windows uniquement              | 10.0.17763.0     |

Sur Linux, seul Android est compilé.

## Architecture

Le projet suit le patron MVVM avec injection de dépendances.

- **Vue** (`MainPage.xaml`, `ResultatPage.xaml`) : interface utilisateur et
  liaison de données, sans logique métier.
- **ViewModel** (`CalculateurViewModel`, `ResultatViewModel`) : état de l'écran,
  validation, calcul et commandes.
- **Service** (`INavigationService`, `ShellNavigationService`) : abstraction de
  la navigation, isolée derrière une interface.

### Composition

`MauiProgram.cs` enregistre les dépendances dans le conteneur :

| Type                        | Durée de vie |
|-----------------------------|--------------|
| `INavigationService`        | Singleton    |
| `ResultatViewModel`         | Singleton    |
| `CalculateurViewModel`      | Transient    |
| `MainPage`, `ResultatPage`  | Transient    |

`ResultatViewModel` est un singleton afin de conserver les données du calcul
lors de la navigation vers la page de résultat, sans passer par les paramètres
de route Shell.

### Liaison de données

- `BaseViewModel` implémente `INotifyPropertyChanged` et fournit `SetField`,
  qui notifie uniquement lorsque la valeur a réellement changé.
- `RelayCommand` implémente `ICommand` avec une fonction `CanExecute` et une
  méthode `Rafraichir` pour notifier le changement d'état.
- La validation des commandes est branchée dans les setters de
  `CalculateurViewModel` : la modification du nom met à jour l'état du bouton
  « Calculer », le changement d'état du résultat met à jour « Voir le résultat
  détaillé ».

## Organisation du projet

```
CalculateurAge/
├── App.xaml / App.xaml.cs          Point d'entrée, fenêtre et Shell
├── AppShell.xaml                    Déclaration des routes de navigation
├── MainPage.xaml(.cs)               Écran de saisie et de calcul
├── MauiProgram.cs                   Configuration et injection de dépendances
├── CalculateurAge.csproj            Configuration du projet et cibles
├── Services/
│   ├── INavigationService.cs        Contrat de navigation
│   └── ShellNavigationService.cs    Implémentation basée sur Shell
├── views/
│   ├── ResultatPage.xaml(.cs)       Écran de résultat détaillé
│   └── ViewModels/
│       ├── BaseViewModel.cs         Notification de changement de propriété
│       ├── CalculateurViewModel.cs  Logique de calcul et validation
│       ├── RelayCommand.cs          Implémentation de ICommand
│       └── ResultatViewModel.cs     Données affichées par ResultatPage
├── Platforms/                       Code spécifique à chaque plateforme
└── Resources/                       Icônes, polices, styles et images
```

## Conventions de code

- Nommage des types en PascalCase, des champs privés préfixés par `_`.
- Interface utilisateur et messages en français.
- Les ressources statiques (couleurs, styles) sont centralisées dans
  `Resources/Styles`.
- La navigation passe par `INavigationService` plutôt que par un appel direct à
  `Shell.Current`.

