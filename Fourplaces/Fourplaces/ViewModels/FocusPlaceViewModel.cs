using Fourplaces.Models;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class FocusPlaceViewModel : ViewModelBase
    {
        private long _placeId;
        private Place _maPlace;
        private IList<Pin> _pins;

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
                Pins = new ObservableCollection<Pin>();
                Pins.Add(new Pin()
                {
                    Position = MaPlace.Position,
                    Label = MaPlace.Title
                });
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
