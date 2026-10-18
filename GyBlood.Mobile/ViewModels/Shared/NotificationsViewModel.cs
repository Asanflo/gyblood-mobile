using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GyBlood.Mobile.Services.Api;
using GyBlood.Mobile.ViewModels.Base;
using GyBlood.Mobile.ViewModels.Items;

namespace GyBlood.Mobile.ViewModels.Shared;

/// <summary>Notifications de l'utilisateur ; toucher une notification la marque comme lue.</summary>
public partial class NotificationsViewModel : BaseViewModel
{
    private readonly INotificationApi _api;

    [ObservableProperty]
    private bool isEmpty;

    public NotificationsViewModel(INotificationApi api)
    {
        _api = api;
        Title = "Notifications";
    }

    public ObservableCollection<NotificationItem> Items { get; } = [];

    public override Task OnAppearingAsync() => LoadAsync();

    [RelayCommand]
    private Task LoadAsync() => RunAsync(async () =>
    {
        var page = await _api.GetNotificationsAsync(size: 50);
        Items.Clear();
        foreach (var dto in page.Content)
        {
            NotificationItem? item = null;
            item = new NotificationItem
            {
                Dto = dto,
                Title = dto.Title,
                Message = dto.Message,
                DateText = Ui.DayAndHour(dto.CreatedAt),
                IsUnread = !dto.Read,
                OpenCommand = new AsyncRelayCommand(() => MarkReadAsync(item!)),
            };
            Items.Add(item);
        }

        IsEmpty = Items.Count == 0;
    });

    private async Task MarkReadAsync(NotificationItem item)
    {
        if (!item.IsUnread)
        {
            return;
        }

        try
        {
            await _api.MarkReadAsync(item.Dto.Id);
            item.IsUnread = false;
        }
        catch (HttpRequestException)
        {
            // Sans réseau, la notification reste « non lue » : l'utilisateur pourra recommencer.
        }
    }
}
