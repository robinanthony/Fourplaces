using Fourplaces.Models;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    public class FocusPlaceViewModel : ViewModelBase
    {
//==============================================================================
//================================= ATTRIBUTS ==================================
//==============================================================================
        public ICommand AddCommentaireCommand { get; private set; }

        public ICommand RefreshCommand { get; private set; }

        private bool _isRefreshing = false;
        public bool IsRefreshing
        {
            get => this._isRefreshing;
            set => SetProperty(ref this._isRefreshing, value);
        }

        private long _placeId;
        [NavigationParameter]
        public long PlaceId
        {
            get { return this._placeId; }
            set
            {
                SetProperty(ref this._placeId, value);
            }
        }

        private Place _maPlace;
        public Place MaPlace
        {
            get { return this._maPlace; }
            set
            {
                SetProperty(ref this._maPlace, value);
                this.Pins = new ObservableCollection<Pin>() {
                    new Pin()
                        {
                            Position = this.MaPlace.Position,
                            Label = this.MaPlace.Title
                        }
                };
            }
        }

        private IList<Pin> _pins;
        public IList<Pin> Pins
        {
            get
            {
                return this._pins;
            }
            set
            {
                SetProperty(ref this._pins, value);
            }
        }

        public string _titleLabel;
        public string TitleLabel
        {
            get => this._titleLabel;
            set => SetProperty(ref this._titleLabel, value);
        }

//==============================================================================
//============================== FCT PRINCIPALES ===============================
//==============================================================================
        public FocusPlaceViewModel()
        {
            this.AddCommentaireCommand = new Command(this.AddCommentaireClicked);
            this.RefreshCommand = new Command(this.RefreshClicked);
        }

        public override async Task OnResume()
        {
            await base.OnResume();
            this.MaPlace = await RestService.Rest.LoadPlace(this.PlaceId);
        }

//==============================================================================
//============================== FCT SECONDAIRES ===============================
//==============================================================================
        private void RefreshClicked()
        {
            RefreshPlace();
            this.IsRefreshing = false;
        }

        private async void RefreshPlace()
        {
            this.MaPlace = await RestService.Rest.LoadPlace(this.PlaceId);
        }

        private void AddCommentaireClicked()
        {
            OpenAddCommentaire();
        }

        private async void OpenAddCommentaire()
        {
            await NavigationService.PushAsync<NewCommentaire>(new Dictionary<string, object>() { { "PlaceId", this.MaPlace.Id } });
        }

    }
}
