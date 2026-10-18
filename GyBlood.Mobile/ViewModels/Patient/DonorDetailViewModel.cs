using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Fiche d'un donneur : nom, groupe, distance (rien de plus, RG3). Reçoit le donneur par la navigation.</summary>
[QueryProperty(nameof(Donor), "donor")]
public partial class DonorDetailViewModel : BaseViewModel
{
    private readonly INavigationService _navigation;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name), nameof(BloodText), nameof(DistanceText))]
    private DonorDto? donor;

    public DonorDetailViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        Title = "Donneur";
    }

    public string Name => Donor?.FullName ?? string.Empty;

    public string BloodText => Donor is null ? string.Empty : BloodCompatibility.Format(Donor.BloodGroup, Donor.Rhesus);

    public string DistanceText => Donor is null ? string.Empty : $"À {Ui.Distance(Donor.DistanceKm)} de vous";

    [RelayCommand]
    private async Task RequestDonationAsync()
    {
        if (Donor is null)
        {
            return;
        }

        await _navigation.GoToAsync(AppRoutes.BookAppointment, new Dictionary<string, object> { ["donor"] = Donor });
    }
}
