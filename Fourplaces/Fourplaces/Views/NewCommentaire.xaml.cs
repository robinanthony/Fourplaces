using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;
using Fourplaces.ViewModels;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewCommentaire : BaseContentPage
    {
		public NewCommentaire ()
		{
            InitializeComponent();
            BindingContext = new NewCommentaireViewModel();
        }
    }
}