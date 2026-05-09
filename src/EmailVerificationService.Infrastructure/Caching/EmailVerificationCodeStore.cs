using EmailVerificationService.Application.Abstractions;
using EmailVerificationService.Contracts.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace EmailVerificationService.Infrastructure.Caching;
// Stores verificationcodes temporarily in RAM-minnet with ASP.NET's built-in memory cache.
public sealed class EmailVerificationCodeStore(IMemoryCache cache) : IEmailVerificationCodeStore
{
    // Bygger en unik cache-nyckel. När ett EmailVerificationCodeEntry record cachas, kommer den att få en nyckel som är baserad på emailen. Emailen kan sedan sökas på för att hämta/ta bort cachningen.
    private static string GetCacheKey(string email) => $"email-verification:{email}";

    public async Task<bool> CacheCodeAsync(EmailVerificationCodeEntry entry, CancellationToken ct = default)
    {
        // skapar nyckeln och beräknar hur länge posten ska finnas i cachen baserat på utgångstiden
        string cacheKey = GetCacheKey(entry.Email);
        TimeSpan expiresAfter = entry.ExpiresAtUtc - DateTimeOffset.UtcNow;
        if (expiresAfter <= TimeSpan.Zero)
            return false;

        // recorden försvinner automatiskt från cachen när expiresAfter har passerat. 
        cache.Set(cacheKey, entry, expiresAfter);

        return true;
    }

    public Task<EmailVerificationCodeEntry?> GetCodeAsync(string email, CancellationToken ct = default)
    {
        string cacheKey = GetCacheKey(email);

        cache.TryGetValue(cacheKey, out EmailVerificationCodeEntry? entry);

        // Skapar direkt en färdig Task som innehåller resultatet. IMemory är synkront, så resultatet kan returneras direkt som en Task. Interfacet är async för att möjliggöra framtida implementeringar som kanske inte är synkrona.
        return Task.FromResult(entry);
    }

    public Task RemoveCodeAsync(string email, CancellationToken ct = default)
    {
        string cacheKey = GetCacheKey(email);
        cache.Remove(cacheKey);

        return Task.CompletedTask;
    }
}
