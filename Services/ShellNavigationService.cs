namespace CalculateurAge.Services;

public class ShellNavigationService : INavigationService
{
    public async Task GoToResultatPageAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.ResultatPage));
    }

    public async Task GoBackAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}