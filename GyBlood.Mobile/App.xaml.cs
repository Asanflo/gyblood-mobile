using GyBlood.Mobile.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace GyBlood.Mobile;

public partial class App : Application
{
    private readonly IServiceProvider _services;

    public App(IServiceProvider services)
    {
        InitializeComponent();
        _services = services;
        RouteRegistration.RegisterAll();
    }

    protected override Window CreateWindow(IActivationState? activationState)
        => new(_services.GetRequiredService<AppShell>());
}
