using Fourplaces.Models;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class FocusPlaceViewModel : ViewModelBase
    {
        private long _placeId;
        private Place _maPlace;
        private IList<Pin> _pins;

        public ICommand AddCommentaireCommand { get; set; }

        public ICommand RefreshCommand { get; set; }
        private bool _isRefreshing = false;

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set => SetProperty(ref _isRefreshing, value);
        }
        
        public FocusPlaceViewModel()
        {
            this.AddCommentaireCommand = new Command(AddCommentaireClicked);
            this.RefreshCommand = new Command(RefreshClicked);
        }

        private void RefreshClicked()
        {
            RefreshPlace();
            IsRefreshing = false;
        }

        private async void RefreshPlace()
        {
            //await GetLocation();
            //Places = await RestService.Rest.LoadPlaces(MaLocation);
            MaPlace = await RestService.Rest.LoadPlace(PlaceId);
        }

        private void AddCommentaireClicked()
        {
            OpenAddCommentaire();
        }

        private async void OpenAddCommentaire()
        {
            await NavigationService.PushAsync<NewCommentaire>(new Dictionary<string, object>());
        }

        [NavigationParameter]
        public long PlaceId
        {
            get { return _placeId; }
            set
            {
                SetProperty(ref _placeId, value);
            }
        }

        public Place MaPlace
        {
            get { return _maPlace; }
            set
            {
                SetProperty(ref _maPlace, value);
                Pins = new ObservableCollection<Pin>() {
                    new Pin()
                        {
                            Position = MaPlace.Position,
                            Label = MaPlace.Title
                        }
                };
            }
        }

        public IList<Pin> Pins
        {
            get
            {
                return _pins;
            }
            set
            {
                SetProperty(ref _pins, value);
            }
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            MaPlace = await RestService.Rest.LoadPlace(PlaceId);
        }

        public string _titleLabel;

        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }
    }
}
