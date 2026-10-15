using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Location;

namespace GyBlood.Mobile.Services.Presence;

/// <summary>
/// Signale au serveur que le donneur est connecté (RG11) : une position toutes les 2 minutes tant que l'application est ouverte.
/// Android limite l'activité en arrière-plan : hors de l'application, le donneur n'est plus « connecté »
/// et ne reçoit que les notifications push.
/// À FAIRE : n'émettre que si le donneur a activé « disponible » (AvailabilityViewModel, US-27).
/// </summary>
public sealed class PresenceService : IDisposable
{
    private readonly IMeApi _meApi;
    private readonly ILocationService _location;
    private CancellationTokenSource? _cts;

    public PresenceService(IMeApi meApi, ILocationService location)
    {
        _meApi = meApi;
        _location = location;
    }

    public void Start()
    {
        if (_cts is not null)
        {
            return;
        }

        _cts = new CancellationTokenSource();
        _ = RunAsync(_cts.Token);
    }

    public void Stop()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }

    public void Dispose() => Stop();

    private async Task RunAsync(CancellationToken cancellationToken)
    {
        try
        {
            await SendOnceAsync(cancellationToken);
            using var timer = new PeriodicTimer(AppConstants.HeartbeatInterval);
            while (await timer.WaitForNextTickAsync(cancellationToken))
            {
                await SendOnceAsync(cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // arrêt demandé
        }
    }

    private async Task SendOnceAsync(CancellationToken cancellationToken)
    {
        try
        {
            var position = await _location.GetCurrentAsync(cancellationToken);
            if (position is not null)
            {
                await _meApi.SendHeartbeatAsync(new HeartbeatRequest(position.Latitude, position.Longitude));
            }
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception)
        {
            // Réseau instable : on réessaie au prochain passage.
        }
    }
}
