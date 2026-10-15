using GyBlood.Mobile.Core.Extensions;
using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Pastille « A+ », « O- »... Liaison : Group et Rhesus.</summary>
public sealed class BloodGroupBadge : ContentView
{
    public static readonly BindableProperty GroupProperty =
        BindableProperty.Create(nameof(Group), typeof(BloodGroup), typeof(BloodGroupBadge), BloodGroup.O, propertyChanged: OnChanged);

    public static readonly BindableProperty RhesusProperty =
        BindableProperty.Create(nameof(Rhesus), typeof(Rhesus), typeof(BloodGroupBadge), Core.Enums.Rhesus.Positive, propertyChanged: OnChanged);

    private readonly Label _label = new()
    {
        TextColor = Colors.White,
        FontAttributes = FontAttributes.Bold,
        HorizontalTextAlignment = TextAlignment.Center,
    };

    public BloodGroupBadge()
    {
        HorizontalOptions = LayoutOptions.Start;
        Content = new Border
        {
            BackgroundColor = Application.Current.GetColor("Primary", Colors.DarkRed),
            StrokeThickness = 0,
            Padding = new Thickness(10, 4),
            StrokeShape = new Microsoft.Maui.Controls.Shapes.RoundRectangle { CornerRadius = 12 },
            Content = _label,
        };
        Refresh();
    }

    public BloodGroup Group
    {
        get => (BloodGroup)GetValue(GroupProperty);
        set => SetValue(GroupProperty, value);
    }

    public Rhesus Rhesus
    {
        get => (Rhesus)GetValue(RhesusProperty);
        set => SetValue(RhesusProperty, value);
    }

    private static void OnChanged(BindableObject bindable, object oldValue, object newValue)
        => ((BloodGroupBadge)bindable).Refresh();

    private void Refresh() => _label.Text = BloodCompatibility.Format(Group, Rhesus);
}
