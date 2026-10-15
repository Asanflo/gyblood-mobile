namespace GyBlood.Mobile.Tests.Fakes;

public class FakeApiTests
{
    [Fact]
    public async Task Registering_a_donor_gives_a_pending_account()
    {
        var response = await new FakeAuthApi().RegisterAsync(new RegisterRequest("d@test.cm", "12345678", UserRole.Donor));

        Assert.Equal(AccountStatus.Pending, response.User.Status);
    }

    [Fact]
    public async Task Registering_a_patient_gives_a_validated_account()
    {
        var response = await new FakeAuthApi().RegisterAsync(new RegisterRequest("p@test.cm", "12345678", UserRole.Patient));

        Assert.Equal(AccountStatus.Validated, response.User.Status);
    }

    [Fact]
    public async Task Availability_is_kept_by_the_fake_donor_api()
    {
        var api = new FakeDonorApi();

        await api.SetAvailabilityAsync(new AvailabilityRequest(true, true));

        Assert.True((await api.GetMeAsync()).IsAvailable);
    }
}
