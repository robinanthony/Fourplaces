﻿using Fourplaces.ViewModels;
using Storm.Mvvm.Forms;
using Xamarin.Forms.Xaml;

namespace Fourplaces
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : BaseContentPage
    {
		public Login ()
		{
			InitializeComponent();
            BindingContext = new LoginViewModel();
        }
    }
}