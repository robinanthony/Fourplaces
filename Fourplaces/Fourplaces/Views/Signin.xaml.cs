using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Signin : BaseContentPage
	{
		public Signin ()
		{
            InitializeComponent();
            BindingContext = new SigninViewModel();
        }
	}
}