using Microsoft.Maui.Handlers;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>
/// Retire le soulignement natif d'Android sur les Entry, Editor et Picker : les champs des maquettes sont des cases grises arrondies
/// (voir FormField / PickerField). À appeler une fois dans MauiProgram.
/// </summary>
public static class EntryHandlerSetup
{
    public static void Apply()
    {
#if ANDROID
        EntryHandler.Mapper.AppendToMapping("GyBloodNoUnderline", (handler, _) =>
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
        PickerHandler.Mapper.AppendToMapping("GyBloodNoUnderline", (handler, _) =>
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
        EditorHandler.Mapper.AppendToMapping("GyBloodNoUnderline", (handler, _) =>
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent));
#endif
    }
}
