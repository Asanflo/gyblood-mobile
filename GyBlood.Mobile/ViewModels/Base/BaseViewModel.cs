using CommunityToolkit.Mvvm.ComponentModel;
using GyBlood.Mobile.Core.Extensions;
using Refit;

namespace GyBlood.Mobile.ViewModels.Base;

public abstract partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string? errorMessage;

    public bool IsNotBusy => !IsBusy;

    /// <summary>Appelé par la page à chaque affichage : charger les données ici.</summary>
    public virtual Task OnAppearingAsync() => Task.CompletedTask;

    /// <summary>Exécute une action avec indicateur d'activité et traduction des erreurs réseau en message lisible.</summary>
    protected async Task RunAsync(Func<Task> action)
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            ErrorMessage = null;
            await action();
        }
        catch (ApiException ex)
        {
            ErrorMessage = ex.ToUserMessage();
        }
        catch (InvalidOperationException ex)
        {
            ErrorMessage = ex.Message;
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Connexion impossible. Vérifiez votre réseau.";
        }
        catch (TaskCanceledException)
        {
            ErrorMessage = "Le serveur met trop de temps à répondre. Réessayez.";
        }
        finally
        {
            IsBusy = false;
        }
    }
}
