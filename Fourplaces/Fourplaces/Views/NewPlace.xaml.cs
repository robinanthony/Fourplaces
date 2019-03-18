using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;
using Fourplaces.ViewModels;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewPlace : BaseContentPage
    {
		public NewPlace ()
		{
            InitializeComponent();
            BindingContext = new NewPlaceViewModel();
        }
    }
}