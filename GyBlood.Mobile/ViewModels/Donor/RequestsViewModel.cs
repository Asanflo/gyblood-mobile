using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Donor;

/// <summary>Demandes de don reçues par le donneur : accepter ou refuser. (US-22, US-23)</summary>
public partial class RequestsViewModel : BaseViewModel
{
    private readonly IAppointmentApi _api;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    private bool isEmpty;

    public RequestsViewModel(IAppointmentApi api, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Demandes";
    }

    public ObservableCollection<AppointmentItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(ReloadAsync);

    private async Task ReloadAsync()
    {
        var page = await _api.GetAppointmentsAsync(size: 50);
        Items.Clear();
        foreach (var dto in page.Content.OrderByDescending(a => a.Status == AppointmentStatus.Requested).ThenByDescending(a => a.ScheduledAt))
        {
            Items.Add(Build(dto));
        }

        IsEmpty = Items.Count == 0;
    }

    private AppointmentItem Build(AppointmentDto dto)
    {
        var pending = dto.Status == AppointmentStatus.Requested;
        return new AppointmentItem
        {
            Dto = dto,
            Title = "Demande de don de sang",
            Subtitle = dto.StructureName,
            DateText = Ui.DayAndHour(dto.ScheduledAt),
            StatusText = Ui.AppointmentStatusText(dto.Status),
            StatusColor = Ui.StatusColor(dto.Status),
            PrimaryText = pending ? "Accepter" : string.Empty,
            SecondaryText = pending ? "Refuser" : string.Empty,
            OpenCommand = new AsyncRelayCommand(() =>
                _navigation.GoToAsync(AppRoutes.RequestDetail, new Dictionary<string, object> { ["appointment"] = dto })),
            PrimaryCommand = new AsyncRelayCommand(() => AcceptAsync(dto)),
            SecondaryCommand = new AsyncRelayCommand(() => DeclineAsync(dto)),
        };
    }

    private async Task AcceptAsync(AppointmentDto dto)
    {
        await RunAsync(async () =>
        {
            await _api.AcceptAsync(dto.Id);
            await ReloadAsync();
        });

        if (string.IsNullOrEmpty(ErrorMessage))
        {
            await _dialogs.ToastAsync("Demande acceptée");
        }
    }

    private async Task DeclineAsync(AppointmentDto dto)
    {
        if (!await _dialogs.ConfirmAsync("Refuser la demande", "Voulez-vous refuser cette demande de don ?", "Refuser", "Retour"))
        {
            return;
        }

        await RunAsync(async () =>
        {
            await _api.DeclineAsync(dto.Id);
            await ReloadAsync();
        });
    }
}
