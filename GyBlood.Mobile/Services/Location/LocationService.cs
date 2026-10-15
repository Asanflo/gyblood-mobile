using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Services.Location;

public sealed class LocationService : ILocationService
{
    public async Task<GeoPoint?> GetCurrentAsync(CancellationToken cancellationToken = default)
    {
        // Les demandes de permission doivent s'exécuter sur le thread principal.
        var status = await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            var current = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            return current == PermissionStatus.Granted
                ? current
                : await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
        });

        if (status != PermissionStatus.Granted)
        {
            return null;
        }

        try
        {
            var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
            var location = await Geolocation.Default.GetLocationAsync(request, cancellationToken);
            return location is null ? null : new GeoPoint(location.Latitude, location.Longitude);
        }
        catch (FeatureNotEnabledException)
        {
            return null; // GPS désactivé
        }
    }
}
