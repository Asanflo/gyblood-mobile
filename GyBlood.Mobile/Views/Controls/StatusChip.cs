using GyBlood.Mobile.Core.Extensions;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Étiquette colorée pour un statut (voir StatusToColorConverter). Liaison : Text et ChipColor.</summary>
public sealed class StatusChip : ContentView
{
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(StatusChip), string.Empty,
            propertyChanged: (b, _, n) => ((StatusChip)b)._label.Text = n as string);

    public static readonly BindableProperty ChipColorProperty =
        BindableProperty.Create(nameof(ChipColor), typeof(Color), typeof(StatusChip), null,
            propertyChanged: (b, _, n) => ((StatusChip)b)._border.BackgroundColor = n as Color);

    private readonly Label _label = new() { TextColor = Colors.White, FontSize = 12, FontAttributes = FontAttributes.Bold };
    private readonly Border _border;

    public StatusChip()
    {
        HorizontalOptions = LayoutOptions.Start;
        _border = new Border
        {
            BackgroundColor = Application.Current.GetColor("Gray500", Colors.Gray),
            StrokeThickness = 0,
            Padding = new Thickness(8, 2),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 10 },
            Content = _label,
        };
        Content = _border;
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Color? ChipColor
    {
        get => (Color?)GetValue(ChipColorProperty);
        set => SetValue(ChipColorProperty, value);
    }
}
