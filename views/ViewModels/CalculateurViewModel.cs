using System.Collections.ObjectModel;
using CalculateurAge.Services;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private readonly INavigationService _navigationService;
    private readonly ResultatViewModel _resultatViewModel;

    // Champs privés
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _categorie = "";
    private string _messageErreur = "";
    private bool _messageErreurVisible;
    private int _joursRestants;
    private int _ageCalcule;

    // Propriétés publiques
    public string Nom
    {
        get => _nom;
        set
        {
            if (SetField(ref _nom, value))
            {
                CalculerCommand.Rafraichir();
            }
        }
    }

    public DateTime DateNaissance
    {
        get => _dateNaissance;
        set => SetField(ref _dateNaissance, value);
    }

    public string Resultat
    {
        get => _resultat;
        set => SetField(ref _resultat, value);
    }

    public bool ResultatVisible
    {
        get => _resultatVisible;
        set
        {
            if (SetField(ref _resultatVisible, value))
            {
                VoirResultatCommand.Rafraichir();
            }
        }
    }

    public string Categorie
    {
        get => _categorie;
        set => SetField(ref _categorie, value);
    }

    public string MessageErreur
    {
        get => _messageErreur;
        set => SetField(ref _messageErreur, value);
    }

    public bool MessageErreurVisible
    {
        get => _messageErreurVisible;
        set => SetField(ref _messageErreurVisible, value);
    }

    public int JoursRestants
    {
        get => _joursRestants;
        set => SetField(ref _joursRestants, value);
    }

    public ObservableCollection<string> Historique { get; } = new();

    // Commandes
    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }
    public RelayCommand VoirResultatCommand { get; }

    // Constructeur avec injection de dépendances
    public CalculateurViewModel(INavigationService navigationService,
                                ResultatViewModel resultatViewModel)
    {
        _navigationService = navigationService;
        _resultatViewModel = resultatViewModel;

        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom)
        );

        EffacerCommand = new RelayCommand(Effacer);

        VoirResultatCommand = new RelayCommand(
            VoirResultat,
            () => ResultatVisible
        );
    }

    // Logique métier : calcul de l'âge et des informations dérivées
    private void Calculer()
    {
        // Réinitialiser les messages d'erreur
        MessageErreur = "";
        MessageErreurVisible = false;

        // Validation : date future
        if (DateNaissance.Date > DateTime.Today)
        {
            MessageErreur = "La date de naissance ne peut pas être dans le futur.";
            MessageErreurVisible = true;
            ResultatVisible = false;
            return;
        }

        // Calcul de l'âge
        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age))
            age--;

        _ageCalcule = age;

        // Mise à jour des propriétés
        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
        Categorie = age >= 18 ? "Majeur" : "Mineur";

        // Calcul des jours restants avant le prochain anniversaire
        DateTime prochainAnniversaire = new DateTime(
            DateTime.Today.Year, DateNaissance.Month, DateNaissance.Day);

        if (prochainAnniversaire < DateTime.Today)
            prochainAnniversaire = prochainAnniversaire.AddYears(1);

        JoursRestants = (prochainAnniversaire - DateTime.Today).Days;

        // Ajout à l'historique
        Historique.Insert(0, $"{Nom} - {age} ans - {DateTime.Now:dd/MM/yyyy HH:mm}");
    }

    // Réinitialisation de tous les champs
    private void Effacer()
    {
        Nom = "";
        DateNaissance = DateTime.Today.AddYears(-20);
        Resultat = "";
        ResultatVisible = false;
        Categorie = "";
        MessageErreur = "";
        MessageErreurVisible = false;
        JoursRestants = 0;
        // On ne vide pas l'historique ici pour permettre de le consulter
        // Historique.Clear();
    }

    // Navigation vers ResultatPage en passant par le ViewModel
    private async void VoirResultat()
    {
        // Remplir le ViewModel de résultat
        _resultatViewModel.Nom = Nom;
        _resultatViewModel.Age = _ageCalcule;
        _resultatViewModel.Categorie = Categorie;
        _resultatViewModel.JoursRestants = JoursRestants;

        // Naviguer sans paramètres d'URL
        await _navigationService.GoToResultatPageAsync();
    }
}