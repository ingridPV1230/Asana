namespace Asana.MAUI
{
	public partial class MainPage : ContentPage
	{

		public MainPage()
		{
			InitializeComponent();
			BindingContext = new MainPageViewModel();
        }
		
	}
}
