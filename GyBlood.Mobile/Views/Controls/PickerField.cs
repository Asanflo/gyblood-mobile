using System.Collections;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Liste déroulante des maquettes (libellé + zone grise). SelectedItem est liable en double sens.</summary>
public sealed class PickerField : ContentView
{
    public static readonly BindableProperty HeaderProperty =
        BindableProperty.Create(nameof(Header), typeof(string), typeof(PickerField), string.Empty,
            propertyChanged: (b, _, _) => ((PickerField)b).RefreshLabel());

    public static readonly BindableProperty IsRequiredProperty =
        BindableProperty.Create(nameof(IsRequired), typeof(bool), typeof(PickerField), false,
            propertyChanged: (b, _, _) => ((PickerField)b).RefreshLabel());

    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(nameof(Title), typeof(string), typeof(PickerField), string.Empty,
            propertyChanged: (b, _, n) => ((PickerField)b)._picker.Title = n as string);

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(nameof(ItemsSource), typeof(IList), typeof(PickerField), null,
            propertyChanged: (b, _, n) =>
            {
                var field = (PickerField)b;
                field._picker.ItemsSource = n as IList;
                field.SyncSelection();
            });

    public static readonly BindableProperty SelectedItemProperty =
        BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(PickerField), null, BindingMode.TwoWay,
            propertyChanged: (b, _, _) => ((PickerField)b).SyncSelection());

    private readonly Label _label = new()
    {
        FontSize = 13,
        FontAttributes = FontAttributes.Bold,
        IsVisible = false,
    };

    private readonly Picker _picker = new();

    public PickerField()
    {
        var border = new Border
        {
            Style = (Style)Application.Current!.Resources["FieldBorder"],
            Content = _picker,
        };

        Content = new VerticalStackLayout { Spacing = 6, Children = { _label, border } };
        _picker.SelectedIndexChanged += (_, _) =>
        {
            if (_picker.SelectedItem is not null && !Equals(_picker.SelectedItem, SelectedItem))
            {
                SelectedItem = _picker.SelectedItem;
            }
        };
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

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public IList? ItemsSource
    {
        get => (IList?)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    private void SyncSelection()
    {
        if (!Equals(_picker.SelectedItem, SelectedItem))
        {
            _picker.SelectedItem = SelectedItem;
        }
    }

    private void RefreshLabel()
    {
        _label.Text = IsRequired ? $"{Header} *" : Header;
        _label.IsVisible = !string.IsNullOrEmpty(Header);
    }
}
