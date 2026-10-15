using GyBlood.Mobile.Core.Extensions;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Message affiché quand une liste est vide (« Aucun donneur à proximité », etc.).</summary>
public sealed class EmptyState : ContentView
{
    public static readonly BindableProperty MessageProperty =
        BindableProperty.Create(nameof(Message), typeof(string), typeof(EmptyState), "Rien à afficher pour le moment.",
            propertyChanged: (b, _, n) => ((EmptyState)b)._label.Text = n as string);

    private readonly Label _label = new()
    {
        Text = "Rien à afficher pour le moment.",
        HorizontalTextAlignment = TextAlignment.Center,
        TextColor = Application.Current.GetColor("Gray600", Colors.Gray),
    };

    public EmptyState()
    {
        Padding = new Thickness(24);
        Content = _label;
    }

    public string Message
    {
        get => (string)GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
}
