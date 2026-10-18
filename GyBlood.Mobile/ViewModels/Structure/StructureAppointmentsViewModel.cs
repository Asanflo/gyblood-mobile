using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Structure;

/// <summary>Rendez-vous de don reçus par la structure : confirmer le don effectué ou signaler une absence. (US-26)</summary>
public partial class StructureAppointmentsViewModel : BaseViewModel
{
    private readonly IAppointmentApi _api;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    private bool isEmpty;

    public StructureAppointmentsViewModel(IAppointmentApi api, IDialogService dialogs)
    {
        _api = api;
        _dialogs = dialogs;
        Title = "Rendez-vous";
    }

    public ObservableCollection<AppointmentItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(ReloadAsync);

    private async Task ReloadAsync()
    {
        var page = await _api.GetAppointmentsAsync(size: 50);
        Items.Clear();
        foreach (var dto in page.Content.OrderBy(a => a.Status is AppointmentStatus.Completed or AppointmentStatus.Cancelled or AppointmentStatus.NoShow).ThenBy(a => a.ScheduledAt))
        {
            Items.Add(Build(dto));
        }

        IsEmpty = Items.Count == 0;
    }

    private AppointmentItem Build(AppointmentDto dto)
    {
        var actionable = dto.Status is AppointmentStatus.Accepted or AppointmentStatus.Paid;
        return new AppointmentItem
        {
            Dto = dto,
            Title = $"Don : {dto.DonorName}",
            Subtitle = "Patient : " + dto.PatientName,
            DateText = Ui.DayAndHour(dto.ScheduledAt),
            StatusText = Ui.AppointmentStatusText(dto.Status),
            StatusColor = Ui.StatusColor(dto.Status),
            FeeText = dto.HospitalFee is > 0 ? $"Frais : {Ui.Money(dto.HospitalFee.Value)}" : string.Empty,
            PrimaryText = actionable ? "Don effectué" : string.Empty,
            SecondaryText = actionable ? "Absent" : string.Empty,
            PrimaryCommand = new AsyncRelayCommand(() => CompleteAsync(dto)),
            SecondaryCommand = new AsyncRelayCommand(() => NoShowAsync(dto)),
        };
    }

    private async Task CompleteAsync(AppointmentDto dto)
    {
        if (!await _dialogs.ConfirmAsync("Don effectué", $"Confirmer que {dto.DonorName} a bien donné son sang ?", "Confirmer", "Retour"))
        {
            return;
        }

        await RunAsync(async () =>
        {
            await _api.CompleteAsync(dto.Id);
            await ReloadAsync();
        });
    }

    private async Task NoShowAsync(AppointmentDto dto)
    {
        if (!await _dialogs.ConfirmAsync("Donneur absent", "Le donneur ne s'est pas présenté. Les frais payés ne sont pas remboursés automatiquement.", "Signaler", "Retour"))
        {
            return;
        }

        await RunAsync(async () =>
        {
            await _api.NoShowAsync(dto.Id);
            await ReloadAsync();
        });
    }
}
