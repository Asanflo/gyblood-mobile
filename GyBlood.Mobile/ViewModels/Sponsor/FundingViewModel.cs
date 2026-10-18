using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Helpers;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Sponsor;

/// <summary>Financement d'une campagne par Mobile Money (montant libre ou rapide, MTN MoMo ou Orange Money). (US-34)</summary>
[QueryProperty(nameof(Campaign), "campaign")]
public partial class FundingViewModel : BaseViewModel
{
    private const int MinimumAmount = 500;

    private readonly ISponsorApi _api;
    private readonly INavigationService _navigation;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(CampaignTitle), nameof(StructureName))]
    private CampaignDto? campaign;

    [ObservableProperty]
    private string amountText = string.Empty;

    [ObservableProperty]
    private PaymentOperator selectedOperator = PaymentOperator.MtnMomo;

    [ObservableProperty]
    private string phoneNumber = string.Empty;

    public FundingViewModel(ISponsorApi api, INavigationService navigation, IDialogService dialogs)
    {
        _api = api;
        _navigation = navigation;
        _dialogs = dialogs;
        Title = "Financer";

        foreach (var amount in new[] { 1_000, 5_000, 10_000, 25_000, 50_000 })
        {
            var value = amount;
            Chips.Add(new AmountChip { Amount = value, SelectCommand = new RelayCommand(() => AmountText = value.ToString()) });
        }
    }

    public ObservableCollection<AmountChip> Chips { get; } = [];

    public string CampaignTitle => Campaign?.Title ?? string.Empty;

    public string StructureName => Campaign?.StructureName ?? string.Empty;

    [RelayCommand]
    private async Task PayAsync()
    {
        if (Campaign is null)
        {
            return;
        }

        if (!int.TryParse(AmountText.Replace(" ", string.Empty), out var amount) || amount < MinimumAmount)
        {
            ErrorMessage = $"Le montant minimum est de {Ui.Money(MinimumAmount)}.";
            return;
        }

        if (!InputValidator.IsValidPhone(PhoneNumber))
        {
            ErrorMessage = "Saisissez un numéro Mobile Money valide (9 chiffres).";
            return;
        }

        await RunAsync(async () =>
        {
            var funding = await _api.FundCampaignAsync(Campaign.Id, new FundCampaignRequest(amount, SelectedOperator, PhoneNumber.Replace(" ", string.Empty)));
            switch (funding.Status)
            {
                case PaymentStatus.Succeeded:
                    await _dialogs.AlertAsync("Merci !", $"Votre financement de {Ui.Money(funding.Amount)} a bien été reçu.");
                    await _navigation.GoBackAsync();
                    break;
                case PaymentStatus.Pending:
                    await _dialogs.AlertAsync("Paiement en cours", "Validez la demande sur votre téléphone. Le statut sera visible dans « Mes financements ».");
                    await _navigation.GoBackAsync();
                    break;
                default:
                    ErrorMessage = "Le paiement a échoué. Vérifiez votre solde et réessayez.";
                    break;
            }
        });
    }
}
