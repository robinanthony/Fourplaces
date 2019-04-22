using Storm.Mvvm;
using Fourplaces.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Windows.Input;

namespace Fourplaces.ViewModels
{
    class AllPlaceViewModel : ViewModelBase
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand RefreshCommand { get; private set; }
        private bool _isRefreshing = true;

        public ICommand AddPlaceCommand { get; private set; }

        public ICommand SeeUserInfos { get; private set; }

        private string _titleLabel;
        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

        public Position MaLocation { get; set; }

        private ObservableCollection<Place> _allPlaces;
        public  ObservableCollection<Place> Places
        {
            get => this._allPlaces;
            set
            {
                SetProperty(ref this._allPlaces, value);
            }
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private Place _currentPlace;
        public Place CurrentPlace
        {
            get => this._currentPlace;
            set
            {
                if (SetProperty(ref this._currentPlace, value))
                {
                    OpenFocusPlace(this._currentPlace);
                }
            }
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public AllPlaceViewModel()
        {
            this.TitleLabel = "Tous les lieux";
            this.RefreshCommand = new Command(this.RefreshClicked);
            this.AddPlaceCommand = new Command(this.AddPlaceClicked);
            this.SeeUserInfos = new Command(this.SeeUserInfosClicked);
        }

        public override async Task OnResume()
        {
            await base.OnResume();

            this.MaLocation = await MyGeolocator.GetLocation();

            this.Places = await RestService.Rest.LoadPlaces(this.MaLocation);

            this.IsRefreshing = false;
        }

//==============================================================================
//================================ FCT METIERS =================================
//==============================================================================
        public async void OpenFocusPlace(Place place)
        {
            await NavigationService.PushAsync<FocusPlace>(new Dictionary<string, object>() { { "PlaceId", place.Id } });
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void RefreshClicked()
        {
            this.RefreshPlaces();
            this.IsRefreshing = false;
        }

        private async void RefreshPlaces()
        {
            this.MaLocation = await MyGeolocator.GetLocation();
            this.Places = await RestService.Rest.LoadPlaces(this.MaLocation);
        }

        private void AddPlaceClicked()
        {
            OpenAddPlace();
        }

        private async void OpenAddPlace()
        {
            await NavigationService.PushAsync<NewPlace>(new Dictionary<string, object>());
        }

        private void SeeUserInfosClicked()
        {
            OpenUserInfos();
        }

        private async void OpenUserInfos()
        {
            await NavigationService.PushAsync<User>(new Dictionary<string, object>());
        }

    }
}
