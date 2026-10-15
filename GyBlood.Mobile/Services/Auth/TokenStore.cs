namespace GyBlood.Mobile.Services.Auth;

/// <summary>Stockage chiffré (Android Keystore) du JWT et du jeton de rafraîchissement.</summary>
public sealed class TokenStore
{
    private const string AccessKey = "gyblood.access_token";
    private const string RefreshKey = "gyblood.refresh_token";

    public Task<string?> GetAccessTokenAsync() => ReadAsync(AccessKey);

    public Task<string?> GetRefreshTokenAsync() => ReadAsync(RefreshKey);

    public async Task SaveAsync(string accessToken, string refreshToken)
    {
        await SecureStorage.Default.SetAsync(AccessKey, accessToken);
        await SecureStorage.Default.SetAsync(RefreshKey, refreshToken);
    }

    public void Clear()
    {
        SecureStorage.Default.Remove(AccessKey);
        SecureStorage.Default.Remove(RefreshKey);
    }

    private static async Task<string?> ReadAsync(string key)
    {
        try
        {
            return await SecureStorage.Default.GetAsync(key);
        }
        catch (Exception)
        {
            // Le Keystore peut être invalidé (restauration, changement de verrouillage) : on repart d'une session vide.
            SecureStorage.Default.RemoveAll();
            return null;
        }
    }
}
