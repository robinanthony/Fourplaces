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
        private string _titleLabel;
        private ObservableCollection<Place> _allPlaces;

        private bool _isVisible = true;
        private bool _isRunning = true;

        public ICommand RefreshCommand { get; set; }
        private bool _isRefreshing = false;

        public ICommand AddPlaceCommand { get; set; }

        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

        public Position MaLocation { get; set; }

        public  ObservableCollection<Place> Places
        {
            get => this._allPlaces;
            set
            {
                SetProperty(ref this._allPlaces, value);
            }
        }

        public AllPlaceViewModel()
        {
            this.TitleLabel = "Tous les lieux";
            this.RefreshCommand = new Command(RefreshClicked);
            this.AddPlaceCommand = new Command(AddPlaceClicked);
        }

        private void RefreshClicked()
        {
            RefreshPlaces();
            IsRefreshing = false;
        }

        private async void RefreshPlaces()
        {
            MaLocation = await MyGeolocator.GetLocation();
            Places = await RestService.Rest.LoadPlaces(MaLocation);
        }

        private void AddPlaceClicked()
        {
            OpenAddPlace();
        }

        private async void OpenAddPlace()
        {
            await NavigationService.PushAsync<NewPlace>(new Dictionary<string, object>());
        }

        public override async Task OnResume()
        {
            await base.OnResume();

            MaLocation = await MyGeolocator.GetLocation();

            Places = await RestService.Rest.LoadPlaces(MaLocation);

            IsVisible = false;
            IsRunning = false;
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            set => SetProperty(ref _isRunning, value);
        }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }

        private Place _currentPlace;

        public Place CurrentPlace
        {
            get => _currentPlace;
            set
            {
                if (SetProperty(ref _currentPlace, value))
                {
                    OpenFocusPlace(_currentPlace);
                }
            }
        }

        public async void OpenFocusPlace(Place place)
        {
            await NavigationService.PushAsync<FocusPlace>(new Dictionary<string, object>() { { "PlaceId", place.Id} });
        }
    }
}
