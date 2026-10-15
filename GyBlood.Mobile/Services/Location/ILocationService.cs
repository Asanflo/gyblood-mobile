using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Services.Location;

public interface ILocationService
{
    /// <summary>Position actuelle, ou null si la permission est refusée ou le GPS indisponible.</summary>
    Task<GeoPoint?> GetCurrentAsync(CancellationToken cancellationToken = default);
}
