namespace CalculateurAge.Services;

public interface INavigationService
{
    Task GoToResultatPageAsync();
    Task GoBackAsync();
}