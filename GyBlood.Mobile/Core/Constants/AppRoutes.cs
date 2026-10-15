namespace GyBlood.Mobile.Core.Constants;

/// <summary>Noms des routes Shell. Les pages « empilées » sont enregistrées dans RouteRegistration.</summary>
public static class AppRoutes
{
    // Routes des ShellContent (AuthShell)
    public const string Login = "login";
    public const string Pending = "pending";

    // Pages empilées
    public const string Register = "register";
    public const string RoleChoice = "role-choice";
    public const string DonorDetail = "donor-detail";
    public const string BookAppointment = "book-appointment";
    public const string Payment = "payment";
    public const string RequestDetail = "request-detail";
    public const string Availability = "availability";
    public const string CampaignEditor = "campaign-editor";
    public const string AccountDetail = "account-detail";
    public const string Funding = "funding";
    public const string CampaignDetail = "campaign-detail";
    public const string Notifications = "notifications";
}
