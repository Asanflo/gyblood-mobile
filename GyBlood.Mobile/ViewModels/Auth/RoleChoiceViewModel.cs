using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Auth;

/// <summary>« Quel est votre profil ? » : choix du rôle avant la création du compte. (US-01, US-02)</summary>
public partial class RoleChoiceViewModel : BaseViewModel
{
    private readonly INavigationService _navigation;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasSelection))]
    private RoleCardItem? selected;

    public RoleChoiceViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        Title = "Votre profil";

        Roles =
        [
            Card(UserRole.Patient, "role_patient", "Patient / Chercheur de sang", "Demandez du sang et trouvez des donneurs compatibles près de chez vous."),
            Card(UserRole.Donor, "role_donor", "Donneur", "Donnez et recevez les demandes des patients à proximité, suivez vos dons."),
            Card(UserRole.Sponsor, "role_sponsor", "Particulier / Sponsor", "Soutenez les campagnes de don et suivez vos financements."),
            Card(UserRole.Structure, "role_structure", "Banque de sang / Hôpital", "Publiez vos stocks, organisez des campagnes et recevez les rendez-vous."),
        ];
    }

    public ObservableCollection<RoleCardItem> Roles { get; }

    public bool HasSelection => Selected is not null;

    private RoleCardItem Card(UserRole role, string icon, string title, string description)
    {
        RoleCardItem? card = null;
        card = new RoleCardItem
        {
            Role = role,
            Title = title,
            Description = description,
            IconSource = icon,
            SelectCommand = new RelayCommand(() => Select(card!)),
        };
        return card;
    }

    private void Select(RoleCardItem card)
    {
        foreach (var role in Roles)
        {
            role.IsSelected = ReferenceEquals(role, card);
        }

        Selected = card;
    }

    [RelayCommand]
    private async Task ContinueAsync()
    {
        if (Selected is null)
        {
            ErrorMessage = "Choisissez un profil pour continuer.";
            return;
        }

        await _navigation.GoToAsync(AppRoutes.Register, new Dictionary<string, object> { ["role"] = Selected.Role });
    }
}