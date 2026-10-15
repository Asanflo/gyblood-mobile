using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Tests.Helpers;

public class InputValidatorTests
{
    [Theory]
    [InlineData("florentin@example.com", true)]
    [InlineData("  florentin@example.com", false)]
    [InlineData("florentin@", false)]
    [InlineData("florentin", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidEmail(string? email, bool expected)
        => Assert.Equal(expected, InputValidator.IsValidEmail(email));

    [Theory]
    [InlineData("12345678", true)]
    [InlineData("1234567", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidPassword(string? password, bool expected)
        => Assert.Equal(expected, InputValidator.IsValidPassword(password));

    [Theory]
    [InlineData("697117458", true)]
    [InlineData("+237 697 117 458", true)]
    [InlineData("237697117458", true)]
    [InlineData("233123456", true)]
    [InlineData("12345", false)]
    [InlineData("797117458", false)]
    [InlineData("", false)]
    public void IsValidPhone(string? phone, bool expected)
        => Assert.Equal(expected, InputValidator.IsValidPhone(phone));
}
