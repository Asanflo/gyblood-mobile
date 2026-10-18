using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>« Mon activité » : prochain don possible et dons passés. (US-24, US-26)</summary>
public partial class DonationHistoryViewModel : BaseViewModel
{
    private readonly IDonorApi _api;

    [ObservableProperty]
    private string nextTitle = "Prochain don de sang";

    [ObservableProperty]
    private string nextText = string.Empty;

    [ObservableProperty]
    private string countText = string.Empty;

    public DonationHistoryViewModel(IDonorApi api)
    {
        _api = api;
        Title = "Mon activité";
    }

    public ObservableCollection<DonationItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var eligibility = await _api.GetEligibilityAsync();
        NextTitle = eligibility.CanDonate ? "Vous pouvez donner" : "Prochain don de sang";
        NextText = eligibility.CanDonate
            ? "Dès maintenant"
            : DateHelper.NextDonationText(eligibility.NextDonationDate);

        var page = await _api.GetDonationsAsync(size: 50);
        Items.Clear();
        foreach (var donation in page.Content)
        {
            Items.Add(new DonationItem
            {
                StructureName = donation.StructureName,
                DateText = Ui.Day(donation.DonatedAt),
                VolumeText = donation.VolumeMl is { } ml ? $"{ml} ml" : string.Empty,
            });
        }

        CountText = Items.Count switch
        {
            0 => "Aucun don enregistré",
            1 => "1 don effectué",
            _ => $"{Items.Count} dons effectués",
        };
    });
}
