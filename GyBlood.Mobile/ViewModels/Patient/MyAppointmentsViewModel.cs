using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Mes rendez-vous : suivi des demandes, paiement et annulation. (US-16)</summary>
public partial class MyAppointmentsViewModel : BaseViewModel
{
    private readonly IAppointmentApi _api;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    private bool isEmpty;

    public MyAppointmentsViewModel(IAppointmentApi api, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Mes rendez-vous";
    }

    public ObservableCollection<AppointmentItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var page = await _api.GetAppointmentsAsync(size: 50);
        Items.Clear();
        foreach (var dto in page.Content)
        {
            Items.Add(Build(dto));
        }

        IsEmpty = Items.Count == 0;
    });

    private AppointmentItem Build(AppointmentDto dto)
    {
        var canPay = dto.Status == AppointmentStatus.Accepted && dto.HospitalFee is > 0;
        var canCancel = dto.Status is AppointmentStatus.Requested or AppointmentStatus.Accepted or AppointmentStatus.Paid;

        return new AppointmentItem
        {
            Dto = dto,
            Title = $"Don avec {dto.DonorName}",
            Subtitle = dto.StructureName,
            DateText = Ui.DayAndHour(dto.ScheduledAt),
            StatusText = Ui.AppointmentStatusText(dto.Status),
            StatusColor = Ui.StatusColor(dto.Status),
            FeeText = dto.HospitalFee is > 0 ? $"Frais : {Ui.Money(dto.HospitalFee.Value)}" : string.Empty,
            PrimaryText = canPay ? "Payer" : string.Empty,
            SecondaryText = canCancel ? "Annuler" : string.Empty,
            PrimaryCommand = new AsyncRelayCommand(() =>
                _navigation.GoToAsync(AppRoutes.Payment, new Dictionary<string, object> { ["appointment"] = dto })),
            SecondaryCommand = new AsyncRelayCommand(() => CancelAsync(dto)),
        };
    }

    private async Task CancelAsync(AppointmentDto dto)
    {
        var message = dto.Status == AppointmentStatus.Paid
            ? "Ce rendez-vous est déjà payé. Le remboursement sera traité par l'administration. Annuler quand même ?"
            : "Voulez-vous annuler cette demande ?";

        if (!await _dialogs.ConfirmAsync("Annuler le rendez-vous", message, "Annuler le rendez-vous", "Garder"))
        {
            return;
        }

        await RunAsync(async () =>
        {
            await _api.CancelAsync(dto.Id);
            var page = await _api.GetAppointmentsAsync(size: 50);
            Items.Clear();
            foreach (var item in page.Content)
            {
                Items.Add(Build(item));
            }

            IsEmpty = Items.Count == 0;
        });
    }
}
