namespace GyBlood.Mobile.Core.Constants;

/// <summary>
/// Noms des routes Shell. Les routes d'onglets (ShellContent) se joignent avec "//" (ex. "//patient-map") ;
/// les pages « empilées » sont enregistrées dans RouteRegistration et se joignent sans préfixe.
/// </summary>
public static class AppRoutes
{
    // AuthShell
    public const string Welcome = "welcome";
    public const string Login = "login";
    public const string Pending = "pending";

    // Onglets patient
    public const string PatientHome = "patient-home";
    public const string PatientMap = "patient-map";
    public const string PatientCampaigns = "patient-campaigns";
    public const string PatientNews = "patient-news";
    public const string PatientProfile = "patient-profile";

    // Onglets donneur
    public const string DonorHome = "donor-home";
    public const string DonorRequests = "donor-requests";
    public const string DonorCampaigns = "donor-campaigns";
    public const string DonorHistory = "donor-history";
    public const string DonorProfile = "donor-profile";

    // Onglets structure
    public const string StructureHome = "structure-home";
    public const string StructureAppointments = "structure-appointments";
    public const string StructureCampaigns = "structure-campaigns";
    public const string StructureProfile = "structure-profile";

    // Onglets sponsor
    public const string SponsorCampaigns = "sponsor-campaigns";
    public const string SponsorFundings = "sponsor-fundings";
    public const string SponsorProfile = "sponsor-profile";

    // Onglets administrateur
    public const string AdminPending = "admin-pending";
    public const string AdminSettings = "admin-settings";

    // Pages empilées
    public const string Register = "register";
    public const string RoleChoice = "role-choice";
    public const string CompleteDonorProfile = "complete-donor-profile";
    public const string CompleteStructureProfile = "complete-structure-profile";
    public const string DonorDetail = "donor-detail";
    public const string BookAppointment = "book-appointment";
    public const string Payment = "payment";
    public const string Stocks = "stocks";
    public const string MyAppointments = "my-appointments";
    public const string RequestDetail = "request-detail";
    public const string Availability = "availability";
    public const string StockEditor = "stock-editor";
    public const string CampaignEditor = "campaign-editor";
    public const string AccountDetail = "account-detail";
    public const string Funding = "funding";
    public const string CampaignDetail = "campaign-detail";
    public const string Notifications = "notifications";

    /// <summary>Route absolue d'un onglet : "//" + nom.</summary>
    public static string Tab(string route) => "//" + route;
}
