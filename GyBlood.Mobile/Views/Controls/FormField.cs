namespace GyBlood.Mobile.Views.Controls;

/// <summary>Champ de saisie des maquettes : libellé au-dessus, zone grise arrondie. Text est liable en double sens.</summary>
public sealed class FormField : ContentView
{
    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(nameof(Header), typeof(string), typeof(FormField), string.Empty,
            propertyChanged: (b, _, _) => ((FormField)b).RefreshLabel());

    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(FormField), false,
            propertyChanged: (b, _, _) => ((FormField)b).RefreshLabel());

    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(FormField), string.Empty,
            propertyChanged: (b, _, n) => ((FormField)b)._entry.Placeholder = n as string);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(FormField), string.Empty, BindingMode.TwoWay,
            propertyChanged: (b, _, n) =>
            {
                var entry = ((FormField)b)._entry;
                if (entry.Text != (n as string))
                {
                    entry.Text = n as string;
                }
            });

    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(FormField), false,
            propertyChanged: (b, _, n) => ((FormField)b)._entry.IsPassword = (bool)n);

    public static readonly BindableProperty KeyboardTypeProperty =
        BindableProperty.Create(nameof(KeyboardType), typeof(Keyboard), typeof(FormField), Keyboard.Default,
            propertyChanged: (b, _, n) => ((FormField)b)._entry.Keyboard = (Keyboard)n);

    public static readonly BindableProperty MaxLengthProperty =
        BindableProperty.Create(nameof(MaxLength), typeof(int), typeof(FormField), int.MaxValue,
            propertyChanged: (b, _, n) => ((FormField)b)._entry.MaxLength = (int)n);

    public static readonly BindableProperty IsReadOnlyProperty =
        BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(FormField), false,
            propertyChanged: (b, _, n) => ((FormField)b)._entry.IsReadOnly = (bool)n);

    private readonly Label _label = new()
    {
        FontSize = 13,
        FontAttributes = FontAttributes.Bold,
        IsVisible = false,
    };

    private readonly Entry _entry = new();

    public FormField()
    {
        var border = new Border
        {
            Style = (Style)Application.Current!.Resources["FieldBorder"],
            Content = _entry,
        };

        Content = new VerticalStackLayout { Spacing = 6, Children = { _label, border } };
        _entry.TextChanged += (_, e) => Text = e.NewTextValue ?? string.Empty;
    }

    public string Header
    {
        get => (string)GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public bool IsRequired
    {
        get => (bool)GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }

    public Keyboard KeyboardType
    {
        get => (Keyboard)GetValue(KeyboardTypeProperty);
        set => SetValue(KeyboardTypeProperty, value);
    }

    public int MaxLength
    {
        get => (int)GetValue(MaxLengthProperty);
        set => SetValue(MaxLengthProperty, value);
    }

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    private void RefreshLabel()
    {
        _label.Text = IsRequired ? $"{Header} *" : Header;
        _label.IsVisible = !string.IsNullOrEmpty(Header);
    }
}
