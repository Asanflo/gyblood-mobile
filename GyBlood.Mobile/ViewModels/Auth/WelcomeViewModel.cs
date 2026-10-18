using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.ViewModels.Base;

namespace GyBlood.Mobile.ViewModels.Auth;

/// <summary>Écran d'accueil (premier écran des maquettes) : « Commencer » ou « Se connecter ».</summary>
public partial class WelcomeViewModel : BaseViewModel
{
    private readonly INavigationService _navigation;

    public WelcomeViewModel(INavigationService navigation)
    {
        _navigation = navigation;
        Title = "Bienvenue";
    }

    [RelayCommand]
    private Task StartAsync() => _navigation.GoToAsync(AppRoutes.RoleChoice);

    [RelayCommand]
    private Task LoginAsync() => _navigation.GoToAsync(AppRoutes.Tab(AppRoutes.Login));
}
