using Storm.Mvvm;
using Fourplaces.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Geolocator;
using System;
using Xamarin.Forms.Maps;
using Plugin.Permissions;
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
            await GetLocation();
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

        private async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentLocation()
        {
            Plugin.Geolocator.Abstractions.Position myPos = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;
                myPos = await locator.GetLastKnownLocationAsync();

                if (myPos != null)
                {
                    return myPos;
                }
                else
                {
                    Console.WriteLine("Erreur, la dernière position connue est nulle ! ");
                    return null;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to get location : " + e.Message);
                return null;
            }
        }

        public async Task GetLocation()
        {
            Plugin.Geolocator.Abstractions.Position myPos = null;
            try
            {
                var status = await CrossPermissions.Current.CheckPermissionStatusAsync(Plugin.Permissions.Abstractions.Permission.Location);

                if (status != Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    if (await CrossPermissions.Current.ShouldShowRequestPermissionRationaleAsync(Plugin.Permissions.Abstractions.Permission.Location))
                    {
                        await Application.Current.MainPage.DisplayAlert("Géolocalisation demandée", "L'application à besoin de votre permission pour vous géolocaliser", "OK");
                    }

                    var results = await CrossPermissions.Current.RequestPermissionsAsync(Plugin.Permissions.Abstractions.Permission.Location);
                    if (results.ContainsKey(Plugin.Permissions.Abstractions.Permission.Location))
                    {
                        status = results[Plugin.Permissions.Abstractions.Permission.Location];
                    }
                }

                if (status == Plugin.Permissions.Abstractions.PermissionStatus.Granted)
                {
                    myPos = await GetCurrentLocation();
                    if (myPos != null)
                    {
                        MaLocation = new Position((float)myPos.Latitude, (float)myPos.Longitude);
                    }
                    else
                    {
                        // Pb avec la geolocalisation ...
                        MaLocation = new Position(0, 0);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Permissions non accordée", "L'application ne peut pas vous géolocaliser en raison d'une permission non accordée", "OK");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
            }
        }

        public override async Task OnResume()
        {
            await base.OnResume();

            await GetLocation();

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
