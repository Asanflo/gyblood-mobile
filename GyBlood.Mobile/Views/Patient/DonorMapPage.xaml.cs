using System.Collections.Specialized;
using System.ComponentModel;
using GyBlood.Mobile.ViewModels.Patient;
using GyBlood.Mobile.Views.Controls;

namespace GyBlood.Mobile.Views.Patient;

public partial class DonorMapPage : ContentPage
{
    private readonly DonorMapViewModel _viewModel;

    public DonorMapPage(DonorMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
        _viewModel.Donors.CollectionChanged += OnDonorsChanged;
        Map.DonorTapped += OnDonorTapped;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearingAsync();
    }

    /// <summary>Recentre la carte et place le point bleu quand une nouvelle position est utilisée.</summary>
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DonorMapViewModel.Center) && _viewModel.Center is { } point)
        {
            Map.SetMyPosition(point.Latitude, point.Longitude);
            // Environ 6 m par pixel et par km de rayon : le cercle de recherche tient à l'écran.
            Map.CenterOn(point.Latitude, point.Longitude, _viewModel.RadiusKm * 6);
        }
    }

    /// <summary>Met à jour les marqueurs quand la liste de donneurs change.</summary>
    private void OnDonorsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Map.SetDonors(_viewModel.Donors.Select(d => new MapDonor(
            d.Dto.Id, d.Dto.Latitude, d.Dto.Longitude, d.BloodText)));
    }

    /// <summary>Toucher sur un marqueur : ouvre la fiche du donneur.</summary>
    private void OnDonorTapped(object? sender, long donorId)
    {
        var item = _viewModel.Donors.FirstOrDefault(d => d.Dto.Id == donorId);
        item?.OpenCommand?.Execute(null);
    }
}