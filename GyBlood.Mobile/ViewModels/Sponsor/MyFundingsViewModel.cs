using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Historique des financements du sponsor. (US-35)</summary>
public partial class MyFundingsViewModel : BaseViewModel
{
    private readonly ISponsorApi _api;

    [ObservableProperty]
    private string totalText = string.Empty;

    public MyFundingsViewModel(ISponsorApi api)
    {
        _api = api;
        Title = "Mes financements";
    }

    public ObservableCollection<FundingItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var page = await _api.GetFundingsAsync(size: 50);
        Items.Clear();
        foreach (var dto in page.Content)
        {
            Items.Add(new FundingItem
            {
                CampaignTitle = dto.CampaignTitle,
                AmountText = Ui.Money(dto.Amount),
                DateText = Ui.Day(dto.CreatedAt),
                StatusText = Ui.PaymentStatusText(dto.Status),
                StatusColor = Ui.StatusColor(dto.Status),
            });
        }

        var total = page.Content.Where(f => f.Status == PaymentStatus.Succeeded).Sum(f => f.Amount);
        TotalText = total > 0 ? $"Total financé : {Ui.Money(total)}" : string.Empty;
    });
}
