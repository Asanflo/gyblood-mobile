using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Tests.Helpers;

public class DateHelperTests
{
    private static readonly DateTimeOffset Now = new(2026, 10, 14, 8, 0, 0, TimeSpan.Zero);

    [Fact]
    public void NextDonationText_without_date_means_eligible()
        => Assert.Contains("dès maintenant", DateHelper.NextDonationText(null, Now));

    [Fact]
    public void NextDonationText_in_the_past_means_eligible()
        => Assert.Contains("dès maintenant", DateHelper.NextDonationText(Now.AddDays(-1), Now));

    [Fact]
    public void NextDonationText_counts_remaining_days()
        => Assert.StartsWith("Dans 12 jours", DateHelper.NextDonationText(Now.AddDays(12), Now));
}
