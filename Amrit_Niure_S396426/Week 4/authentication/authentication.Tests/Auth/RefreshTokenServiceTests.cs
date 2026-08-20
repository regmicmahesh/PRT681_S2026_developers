using authentication.Auth;
using Xunit;

namespace authentication.Tests.Auth;

public class RefreshTokenServiceTests
{
    [Fact]
    public void Hash_Is_Deterministic_For_Same_Input()
    {
        var rawToken = RefreshTokenService.GenerateRawToken();

        var hash1 = RefreshTokenService.Hash(rawToken);
        var hash2 = RefreshTokenService.Hash(rawToken);

        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void Hash_Differs_For_Different_Inputs()
    {
        var hash1 = RefreshTokenService.Hash(RefreshTokenService.GenerateRawToken());
        var hash2 = RefreshTokenService.Hash(RefreshTokenService.GenerateRawToken());

        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void GenerateRawToken_Produces_Unique_Values()
    {
        var tokens = Enumerable.Range(0, 1000)
            .Select(_ => RefreshTokenService.GenerateRawToken())
            .ToHashSet();

        Assert.Equal(1000, tokens.Count);
    }
}
