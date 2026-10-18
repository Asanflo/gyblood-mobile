using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Patient;

/// <summary>Paiement des frais d'entretien et d'examens par Mobile Money (MTN MoMo ou Orange Money). (US-15)</summary>
[QueryProperty(nameof(Appointment), "appointment")]
public partial class PaymentViewModel : BaseViewModel
{
    private const int PollAttempts = 20;
    private static readonly TimeSpan PollDelay = TimeSpan.FromSeconds(3);

    private readonly IPaymentApi _paymentApi;
    private readonly INavigationService _navigation;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(DonorName), nameof(StructureName), nameof(DateText), nameof(AmountText), nameof(PayButtonText))]
    private AppointmentDto? appointment;

    [ObservableProperty]
    private PaymentOperator selectedOperator = PaymentOperator.MtnMomo;

    [ObservableProperty]
    private string phoneNumber = string.Empty;

    [ObservableProperty]
    private string resultMessage = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CanEdit))]
    private bool isFinished;

    public PaymentViewModel(IPaymentApi paymentApi, INavigationService navigation)
    {
        _paymentApi = paymentApi;
        _navigation = navigation;
        Title = "Paiement";
    }

    public string DonorName => Appointment?.DonorName ?? string.Empty;

    public string StructureName => Appointment?.StructureName ?? string.Empty;

    public string DateText => Appointment is null ? string.Empty : Ui.DayAndHour(Appointment.ScheduledAt);

    public string AmountText => Appointment?.HospitalFee is { } fee ? Ui.Money(fee) : string.Empty;

    public string PayButtonText => Appointment?.HospitalFee is { } fee ? $"Payer {Ui.Money(fee)}" : "Payer";

    public bool CanEdit => !IsFinished;

    [RelayCommand]
    private async Task PayAsync()
    {
        if (Appointment is null)
        {
            return;
        }

        if (!InputValidator.IsValidPhone(PhoneNumber))
        {
            ErrorMessage = "Saisissez un numéro Mobile Money valide (9 chiffres).";
            return;
        }

        await RunAsync(async () =>
        {
            ResultMessage = string.Empty;
            var payment = await _paymentApi.PayAppointmentAsync(
                Appointment.Id, new CreatePaymentRequest(SelectedOperator, PhoneNumber.Replace(" ", string.Empty)));

            for (var attempt = 0; attempt < PollAttempts && payment.Status == PaymentStatus.Pending; attempt++)
            {
                ResultMessage = "Validez le paiement sur votre téléphone…";
                await Task.Delay(PollDelay);
                payment = await _paymentApi.GetPaymentAsync(payment.Id);
            }

            switch (payment.Status)
            {
                case PaymentStatus.Succeeded:
                    IsFinished = true;
                    ResultMessage = "Paiement confirmé. Votre rendez-vous est validé.";
                    break;
                case PaymentStatus.Pending:
                    IsFinished = true;
                    ResultMessage = "Le paiement n'est pas encore confirmé. Vous serez notifié dès qu'il le sera.";
                    break;
                default:
                    ErrorMessage = "Le paiement a échoué. Vérifiez votre solde et réessayez.";
                    ResultMessage = string.Empty;
                    break;
            }
        });
    }

    [RelayCommand]
    private Task DoneAsync() => _navigation.GoBackAsync();
}
