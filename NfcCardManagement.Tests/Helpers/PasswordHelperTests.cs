using NfcCardManagement.API.Helpers;
using Xunit;

namespace NfcCardManagement.Tests.Helpers;

public class PasswordHelperTests
{
    [Fact]
    public void Generate_LengthIsBetween8And12()
    {
        for (int i = 0; i < 50; i++)
        {
            var pwd = PasswordHelper.Generate();
            Assert.InRange(pwd.Length, 8, 12);
        }
    }

    [Fact]
    public void Generate_ContainsOnlyAlphanumericCharacters()
    {
        for (int i = 0; i < 50; i++)
        {
            var pwd = PasswordHelper.Generate();
            Assert.All(pwd, c => Assert.True(char.IsLetterOrDigit(c),
                $"Caractère non alphanumérique trouvé : '{c}' dans '{pwd}'"));
        }
    }

    [Fact]
    public void Generate_ProducesDifferentValues()
    {
        var results = Enumerable.Range(0, 20).Select(_ => PasswordHelper.Generate()).ToList();
        // Au moins quelques valeurs distinctes (pas toutes identiques)
        Assert.True(results.Distinct().Count() > 1);
    }
}
