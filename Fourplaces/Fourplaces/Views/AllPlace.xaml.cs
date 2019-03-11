using Fourplaces.ViewModels;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Storm.Mvvm;
using Storm.Mvvm.Forms;
using System;
using System.Diagnostics;
using Storm.Mvvm.Services;

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
