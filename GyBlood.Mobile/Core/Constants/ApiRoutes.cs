namespace GyBlood.Mobile.Core.Constants;

/// <summary>Chemins de l'API (section 5 du cahier des charges), relatifs à AppConstants.ApiBaseUrl : pas de "/" initial.</summary>
public static class ApiRoutes
{
    public static class Auth
    {
        public const string Register = "auth/register";
        public const string Login = "auth/login";
        public const string Google = "auth/google";
        public const string GoogleExchange = "auth/google/exchange";
        public const string Refresh = "auth/refresh";
        public const string Logout = "auth/logout";
    }

    public static class Me
    {
        public const string Root = "me";
        public const string Devices = "me/devices";
        public const string Heartbeat = "me/heartbeat";
    }

    public static class Patients
    {
        public const string Me = "patients/me";
        public const string SearchDonors = "donors/search";
        public const string NearestStructures = "structures/nearest";
    }

    public static class Donors
    {
        public const string Me = "donors/me";
        public const string BloodDocument = "donors/me/blood-document";
        public const string Availability = "donors/me/availability";
        public const string Eligibility = "donors/me/eligibility";
        public const string Donations = "donors/me/donations";
        public const string Campaigns = "donors/me/campaigns";
    }

    public static class Appointments
    {
        public const string Root = "appointments";
        public const string ById = "appointments/{id}";
        public const string Accept = "appointments/{id}/accept";
        public const string Decline = "appointments/{id}/decline";
        public const string Cancel = "appointments/{id}/cancel";
        public const string Complete = "appointments/{id}/complete";
        public const string NoShow = "appointments/{id}/no-show";
        public const string Payments = "appointments/{id}/payments";
    }

    public static class Payments
    {
        public const string ById = "payments/{id}";
    }

    public static class Structures
    {
        public const string Me = "structures/me";
        public const string List = "structures";
        public const string Stocks = "stocks";
        public const string MyStocks = "structures/me/stocks";
    }

    public static class Campaigns
    {
        public const string Root = "campaigns";
        public const string ById = "campaigns/{id}";
        public const string Join = "campaigns/{id}/join";
        public const string Fundings = "campaigns/{id}/fundings";
    }

    public static class Sponsors
    {
        public const string Me = "sponsors/me";
        public const string Fundings = "sponsors/me/fundings";
    }

    public static class Notifications
    {
        public const string Root = "notifications";
        public const string Read = "notifications/{id}/read";
    }

    public static class Admin
    {
        public const string Accounts = "admin/accounts";
        public const string DonorBloodDocument = "admin/donors/{id}/blood-document";
        public const string Approve = "admin/accounts/{id}/approve";
        public const string Reject = "admin/accounts/{id}/reject";
        public const string Suspend = "admin/accounts/{id}/suspend";
        public const string Reactivate = "admin/accounts/{id}/reactivate";
        public const string Intervals = "admin/settings/donation-intervals";
    }
}
