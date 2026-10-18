using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Disponibilité : « je suis disponible » et partage de la position (visible des patients à proximité). (US-27)</summary>
public partial class AvailabilityViewModel : BaseViewModel
{
    private readonly IDonorApi _api;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Hint))]
    private bool isAvailable;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Hint))]
    private bool shareLocation;

    public AvailabilityViewModel(IDonorApi api, IDialogService dialogs)
    {
        _api = api;
        _dialogs = dialogs;
        Title = "Disponibilité";
    }

    public string Hint => (IsAvailable, ShareLocation) switch
    {
        (true, true) => "Les patients compatibles proches de vous peuvent vous voir sur la carte.",
        (true, false) => "Vous n'apparaîtrez pas sur la carte tant que le partage de position est désactivé.",
        _ => "Vous ne recevrez pas de nouvelles demandes.",
    };

    public override Task OnAppearingAsync() => RunAsync(async () =>
    {
        var profile = await _api.GetMeAsync();
        IsAvailable = profile.IsAvailable;
        ShareLocation = profile.ShareLocation;
    });

    [RelayCommand]
    private Task SaveAsync() => RunAsync(async () =>
    {
        var profile = await _api.SetAvailabilityAsync(new AvailabilityRequest(IsAvailable, ShareLocation));
        IsAvailable = profile.IsAvailable;
        ShareLocation = profile.ShareLocation;
        await _dialogs.ToastAsync("Disponibilité enregistrée");
    });
}
