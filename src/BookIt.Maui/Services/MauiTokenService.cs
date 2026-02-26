using BookIt.Core.DTOs;
using System.Text.Json;

namespace BookIt.Maui.Services;

/// <summary>
/// Persists JWT auth tokens and user details securely using MAUI SecureStorage.
/// </summary>
public class MauiTokenService
{
    private const string AuthKey = "bookit_auth";

    public async Task SaveAuthAsync(AuthResponse auth)
    {
        var json = JsonSerializer.Serialize(auth);
        await SecureStorage.Default.SetAsync(AuthKey, json);
    }

    public async Task<AuthResponse?> GetSavedAuthAsync()
    {
        try
        {
            var json = await SecureStorage.Default.GetAsync(AuthKey);
            if (string.IsNullOrEmpty(json)) return null;
            var auth = JsonSerializer.Deserialize<AuthResponse>(json);
            // Check if token is still valid
            if (auth == null || auth.ExpiresAt <= DateTime.UtcNow.AddMinutes(5))
            {
                await ClearAsync();
                return null;
            }
            return auth;
        }
        catch
        {
            return null;
        }
    }

    public Task ClearAsync()
    {
        SecureStorage.Default.Remove(AuthKey);
        return Task.CompletedTask;
    }
}
