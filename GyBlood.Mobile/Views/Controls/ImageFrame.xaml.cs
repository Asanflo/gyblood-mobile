using Microsoft.Maui.Controls.Shapes;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Cadre d'image réutilisable : affiche un emplacement pointillé tant qu'aucune image n'est renseignée.</summary>
public partial class ImageFrame : ContentView
{
    public static readonly BindableProperty SourceProperty =
        BindableProperty.Create(nameof(Source), typeof(string), typeof(ImageFrame), string.Empty,
            propertyChanged: (b, _, _) => ((ImageFrame)b).Refresh());

    public static readonly BindableProperty CaptionProperty =
        BindableProperty.Create(nameof(Caption), typeof(string), typeof(ImageFrame), "Image",
            propertyChanged: (b, _, n) => ((ImageFrame)b).CaptionLabel.Text = n as string);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(ImageFrame), 12d,
            propertyChanged: (b, _, n) => ((ImageFrame)b).ApplyRadius((double)n));

    public static readonly BindableProperty AspectProperty =
        BindableProperty.Create(nameof(Aspect), typeof(Aspect), typeof(ImageFrame), Aspect.AspectFill,
            propertyChanged: (b, _, n) => ((ImageFrame)b).Picture.Aspect = (Aspect)n);

    public ImageFrame()
    {
        InitializeComponent();
    }

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public string Caption
    {
        get => (string)GetValue(CaptionProperty);
        set => SetValue(CaptionProperty, value);
    }

    public double CornerRadius
    {
        get => (double)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public Aspect Aspect
    {
        get => (Aspect)GetValue(AspectProperty);
        set => SetValue(AspectProperty, value);
    }

    private void Refresh()
    {
        var hasImage = !string.IsNullOrWhiteSpace(Source);
        Picture.Source = hasImage
            ? Source.StartsWith("http", StringComparison.OrdinalIgnoreCase) ? ImageSource.FromUri(new Uri(Source)) : ImageSource.FromFile(Source)
            : null;
        PictureHost.IsVisible = hasImage;
        Placeholder.IsVisible = !hasImage;
    }

    private void ApplyRadius(double radius)
    {
        Placeholder.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(radius) };
        PictureHost.StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(radius) };
    }
}
