using GyBlood.Mobile.Core.Constants;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.Services.Auth;
using GyBlood.Mobile.Services.Dialogs;
using GyBlood.Mobile.Services.Location;
using GyBlood.Mobile.Services.Navigation;
using GyBlood.Mobile.Services.Notifications;
using GyBlood.Mobile.Services.Presence;
using Microsoft.Extensions.DependencyInjection;
using Refit;

namespace GyBlood.Mobile.Core.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>Clients Refit : IAuthApi sans jeton, tous les autres avec AuthHeaderHandler.</summary>
    public static IServiceCollection AddApiClients(this IServiceCollection services)
    {
        var settings = RefitSettingsFactory.Create();

        services.AddTransient<AuthHeaderHandler>();
        services.AddRefitClient<IAuthApi>(settings).ConfigureHttpClient(Configure);
        services.AddAuthorizedClient<IMeApi>(settings);
        services.AddAuthorizedClient<IPatientApi>(settings);
        services.AddAuthorizedClient<IDonorApi>(settings);
        services.AddAuthorizedClient<IAppointmentApi>(settings);
        services.AddAuthorizedClient<IPaymentApi>(settings);
        services.AddAuthorizedClient<IStructureApi>(settings);
        services.AddAuthorizedClient<ICampaignApi>(settings);
        services.AddAuthorizedClient<ISponsorApi>(settings);
        services.AddAuthorizedClient<INotificationApi>(settings);
        services.AddAuthorizedClient<IAdminApi>(settings);

        return services;
    }

    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<TokenStore>();
        services.AddSingleton<IAuthService, AuthService>();
        services.AddSingleton<ILocationService, LocationService>();
        services.AddSingleton<IPushService, PushService>();
        services.AddSingleton<PresenceService>();
        services.AddSingleton<RoleShellSelector>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        return services;
    }

    public static IServiceCollection AddViewModelsAndPages(this IServiceCollection services)
    {
        services.AddTransient<ViewModels.Auth.RoleChoiceViewModel>();
        services.AddTransient<Views.Auth.RoleChoicePage>();
        services.AddTransient<ViewModels.Patient.StocksViewModel>();
        services.AddTransient<Views.Patient.StocksPage>();
        services.AddTransient<ViewModels.Patient.PatientProfileViewModel>();
        services.AddTransient<Views.Patient.PatientProfilePage>();
        services.AddTransient<ViewModels.Patient.DonorMapViewModel>();
        services.AddTransient<Views.Patient.DonorMapPage>();
        services.AddTransient<ViewModels.Patient.DonorDetailViewModel>();
        services.AddTransient<Views.Patient.DonorDetailPage>();
        services.AddTransient<ViewModels.Patient.BookAppointmentViewModel>();
        services.AddTransient<Views.Patient.BookAppointmentPage>();
        services.AddTransient<ViewModels.Patient.PaymentViewModel>();
        services.AddTransient<Views.Patient.PaymentPage>();
        services.AddTransient<ViewModels.Patient.MyAppointmentsViewModel>();
        services.AddTransient<Views.Patient.MyAppointmentsPage>();
        services.AddTransient<ViewModels.Donor.DonorProfileViewModel>();
        services.AddTransient<Views.Donor.DonorProfilePage>();
        services.AddTransient<ViewModels.Donor.RequestsViewModel>();
        services.AddTransient<Views.Donor.RequestsPage>();
        services.AddTransient<ViewModels.Donor.RequestDetailViewModel>();
        services.AddTransient<Views.Donor.RequestDetailPage>();
        services.AddTransient<ViewModels.Donor.DonationHistoryViewModel>();
        services.AddTransient<Views.Donor.DonationHistoryPage>();
        services.AddTransient<ViewModels.Donor.DonorCampaignsViewModel>();
        services.AddTransient<Views.Donor.DonorCampaignsPage>();
        services.AddTransient<ViewModels.Donor.AvailabilityViewModel>();
        services.AddTransient<Views.Donor.AvailabilityPage>();
        services.AddTransient<ViewModels.Structure.StructureProfileViewModel>();
        services.AddTransient<Views.Structure.StructureProfilePage>();
        services.AddTransient<ViewModels.Structure.StockEditorViewModel>();
        services.AddTransient<Views.Structure.StockEditorPage>();
        services.AddTransient<ViewModels.Structure.StructureAppointmentsViewModel>();
        services.AddTransient<Views.Structure.StructureAppointmentsPage>();
        services.AddTransient<ViewModels.Structure.CampaignEditorViewModel>();
        services.AddTransient<Views.Structure.CampaignEditorPage>();
        services.AddTransient<ViewModels.Structure.StructureCampaignsViewModel>();
        services.AddTransient<Views.Structure.StructureCampaignsPage>();
        services.AddTransient<ViewModels.Sponsor.SponsorCampaignsViewModel>();
        services.AddTransient<Views.Sponsor.SponsorCampaignsPage>();
        services.AddTransient<ViewModels.Sponsor.FundingViewModel>();
        services.AddTransient<Views.Sponsor.FundingPage>();
        services.AddTransient<ViewModels.Sponsor.MyFundingsViewModel>();
        services.AddTransient<Views.Sponsor.MyFundingsPage>();
        services.AddTransient<ViewModels.Sponsor.SponsorProfileViewModel>();
        services.AddTransient<Views.Sponsor.SponsorProfilePage>();
        services.AddTransient<ViewModels.Admin.PendingAccountsViewModel>();
        services.AddTransient<Views.Admin.PendingAccountsPage>();
        services.AddTransient<ViewModels.Admin.AccountDetailViewModel>();
        services.AddTransient<Views.Admin.AccountDetailPage>();
        services.AddTransient<ViewModels.Admin.SettingsViewModel>();
        services.AddTransient<Views.Admin.SettingsPage>();
        services.AddTransient<ViewModels.Shared.NotificationsViewModel>();
        services.AddTransient<Views.Shared.NotificationsPage>();
        services.AddTransient<ViewModels.Shared.CampaignDetailViewModel>();
        services.AddTransient<Views.Shared.CampaignDetailPage>();
        services.AddTransient<ViewModels.Auth.LoginViewModel>();
        services.AddTransient<Views.Auth.LoginPage>();
        services.AddTransient<ViewModels.Auth.RegisterViewModel>();
        services.AddTransient<Views.Auth.RegisterPage>();
        services.AddTransient<ViewModels.Auth.PendingAccountViewModel>();
        services.AddTransient<Views.Auth.PendingAccountPage>();
        services.AddTransient<ViewModels.Auth.WelcomeViewModel>();
        services.AddTransient<Views.Auth.WelcomePage>();
        services.AddTransient<ViewModels.Patient.PatientHomeViewModel>();
        services.AddTransient<Views.Patient.PatientHomePage>();
        services.AddTransient<ViewModels.Shared.CampaignsViewModel>();
        services.AddTransient<Views.Shared.CampaignsPage>();
        services.AddTransient<ViewModels.Donor.DonorHomeViewModel>();
        services.AddTransient<Views.Donor.DonorHomePage>();
        services.AddTransient<ViewModels.Structure.StructureDashboardViewModel>();
        services.AddTransient<Views.Structure.StructureDashboardPage>();
        services.AddTransient<ViewModels.Shared.StartupViewModel>();
        services.AddTransient<Views.Shared.StartupPage>();
        return services;
    }

    public static IServiceCollection AddShells(this IServiceCollection services)
    {
        services.AddTransient<AppShell>();
        services.AddTransient<Views.Shells.AuthShell>();
        services.AddTransient<Views.Shells.PatientShell>();
        services.AddTransient<Views.Shells.DonorShell>();
        services.AddTransient<Views.Shells.StructureShell>();
        services.AddTransient<Views.Shells.SponsorShell>();
        services.AddTransient<Views.Shells.AdminShell>();
        return services;
    }

    private static void AddAuthorizedClient<T>(this IServiceCollection services, RefitSettings settings)
        where T : class
        => services.AddRefitClient<T>(settings)
            .ConfigureHttpClient(Configure)
            .AddHttpMessageHandler<AuthHeaderHandler>();

    private static void Configure(HttpClient client)
    {
        client.BaseAddress = new Uri(AppConstants.ApiBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(AppConstants.ClientTimeoutSeconds);
    }
}