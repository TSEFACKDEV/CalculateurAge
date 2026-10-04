namespace CalculateurAge.ViewModels;

public class ResultatViewModel : BaseViewModel
{
    private string _nom = "";
    public string Nom
    {
        get => _nom;
        set => SetField(ref _nom, value);
    }

    private int _age;
    public int Age
    {
        get => _age;
        set => SetField(ref _age, value);
    }

    private string _categorie = "";
    public string Categorie
    {
        get => _categorie;
        set => SetField(ref _categorie, value);
    }

    private int _joursRestants;
    public int JoursRestants
    {
        get => _joursRestants;
        set => SetField(ref _joursRestants, value);
    }

    public string Message => $"{Nom}, vous avez {Age} ans";
}