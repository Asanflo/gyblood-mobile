using CommunityToolkit.Maui.Alerts;

namespace GyBlood.Mobile.Services.Dialogs;

public sealed class DialogService : IDialogService
{
    public Task AlertAsync(string title, string message, string accept = "OK")
        => Shell.Current.DisplayAlert(title, message, accept);

    public Task<bool> ConfirmAsync(string title, string message, string accept = "Oui", string cancel = "Non")
        => Shell.Current.DisplayAlert(title, message, accept, cancel);

    public Task ToastAsync(string message) => Toast.Make(message).Show();
}
