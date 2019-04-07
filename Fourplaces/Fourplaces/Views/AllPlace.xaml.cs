using Fourplaces.ViewModels;
using Xamarin.Forms.Xaml;
using Storm.Mvvm.Forms;

namespace Fourplaces
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AllPlace : BaseContentPage
    {
        public AllPlace()
        {
            InitializeComponent();
            BindingContext = new AllPlaceViewModel();
        }
    }
}
