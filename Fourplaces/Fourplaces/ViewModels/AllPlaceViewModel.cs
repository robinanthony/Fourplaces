using Storm.Mvvm;
using Fourplaces.Models;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

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

        public  ObservableCollection<Place> Places
        {
            get => this._allPlaces;
            set => SetProperty(ref this._allPlaces, value);
        }

        public AllPlaceViewModel()
        {
            this.TitleLabel = "Tous les lieux";
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            Places = await RestService.Rest.LoadPlaces();

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
            await NavigationService.PushAsync<FocusPlace>(new Dictionary<string, object>() { { "Data", place} });
        }
    }
}
