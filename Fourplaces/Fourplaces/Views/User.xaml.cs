using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class User : BaseContentPage
    {
		public User()
		{
			InitializeComponent();
            BindingContext = new UserViewModel();
		}
	}
}