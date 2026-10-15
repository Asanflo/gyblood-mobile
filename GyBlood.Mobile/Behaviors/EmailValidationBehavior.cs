using GyBlood.Mobile.Core.Extensions;
using GyBlood.Mobile.Core.Helpers;

namespace GyBlood.Mobile.Behaviors;

/// <summary>Colore le texte en rouge tant que le champ (email) est invalide ; ne bloque rien, la validation finale est dans le ViewModel.</summary>
public sealed class EmailValidationBehavior : Behavior<Entry>
{
    public bool IsValid { get; private set; }

    protected override void OnAttachedTo(Entry entry)
    {
        base.OnAttachedTo(entry);
        entry.TextChanged += OnTextChanged;
    }

    protected override void OnDetachingFrom(Entry entry)
    {
        entry.TextChanged -= OnTextChanged;
        base.OnDetachingFrom(entry);
    }

    private void OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
        {
            return;
        }

        IsValid = InputValidator.IsValidEmail(e.NewTextValue);
        var empty = string.IsNullOrEmpty(e.NewTextValue);
        entry.TextColor = empty || IsValid
            ? Application.Current.GetColor("Gray900", Colors.Black)
            : Application.Current.GetColor("Danger", Colors.Red);
    }
}
