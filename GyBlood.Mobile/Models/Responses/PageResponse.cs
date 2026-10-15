namespace GyBlood.Mobile.Models.Responses;

/// <summary>Page renvoyée par Spring Data (content, totalElements, totalPages, number, size).</summary>
public sealed record PageResponse<T>(
    List<T> Content,
    long TotalElements,
    int TotalPages,
    int Number,
    int Size);
