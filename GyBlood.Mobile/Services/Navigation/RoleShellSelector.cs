using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Views.Shells;
using Microsoft.Extensions.DependencyInjection;

namespace GyBlood.Mobile.Services.Navigation;

/// <summary>Choisit le Shell selon le rôle et l'état de validation du compte (voir section 6.3).</summary>
public sealed class RoleShellSelector
{
    private readonly IServiceProvider _services;

    public RoleShellSelector(IServiceProvider services) => _services = services;

    public (Shell Shell, string? StartRoute) Resolve(UserDto? user)
    {
        if (user is null)
        {
            return (_services.GetRequiredService<AuthShell>(), null);
        }

        // Compte en attente, refusé ou suspendu : on reste sur l'écran « Compte en attente ».
        if (user.Status != AccountStatus.Validated)
        {
            return (_services.GetRequiredService<AuthShell>(), "//" + AppRoutes.Pending);
        }

        Shell shell = user.Role switch
        {
            UserRole.Patient => _services.GetRequiredService<PatientShell>(),
            UserRole.Donor => _services.GetRequiredService<DonorShell>(),
            UserRole.Structure => _services.GetRequiredService<StructureShell>(),
            UserRole.Sponsor => _services.GetRequiredService<SponsorShell>(),
            UserRole.Admin => _services.GetRequiredService<AdminShell>(),
            _ => _services.GetRequiredService<AuthShell>(),
        };

        return (shell, null);
    }
}
