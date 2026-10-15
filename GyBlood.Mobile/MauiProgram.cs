using CommunityToolkit.Maui;
using GyBlood.Mobile.Core.Extensions;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace GyBlood.Mobile;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseSkiaSharp(); // requis par Mapsui

        builder.Services
            .AddApiClients()
            .AddAppServices()
            .AddViewModelsAndPages()
            .AddShells();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
