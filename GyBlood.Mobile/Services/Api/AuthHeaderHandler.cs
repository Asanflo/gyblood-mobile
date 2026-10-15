using System.Net;
using System.Net.Http.Headers;
using CommunityToolkit.Mvvm.Messaging;
using GyBlood.Mobile.Services.Auth;

namespace GyBlood.Mobile.Services.Api;

/// <summary>
/// Ajoute le JWT à chaque requête. Sur 401, tente un renouvellement avec le jeton de rafraîchissement puis rejoue la requête.
/// Si le renouvellement échoue, la session est effacée et un SessionExpiredMessage est publié.
/// Limite : les envois multipart (document de groupe sanguin) ne sont pas rejoués automatiquement.
/// </summary>
public sealed class AuthHeaderHandler : DelegatingHandler
{
    private static readonly SemaphoreSlim RefreshLock = new(1, 1);

    private readonly TokenStore _tokens;
    private readonly IAuthApi _authApi;

    public AuthHeaderHandler(TokenStore tokens, IAuthApi authApi)
    {
        _tokens = tokens;
        _authApi = authApi;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var access = await _tokens.GetAccessTokenAsync();
        if (!string.IsNullOrEmpty(access))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", access);
        }

        // On garde une copie du contenu avant l'envoi : une requête ne peut pas être envoyée deux fois.
        var retry = await TryCloneAsync(request);
        var response = await base.SendAsync(request, cancellationToken);

        if (response.StatusCode != HttpStatusCode.Unauthorized || retry is null)
        {
            return response;
        }

        var newAccess = await RefreshAsync(access, cancellationToken);
        if (newAccess is null)
        {
            retry.Dispose();
            return response;
        }

        response.Dispose();
        retry.Headers.Authorization = new AuthenticationHeaderValue("Bearer", newAccess);
        return await base.SendAsync(retry, cancellationToken);
    }

    private async Task<string?> RefreshAsync(string? usedAccessToken, CancellationToken cancellationToken)
    {
        await RefreshLock.WaitAsync(cancellationToken);
        try
        {
            // Un autre appel a peut-être déjà renouvelé le jeton pendant qu'on attendait.
            var current = await _tokens.GetAccessTokenAsync();
            if (!string.IsNullOrEmpty(current) && current != usedAccessToken)
            {
                return current;
            }

            var refresh = await _tokens.GetRefreshTokenAsync();
            if (string.IsNullOrEmpty(refresh))
            {
                ExpireSession();
                return null;
            }

            try
            {
                var response = await _authApi.RefreshAsync(new RefreshRequest(refresh));
                await _tokens.SaveAsync(response.AccessToken, response.RefreshToken);
                return response.AccessToken;
            }
            catch (Refit.ApiException)
            {
                ExpireSession();
                return null;
            }
        }
        finally
        {
            RefreshLock.Release();
        }
    }

    private void ExpireSession()
    {
        _tokens.Clear();
        WeakReferenceMessenger.Default.Send(new SessionExpiredMessage());
    }

    private static async Task<HttpRequestMessage?> TryCloneAsync(HttpRequestMessage request)
    {
        if (request.Content is MultipartContent)
        {
            return null;
        }

        var clone = new HttpRequestMessage(request.Method, request.RequestUri);
        foreach (var header in request.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content is not null)
        {
            var bytes = await request.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(bytes);
            foreach (var header in request.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return clone;
    }
}
