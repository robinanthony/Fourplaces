using Fourplaces.Models;
using Storm.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Fourplaces.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private UserData _myUser;
        public UserData MyUser {
            get
            {
                return this._myUser;
            }
            set
            {
                SetProperty(ref this._myUser, value);
            }
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            (Boolean test, UserData data) = await RestService.Rest.GetUserData();

            if (test)
            {
                MyUser = data;
            }
            else
            {
                // deconnexion ?
            }
        }
    }
}
