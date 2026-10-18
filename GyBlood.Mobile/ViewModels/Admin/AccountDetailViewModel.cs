using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Admin;

/// <summary>Fiche d'un compte : justificatif, validation, refus motivé, suspension. (US-37, US-38)</summary>
[QueryProperty(nameof(Account), "account")]
public partial class AccountDetailViewModel : BaseViewModel
{
    private readonly IAdminApi _api;
    private readonly IDialogService _dialogs;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Name), nameof(EmailText), nameof(RoleText), nameof(StatusText), nameof(StatusColor), nameof(CanApprove), nameof(CanReject), nameof(CanSuspend), nameof(CanReactivate), nameof(HasDocument))]
    private AccountSummaryDto? account;

    [ObservableProperty]
    private bool isRejecting;

    [ObservableProperty]
    private string reasonText = string.Empty;

    public AccountDetailViewModel(IAdminApi api, IDialogService dialogs)
    {
        _api = api;
        _dialogs = dialogs;
        Title = "Compte";
    }

    public string Name => string.IsNullOrWhiteSpace(Account?.DisplayName) ? "Sans nom" : Account!.DisplayName!;

    public string EmailText => Account?.Email ?? string.Empty;

    public string RoleText => Account is null ? string.Empty : Ui.RoleText(Account.Role);

    public string StatusText => Account is null ? string.Empty : Ui.AccountStatusText(Account.Status);

    public Color StatusColor => Account is null ? Colors.Gray : Ui.StatusColor(Account.Status);

    public bool CanApprove => Account is { Status: AccountStatus.Pending or AccountStatus.Rejected };

    public bool CanReject => Account is { Status: AccountStatus.Pending };

    public bool CanSuspend => Account is { Status: AccountStatus.Validated, Role: not UserRole.Admin };

    public bool CanReactivate => Account is { Status: AccountStatus.Suspended };

    public bool HasDocument => Account is { Role: UserRole.Donor };

    [RelayCommand]
    private Task ViewDocumentAsync() => RunAsync(async () =>
    {
        if (Account is null)
        {
            return;
        }

        await using var stream = await _api.GetDonorBloodDocumentAsync(Account.Id);
        using var buffer = new MemoryStream();
        await stream.CopyToAsync(buffer);
        var bytes = buffer.ToArray();
        if (bytes.Length == 0)
        {
            ErrorMessage = "Aucun justificatif n'a été envoyé.";
            return;
        }

        var path = Path.Combine(FileSystem.CacheDirectory, $"justificatif-{Account.Id}{ExtensionOf(bytes)}");
        await File.WriteAllBytesAsync(path, bytes);
        await Launcher.Default.OpenAsync(new OpenFileRequest("Justificatif de groupe sanguin", new ReadOnlyFile(path)));
    });

    [RelayCommand]
    private Task ApproveAsync() => RunAsync(async () =>
    {
        if (Account is null)
        {
            return;
        }

        await _api.ApproveAsync(Account.Id);
        Account = Account with { Status = AccountStatus.Validated };
        IsRejecting = false;
        await _dialogs.ToastAsync("Compte validé");
    });

    [RelayCommand]
    private void ShowReject()
    {
        ErrorMessage = null;
        IsRejecting = true;
    }

    [RelayCommand]
    private void CancelReject()
    {
        IsRejecting = false;
        ReasonText = string.Empty;
    }

    [RelayCommand]
    private async Task ConfirmRejectAsync()
    {
        if (string.IsNullOrWhiteSpace(ReasonText))
        {
            ErrorMessage = "Indiquez le motif du refus : il sera communiqué à l'utilisateur.";
            return;
        }

        await RunAsync(async () =>
        {
            if (Account is null)
            {
                return;
            }

            await _api.RejectAsync(Account.Id, new RejectAccountRequest(ReasonText.Trim()));
            Account = Account with { Status = AccountStatus.Rejected };
            IsRejecting = false;
            ReasonText = string.Empty;
            await _dialogs.ToastAsync("Compte refusé");
        });
    }

    [RelayCommand]
    private async Task SuspendAsync()
    {
        if (!await _dialogs.ConfirmAsync("Suspendre le compte", "L'utilisateur ne pourra plus utiliser l'application. Continuer ?", "Suspendre", "Retour"))
        {
            return;
        }

        await RunAsync(async () =>
        {
            if (Account is null)
            {
                return;
            }

            await _api.SuspendAsync(Account.Id);
            Account = Account with { Status = AccountStatus.Suspended };
        });
    }

    [RelayCommand]
    private Task ReactivateAsync() => RunAsync(async () =>
    {
        if (Account is null)
        {
            return;
        }

        await _api.ReactivateAsync(Account.Id);
        Account = Account with { Status = AccountStatus.Validated };
    });

    private static string ExtensionOf(byte[] bytes)
    {
        if (bytes.Length > 4 && bytes[0] == 0x25 && bytes[1] == 0x50 && bytes[2] == 0x44 && bytes[3] == 0x46)
        {
            return ".pdf";
        }

        if (bytes.Length > 4 && bytes[0] == 0x89 && bytes[1] == 0x50)
        {
            return ".png";
        }

        return ".jpg";
    }
}
