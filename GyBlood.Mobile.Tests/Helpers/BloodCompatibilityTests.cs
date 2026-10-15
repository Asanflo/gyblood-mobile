using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Tests.Helpers;

public class BloodCompatibilityTests
{
    [Theory]
    [InlineData(BloodGroup.O, Rhesus.Negative, BloodGroup.A, Rhesus.Positive, true)]
    [InlineData(BloodGroup.O, Rhesus.Negative, BloodGroup.AB, Rhesus.Negative, true)]
    [InlineData(BloodGroup.A, Rhesus.Positive, BloodGroup.AB, Rhesus.Positive, true)]
    [InlineData(BloodGroup.A, Rhesus.Positive, BloodGroup.A, Rhesus.Negative, false)]
    [InlineData(BloodGroup.A, Rhesus.Negative, BloodGroup.A, Rhesus.Positive, true)]
    [InlineData(BloodGroup.B, Rhesus.Positive, BloodGroup.A, Rhesus.Positive, false)]
    [InlineData(BloodGroup.AB, Rhesus.Positive, BloodGroup.O, Rhesus.Negative, false)]
    [InlineData(BloodGroup.AB, Rhesus.Negative, BloodGroup.AB, Rhesus.Negative, true)]
    [InlineData(BloodGroup.O, Rhesus.Positive, BloodGroup.O, Rhesus.Negative, false)]
    public void CanDonate_follows_abo_and_rhesus_rules(BloodGroup dg, Rhesus dr, BloodGroup rg, Rhesus rr, bool expected)
        => Assert.Equal(expected, BloodCompatibility.CanDonate(dg, dr, rg, rr));

    [Fact]
    public void Receiver_ABpositive_accepts_all_eight_types()
        => Assert.Equal(8, BloodCompatibility.CompatibleDonorTypes(BloodGroup.AB, Rhesus.Positive).Count);

    [Fact]
    public void Receiver_Onegative_accepts_only_Onegative()
    {
        var types = BloodCompatibility.CompatibleDonorTypes(BloodGroup.O, Rhesus.Negative);

        var single = Assert.Single(types);
        Assert.Equal((BloodGroup.O, Rhesus.Negative), single);
    }

    [Theory]
    [InlineData(BloodGroup.A, Rhesus.Positive, "A+")]
    [InlineData(BloodGroup.O, Rhesus.Negative, "O-")]
    public void Format_writes_group_then_sign(BloodGroup group, Rhesus rhesus, string expected)
        => Assert.Equal(expected, BloodCompatibility.Format(group, rhesus));
}
