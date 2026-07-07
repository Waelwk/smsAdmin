using NfcCardManagement.API.Helpers;
using Xunit;

namespace NfcCardManagement.Tests.Helpers;

public class CTagHelperTests
{
    [Fact]
    public void Generate_Length12()
    {
        var ctag = CTagHelper.Generate();
        Assert.Equal(12, ctag.Length);
    }

    [Fact]
    public void Generate_IsUppercase()
    {
        var ctag = CTagHelper.Generate();
        Assert.Equal(ctag.ToUpperInvariant(), ctag);
    }

    [Fact]
    public void Generate_ContainsOnlyHexCharacters()
    {
        for (int i = 0; i < 30; i++)
        {
            var ctag = CTagHelper.Generate();
            Assert.All(ctag, c => Assert.True(
                (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F'),
                $"Caractère hexadécimal invalide : '{c}' dans '{ctag}'"));
        }
    }

    [Fact]
    public void Generate_ProducesDifferentValues()
    {
        var results = Enumerable.Range(0, 20).Select(_ => CTagHelper.Generate()).ToList();
        Assert.True(results.Distinct().Count() > 1);
    }
}
