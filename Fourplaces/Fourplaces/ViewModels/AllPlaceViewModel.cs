using Storm.Mvvm;
using Fourplaces.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using Xamarin.Forms;
using Plugin.Geolocator;
using System;
using Xamarin.Forms.Maps;


namespace Fourplaces.ViewModels
{

    class AllPlaceViewModel : ViewModelBase
    {
        private string _titleLabel;
        private ObservableCollection<Place> _allPlaces;

        private bool _isVisible = true;
        private bool _isRunning = true;

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
                // TODO : Pas très propre. Se serait mieux de faire cela dans la fonction RestService.LoadPlaces(Position MaLocation) ...
                foreach (Place p in value)
                {
                    p.Distance = GetDistanceBetweenPositions(p.Position, MaLocation);
                }
                SetProperty(ref this._allPlaces, value);
            }
        }

        public AllPlaceViewModel()
        {
            this.TitleLabel = "Tous les lieux";
        }

        public override async Task OnResume()
        {
            await base.OnResume();

            // TODO : Semble ne pas fonctionner sur une version >= 23 ...
            // Une erreur concernant un manque de permission apparait lors de l'appel de fonction.
            // Comment puis-je résoudre ça ? '.'

            // Voir https://bitbucket.org/WhyNotPH/xamarinproject/src/master/projet/VIEWMODELS/VMLieu.cs : peut être un truc à tirer d'interessant
            var res = await CrossGeolocator.Current.GetPositionAsync();
            MaLocation = new Position(res.Latitude, res.Longitude);

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

        private int GetDistanceBetweenPositions( Position source, Position dest)
        {
            int R = 6378;

            double SourceLat = GetRadian(source.Latitude);
            double SourceLong = GetRadian(source.Longitude);
            double DestLat = GetRadian(dest.Latitude);
            double DestLong = GetRadian(dest.Longitude);

            return (int)(R * (Math.PI / 2 - Math.Asin(Math.Sin(DestLat) * Math.Sin(SourceLat) + Math.Cos(DestLong - SourceLong) * Math.Cos(DestLat) * Math.Cos(SourceLat))));
        }

        private double GetRadian(double degree)
        {
            return Math.PI * degree / 180;
        }
    }
}
