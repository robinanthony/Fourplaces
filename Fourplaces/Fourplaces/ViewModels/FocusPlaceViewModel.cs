using Fourplaces.Models;
using Storm.Mvvm;
using Storm.Mvvm.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using Xamarin.Forms.Maps;

namespace Fourplaces.ViewModels
{
    class FocusPlaceViewModel : ViewModelBase
    {
        private Place _data;
        private IList<Pin> _pins;

        [NavigationParameter]
        public Place Data
        {
            get { return _data; }
            set
            {
                SetProperty(ref _data, value);
                Pins = new ObservableCollection<Pin>();
                Pins.Add(new Pin() {
                    Position = Data.Position,
                    Label = Data.Title});
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
    }
}
