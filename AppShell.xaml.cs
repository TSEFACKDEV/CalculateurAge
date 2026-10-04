namespace CalculateurAge;
using CalculateurAge.Views;
public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		 // Déclare la route.
    	Routing.RegisterRoute(
			nameof(ResultatPage),
			typeof(ResultatPage));
	}
}
