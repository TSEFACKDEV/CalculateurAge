using CalculateurAge.ViewModels;

namespace CalculateurAge;

public partial class MainPage : ContentPage
{
    public MainPage(CalculateurViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}