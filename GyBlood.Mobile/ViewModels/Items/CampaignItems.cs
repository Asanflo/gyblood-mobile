using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Navigation;

namespace GyBlood.Mobile.ViewModels.Items;

/// <summary>Fabrique des éléments de liste de campagnes (partagée par tous les rôles).</summary>
public static class CampaignItems
{
    public static CampaignItem From(CampaignDto dto, INavigationService navigation) => new()
    {
        Dto = dto,
        Title = dto.Title,
        Location = dto.Location,
        StructureName = dto.StructureName,
        PeriodText = dto.StartDate.Date == dto.EndDate.Date
            ? Ui.Day(dto.StartDate)
            : $"Du {Ui.Day(dto.StartDate)} au {Ui.Day(dto.EndDate)}",
        TargetText = $"Objectif : {dto.TargetDonations} dons",
        FundsText = dto.FundsReceived > 0 ? $"Financement reçu : {Ui.Money(dto.FundsReceived)}" : string.Empty,
        IsJoined = dto.Joined,
        OpenCommand = new AsyncRelayCommand(() =>
            navigation.GoToAsync(AppRoutes.CampaignDetail, new Dictionary<string, object> { ["campaign"] = dto })),
    };
}
