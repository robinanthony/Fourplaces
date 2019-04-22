using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.MyUI
{
    public class BindableMap : Map
    {

        public static readonly BindableProperty MapPinsProperty = BindableProperty.Create(
                    nameof(MapPins),
                    typeof(ObservableCollection<Pin>),
                    typeof(BindableMap),
                    new ObservableCollection<Pin>(),
                    propertyChanged: (b, o, n) =>
                    {
                        var bindable = (BindableMap)b;
                        bindable.Pins.Clear();

                        var collection = (ObservableCollection<Pin>)n;
                        if (collection == null) return;
                        foreach (var item in collection)
                            bindable.Pins.Add(item);
                        collection.CollectionChanged += (sender, e) =>
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                switch (e.Action)
                                {
                                    case NotifyCollectionChangedAction.Add:
                                    case NotifyCollectionChangedAction.Replace:
                                    case NotifyCollectionChangedAction.Remove:
                                        if (e.OldItems != null)
                                            foreach (var item in e.OldItems)
                                                bindable.Pins.Remove((Pin)item);
                                        if (e.NewItems != null)
                                            foreach (var item in e.NewItems)
                                                bindable.Pins.Add((Pin)item);
                                        break;
                                    case NotifyCollectionChangedAction.Reset:
                                        bindable.Pins.Clear();
                                        break;
                                }
                            });
                        };
                    });
        public ObservableCollection<Pin> MapPins
        {
            get => (ObservableCollection<Pin>)GetValue(MapPinsProperty);
            set => SetValue(MapPinsProperty, value);
        }

        public static readonly BindableProperty MapPositionProperty = BindableProperty.Create(
                    nameof(MapPosition),
                    typeof(Position),
                    typeof(BindableMap),
                    new Position(0, 0),
                    propertyChanged: (b, o, n) =>
                    {
                        ((BindableMap)b).MoveToRegion(MapSpan.FromCenterAndRadius(
                            (Position)n,
                            Distance.FromMiles(1)));
                    });

        public Position MapPosition {
            get => (Position)GetValue(MapPositionProperty);
            set => SetValue(MapPositionProperty, value);
        }
    }
}
