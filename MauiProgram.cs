using CalculateurAge;
using CalculateurAge.Services;
using CalculateurAge.ViewModels;
using CalculateurAge.Views;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<INavigationService, ShellNavigationService>();

        // ViewModels
        builder.Services.AddSingleton<ResultatViewModel>();
        builder.Services.AddTransient<CalculateurViewModel>();

        // Pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<ResultatPage>();

        return builder.Build();
    }
}