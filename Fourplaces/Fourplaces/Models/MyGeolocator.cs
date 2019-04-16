using Plugin.Geolocator;
using Plugin.Permissions;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Fourplaces.Models
{
    public static class MyGeolocator
    {
        private static async Task<Plugin.Geolocator.Abstractions.Position> GetCurrentLocation()
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

        public static async Task<Position> GetLocation()
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
                        return new Position((float)myPos.Latitude, (float)myPos.Longitude);
                    }
                    else
                    {
                        // Pb avec la geolocalisation ...
                        return new Position(0, 0);
                    }
                }
                else
                {
                    return new Position(0, 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Erreur : " + e.Message);
                return new Position(0, 0);
            }
        }
    }
}
