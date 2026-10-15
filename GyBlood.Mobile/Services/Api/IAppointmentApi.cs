using GyBlood.Mobile.Core.Constants;
using Refit;

namespace GyBlood.Mobile.Services.Api;


public interface IAppointmentApi
{
    /// <summary>Crée le rendez-vous au statut Requested et notifie le donneur (US-13 et US-14).</summary>
    [Post(ApiRoutes.Appointments.Root)]
    Task<AppointmentDto> CreateAsync([Body] CreateAppointmentRequest request);

    [Get(ApiRoutes.Appointments.Root)]
    Task<PageResponse<AppointmentDto>> GetAppointmentsAsync(AppointmentStatus? status = null, int page = 0, int size = 20);

    [Get(ApiRoutes.Appointments.ById)]
    Task<AppointmentDto> GetAsync(EntityId id);

    [Post(ApiRoutes.Appointments.Accept)]
    Task<AppointmentDto> AcceptAsync(EntityId id);

    [Post(ApiRoutes.Appointments.Decline)]
    Task<AppointmentDto> DeclineAsync(EntityId id);

    [Post(ApiRoutes.Appointments.Cancel)]
    Task<AppointmentDto> CancelAsync(EntityId id);

    [Post(ApiRoutes.Appointments.Complete)]
    Task<AppointmentDto> CompleteAsync(EntityId id);

    [Post(ApiRoutes.Appointments.NoShow)]
    Task<AppointmentDto> NoShowAsync(EntityId id);
}
