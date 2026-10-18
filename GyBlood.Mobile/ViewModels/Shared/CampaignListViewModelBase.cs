using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Shared;

/// <summary>Liste de campagnes commune au patient, au donneur, au sponsor et à la structure.</summary>
public abstract partial class CampaignListViewModelBase : BaseViewModel
{
    private readonly ICampaignApi _api;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    private bool isEmpty;

    protected CampaignListViewModelBase(ICampaignApi api, INavigationService navigation)
    {
        _api = api;
        _navigation = navigation;
    }

    public ObservableCollection<CampaignItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    /// <summary>Permet à une sous-classe de restreindre la liste (ex. la structure ne voit que ses campagnes).</summary>
    protected virtual bool Keep(CampaignDto campaign) => true;

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var page = await _api.GetCampaignsAsync(size: 50);
        Items.Clear();
        foreach (var dto in page.Content.Where(Keep))
        {
            Items.Add(CampaignItems.From(dto, _navigation));
        }

        IsEmpty = Items.Count == 0;
    });
}
