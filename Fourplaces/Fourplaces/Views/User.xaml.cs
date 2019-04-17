using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class User : TabbedPage
    {
		public User()
		{
			InitializeComponent();
            BindingContext = new UserViewModel();
		}
	}
}