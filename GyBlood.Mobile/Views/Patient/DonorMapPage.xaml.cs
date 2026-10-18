using System.ComponentModel;
using GyBlood.Mobile.ViewModels.Patient;

namespace GyBlood.Mobile.Views.Patient;

public partial class DonorMapPage : ContentPage
{
    private readonly DonorMapViewModel _viewModel;

    public DonorMapPage(DonorMapViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.OnAppearingAsync();
    }

    /// <summary>Recentre la carte quand une nouvelle position est utilisée pour la recherche.</summary>
    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(DonorMapViewModel.Center) && _viewModel.Center is { } point)
        {
            // Environ 6 m par pixel et par km de rayon : le cercle de recherche tient à l'écran.
            Map.CenterOn(point.Latitude, point.Longitude, _viewModel.RadiusKm * 6);
        }
    }
}
