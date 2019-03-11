using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FocusPlace : TabbedPage
    {
		public FocusPlace ()
		{
			InitializeComponent ();
            BindingContext = new FocusPlaceViewModel();
        }
    }
}