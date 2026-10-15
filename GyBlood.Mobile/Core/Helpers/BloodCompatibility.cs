namespace GyBlood.Mobile.Core.Helpers;

/// <summary>
/// Compatibilité ABO/Rh pour un don de sang total (globules rouges). La recherche réelle est faite côté serveur ;
/// cette copie sert à contrôler les formulaires et à afficher les groupes acceptés.
/// </summary>
public static class BloodCompatibility
{
    public static bool CanDonate(BloodGroup donorGroup, Rhesus donorRhesus, BloodGroup recipientGroup, Rhesus recipientRhesus)
        => AboCompatible(donorGroup, recipientGroup) && RhesusCompatible(donorRhesus, recipientRhesus);

    /// <summary>Tous les (groupe, rhésus) de donneurs pouvant donner à ce receveur.</summary>
    public static IReadOnlyList<(BloodGroup Group, Rhesus Rhesus)> CompatibleDonorTypes(BloodGroup recipientGroup, Rhesus recipientRhesus)
    {
        var result = new List<(BloodGroup, Rhesus)>();
        foreach (var group in Enum.GetValues<BloodGroup>())
        {
            foreach (var rhesus in Enum.GetValues<Rhesus>())
            {
                if (CanDonate(group, rhesus, recipientGroup, recipientRhesus))
                {
                    result.Add((group, rhesus));
                }
            }
        }

        return result;
    }

    public static string Format(BloodGroup group, Rhesus rhesus)
        => $"{group}{(rhesus == Rhesus.Positive ? "+" : "-")}";

    private static bool AboCompatible(BloodGroup donor, BloodGroup recipient) => donor switch
    {
        BloodGroup.O => true,
        BloodGroup.A => recipient is BloodGroup.A or BloodGroup.AB,
        BloodGroup.B => recipient is BloodGroup.B or BloodGroup.AB,
        BloodGroup.AB => recipient == BloodGroup.AB,
        _ => false,
    };

    // Un donneur Rh négatif peut donner à tous ; un donneur Rh positif seulement à un receveur Rh positif.
    private static bool RhesusCompatible(Rhesus donor, Rhesus recipient)
        => donor == Rhesus.Negative || recipient == Rhesus.Positive;
}
