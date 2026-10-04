using System.Collections.ObjectModel;

namespace CalculateurAge.ViewModels;

public class CalculateurViewModel : BaseViewModel
{
    private string _nom = "";
    private DateTime _dateNaissance = DateTime.Today.AddYears(-20);
    private string _resultat = "";
    private bool _resultatVisible;
    private string _categorie = "";
    private string _messageErreur = "";
    private bool _messageErreurVisible;
    private int _joursRestants;

    public string Nom
    {
        get => _nom;
        set { if (SetField(ref _nom, value)) CalculerCommand.Rafraichir(); }
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
        set => SetField(ref _resultatVisible, value);
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

    public RelayCommand CalculerCommand { get; }
    public RelayCommand EffacerCommand { get; }

    public CalculateurViewModel()
    {
        CalculerCommand = new RelayCommand(
            Calculer,
            () => !string.IsNullOrWhiteSpace(Nom)
        );

        EffacerCommand = new RelayCommand(Effacer);
    }

    private void Calculer()
    {
        MessageErreurVisible = false;
        MessageErreur = "";

        if (DateNaissance.Date > DateTime.Today)
        {
            MessageErreur = "La date de naissance ne peut pas être dans le futur.";
            MessageErreurVisible = true;
            return;
        }

        int age = DateTime.Today.Year - DateNaissance.Year;
        if (DateNaissance.Date > DateTime.Today.AddYears(-age)) age--;

        Resultat = $"{Nom}, vous avez {age} ans";
        ResultatVisible = true;
        Categorie = age >= 18 ? "Majeur" : "Mineur";

        DateTime prochainAnniversaire = new DateTime(
            DateTime.Today.Year, DateNaissance.Month, DateNaissance.Day);
        if (prochainAnniversaire < DateTime.Today)
            prochainAnniversaire = prochainAnniversaire.AddYears(1);

        JoursRestants = (prochainAnniversaire - DateTime.Today).Days;

        Historique.Insert(0, $"{Nom} - {age} ans - {DateTime.Now:dd/MM/yyyy HH:mm}");
    }

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
    }
}