using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.ViewModels.Items;

// Les « éléments » sont ce que les listes affichent : textes déjà formatés + commandes propres à la ligne.
// Cela évite les liaisons vers le ViewModel parent dans les modèles de ligne.

public sealed class AppointmentItem
{
    public required AppointmentDto Dto { get; init; }
    public required string Title { get; init; }
    public required string Subtitle { get; init; }
    public required string DateText { get; init; }
    public required string StatusText { get; init; }
    public required Color StatusColor { get; init; }
    public string FeeText { get; init; } = string.Empty;
    public string PrimaryText { get; init; } = string.Empty;
    public string SecondaryText { get; init; } = string.Empty;
    public ICommand? OpenCommand { get; init; }
    public ICommand? PrimaryCommand { get; init; }
    public ICommand? SecondaryCommand { get; init; }

    public bool HasFee => !string.IsNullOrEmpty(FeeText);
    public bool HasPrimary => !string.IsNullOrEmpty(PrimaryText);
    public bool HasSecondary => !string.IsNullOrEmpty(SecondaryText);
    public bool HasActions => HasPrimary || HasSecondary;
}

public sealed class CampaignItem
{
    public required CampaignDto Dto { get; init; }
    public required string Title { get; init; }
    public required string Location { get; init; }
    public required string StructureName { get; init; }
    public required string PeriodText { get; init; }
    public required string TargetText { get; init; }
    public string FundsText { get; init; } = string.Empty;
    public bool IsJoined { get; init; }
    public ICommand? OpenCommand { get; init; }
}

public sealed class StockItem
{
    public required string StructureName { get; init; }
    public required string BloodText { get; init; }
    public required string QuantityText { get; init; }
    public required string DistanceText { get; init; }
    public required string UpdatedText { get; init; }
    public required string LevelText { get; init; }
    public required Color LevelColor { get; init; }
    public required Color LevelBackground { get; init; }
    public ICommand? ContactCommand { get; init; }
    public bool HasDistance => !string.IsNullOrEmpty(DistanceText);
}

public sealed class StockCell
{
    public required string BloodText { get; init; }
    public required int Quantity { get; init; }
    public string QuantityText => Quantity.ToString();
}

public sealed class DonationItem
{
    public required string StructureName { get; init; }
    public required string DateText { get; init; }
    public string VolumeText { get; init; } = string.Empty;
    public bool HasVolume => !string.IsNullOrEmpty(VolumeText);
}

public sealed class NotificationItem : ObservableObject
{
    private bool _isUnread;

    public required NotificationDto Dto { get; init; }
    public required string Title { get; init; }
    public required string Message { get; init; }
    public required string DateText { get; init; }
    public ICommand? OpenCommand { get; init; }

    public bool IsUnread
    {
        get => _isUnread;
        set => SetProperty(ref _isUnread, value);
    }
}

public sealed class FundingItem
{
    public required string CampaignTitle { get; init; }
    public required string AmountText { get; init; }
    public required string DateText { get; init; }
    public required string StatusText { get; init; }
    public required Color StatusColor { get; init; }
}

public sealed class AccountItem
{
    public required AccountSummaryDto Dto { get; init; }
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string RoleText { get; init; }
    public required string DateText { get; init; }
    public required string StatusText { get; init; }
    public required Color StatusColor { get; init; }
    public ICommand? OpenCommand { get; init; }
}

public sealed class DonorItem
{
    public required DonorDto Dto { get; init; }
    public required string Name { get; init; }
    public required string BloodText { get; init; }
    public required string DistanceText { get; init; }
    public ICommand? OpenCommand { get; init; }
}

/// <summary>Une structure proposée dans la liste déroulante de prise de rendez-vous (ToString = texte affiché).</summary>
public sealed class StructureOption(StructureDto dto)
{
    public StructureDto Dto { get; } = dto;

    public override string ToString()
        => Dto.DistanceKm is { } km ? $"{Dto.Name} ({Ui.Distance(km)})" : Dto.Name;
}

/// <summary>Carte de choix du rôle (écran « Quel est votre profil ? »).</summary>
public sealed partial class RoleCardItem : ObservableObject
{
    public required UserRole Role { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public string IconSource { get; init; } = string.Empty;
    public ICommand? SelectCommand { get; init; }

    [ObservableProperty]
    private bool isSelected;
}

/// <summary>Ligne de l'éditeur de stocks : un groupe sanguin et sa quantité.</summary>
public sealed partial class StockEditRow : ObservableObject
{
    public required BloodGroup Group { get; init; }
    public required Rhesus Rhesus { get; init; }
    public string BloodText => BloodCompatibility.Format(Group, Rhesus);

    [ObservableProperty]
    private int quantity;

    [RelayCommand]
    private void Increase() => Quantity = Math.Min(Quantity + 1, 100_000);

    [RelayCommand]
    private void Decrease() => Quantity = Math.Max(Quantity - 1, 0);
}

/// <summary>Ligne des paramètres administrateur : délai minimal entre deux dons.</summary>
public sealed partial class IntervalItem : ObservableObject
{
    public required Sex Sex { get; init; }
    public required DonationType DonationType { get; init; }
    public string Label => $"{Ui.SexLabel(Sex)} · {TypeText}";

    private string TypeText => DonationType switch
    {
        DonationType.WholeBlood => "Sang total",
        DonationType.Plasma => "Plasma",
        DonationType.Platelets => "Plaquettes",
        _ => DonationType.ToString(),
    };

    [ObservableProperty]
    private string days = string.Empty;
}

/// <summary>Choix rapide d'un montant (écran de financement).</summary>
public sealed class AmountChip
{
    public required int Amount { get; init; }
    public string Text => Ui.Money(Amount);
    public ICommand? SelectCommand { get; init; }
}
