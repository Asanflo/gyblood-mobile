namespace GyBlood.Mobile.Models.Responses;

public sealed record CampaignDto(
    EntityId Id,
    EntityId StructureId,
    string StructureName,
    string Title,
    string Location,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int TargetDonations,
    decimal FundsReceived,
    bool Joined,
    string? Description);
