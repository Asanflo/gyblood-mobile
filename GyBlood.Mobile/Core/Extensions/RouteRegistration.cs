using GyBlood.Mobile.Core.Constants;

namespace GyBlood.Mobile.Core.Extensions;

public static class RouteRegistration
{
    /// <summary>Enregistre les pages empilées (hors onglets). Appelé une seule fois au démarrage, dans App.</summary>
    public static void RegisterAll()
    {
        Routing.RegisterRoute(AppRoutes.Register, typeof(Views.Auth.RegisterPage));
        Routing.RegisterRoute(AppRoutes.RoleChoice, typeof(Views.Auth.RoleChoicePage));
        Routing.RegisterRoute(AppRoutes.DonorDetail, typeof(Views.Patient.DonorDetailPage));
        Routing.RegisterRoute(AppRoutes.BookAppointment, typeof(Views.Patient.BookAppointmentPage));
        Routing.RegisterRoute(AppRoutes.Payment, typeof(Views.Patient.PaymentPage));
        Routing.RegisterRoute(AppRoutes.RequestDetail, typeof(Views.Donor.RequestDetailPage));
        Routing.RegisterRoute(AppRoutes.Availability, typeof(Views.Donor.AvailabilityPage));
        Routing.RegisterRoute(AppRoutes.CampaignEditor, typeof(Views.Structure.CampaignEditorPage));
        Routing.RegisterRoute(AppRoutes.AccountDetail, typeof(Views.Admin.AccountDetailPage));
        Routing.RegisterRoute(AppRoutes.Funding, typeof(Views.Sponsor.FundingPage));
        Routing.RegisterRoute(AppRoutes.CampaignDetail, typeof(Views.Shared.CampaignDetailPage));
        Routing.RegisterRoute(AppRoutes.Notifications, typeof(Views.Shared.NotificationsPage));
    }
}
