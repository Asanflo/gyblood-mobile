namespace GyBlood.Mobile.Services.Dialogs;

public interface IDialogService
{
    Task AlertAsync(string title, string message, string accept = "OK");

    Task<bool> ConfirmAsync(string title, string message, string accept = "Oui", string cancel = "Non");

    Task ToastAsync(string message);
}
