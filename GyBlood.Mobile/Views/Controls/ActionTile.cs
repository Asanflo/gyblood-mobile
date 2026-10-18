using System.Windows.Input;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Tuile d'action de l'accueil (icône, titre, sous-titre). Le toucher exécute Command.</summary>
public sealed class ActionTile : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(ActionTile), string.Empty,
            propertyChanged: (b, _, n) => ((ActionTile)b)._title.Text = n as string);

    public static readonly BindableProperty SubtitleProperty =
        BindableProperty.Create(nameof(Subtitle), typeof(string), typeof(ActionTile), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var tile = (ActionTile)b;
                tile._subtitle.Text = n as string;
                tile._subtitle.IsVisible = !string.IsNullOrEmpty(n as string);
            });

    public static readonly BindableProperty IconProperty =
        BindableProperty.Create(nameof(Icon), typeof(string), typeof(ActionTile), string.Empty,
            propertyChanged: (b, _, n) =>
            {
                var name = n as string;
                ((ActionTile)b)._icon.Source = string.IsNullOrEmpty(name) ? null : ImageSource.FromFile(name);
            });

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ActionTile), null);

    private readonly Label _title = new() { FontAttributes = FontAttributes.Bold, FontSize = 14, HorizontalTextAlignment = TextAlignment.Center };
    private readonly Label _subtitle = new() { FontSize = 12, HorizontalTextAlignment = TextAlignment.Center, IsVisible = false };
    private readonly Image _icon = new() { WidthRequest = 36, HeightRequest = 36, Aspect = Aspect.AspectFit, HorizontalOptions = LayoutOptions.Center };

    public ActionTile()
    {
        var border = new Border
        {
            Style = (Style)Application.Current!.Resources["Card"],
            Padding = new Thickness(10, 14),
            MinimumHeightRequest = 104,
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                VerticalOptions = LayoutOptions.Center,
                Children = { _icon, _title, _subtitle },
            },
        };

        _subtitle.SetDynamicResource(Label.TextColorProperty, "Gray600");

        var tap = new TapGestureRecognizer();
        tap.SetBinding(TapGestureRecognizer.CommandProperty, new Binding(nameof(Command), source: this));
        border.GestureRecognizers.Add(tap);
        Content = border;
    }

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string Subtitle
    {
        get => (string)GetValue(SubtitleProperty);
        set => SetValue(SubtitleProperty, value);
    }

    public string Icon
    {
        get => (string)GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public ICommand? Command
    {
        get => (ICommand?)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
}
