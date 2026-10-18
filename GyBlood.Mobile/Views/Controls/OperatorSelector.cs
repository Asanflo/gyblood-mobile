using GyBlood.Mobile.Core.Extensions;
using Microsoft.Maui.Controls.Shapes;

namespace GyBlood.Mobile.Views.Controls;

/// <summary>Choix de l'opérateur Mobile Money : deux cartes (MTN MoMo, Orange Money). Selected est liable en double sens.</summary>
public sealed class OperatorSelector : ContentView
{
    public static readonly BindableProperty SelectedProperty =
        BindableProperty.Create(nameof(Selected), typeof(PaymentOperator), typeof(OperatorSelector), PaymentOperator.MtnMomo,
            BindingMode.TwoWay, propertyChanged: (b, _, _) => ((OperatorSelector)b).Refresh());

    private readonly Border _mtn;
    private readonly Border _orange;

    public OperatorSelector()
    {
        _mtn = Build("MTN MoMo", "Logo MTN", PaymentOperator.MtnMomo);
        _orange = Build("Orange Money", "Logo Orange", PaymentOperator.OrangeMoney);

        var grid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 12,
        };
        grid.Add(_mtn, 0);
        grid.Add(_orange, 1);
        Content = grid;
        Refresh();
    }

    public PaymentOperator Selected
    {
        get => (PaymentOperator)GetValue(SelectedProperty);
        set => SetValue(SelectedProperty, value);
    }

    private Border Build(string name, string logoCaption, PaymentOperator op)
    {
        var card = new Border
        {
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
            StrokeThickness = 1.5,
            Padding = new Thickness(12),
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                HorizontalOptions = LayoutOptions.Center,
                Children =
                {
                    new ImageFrame { Caption = logoCaption, HeightRequest = 36, WidthRequest = 72, CornerRadius = 8 },
                    new Label { Text = name, FontAttributes = FontAttributes.Bold, HorizontalTextAlignment = TextAlignment.Center },
                },
            },
        };
        card.GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => Selected = op) });
        return card;
    }

    private void Refresh()
    {
        Apply(_mtn, Selected == PaymentOperator.MtnMomo);
        Apply(_orange, Selected == PaymentOperator.OrangeMoney);
    }

    private static void Apply(Border card, bool selected)
    {
        var app = Application.Current;
        card.Stroke = selected ? app.GetColor("Primary", Colors.Red) : app.GetColor("Stroke", Colors.LightGray);
        card.BackgroundColor = selected ? app.GetColor("PrimaryLight", Colors.MistyRose) : app.GetColor("White", Colors.White);
    }
}
